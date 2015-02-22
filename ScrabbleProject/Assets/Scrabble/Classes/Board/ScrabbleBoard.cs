﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Board
{
	using Ext;
	using Events;
	using Model;
	using MGTools;

	public class ScrabbleBoard : MonoBehaviour 
	{
		// Filters
		private Predicate<Tile> ACTIVE = new Predicate<Tile>(t => t.TileModel.Status == ETileStatus.Open);
		/// <summary>
		/// OCCUPIED | POCCUPIED
		/// </summary>
		private Predicate<Tile> OCCUPIED = new Predicate<Tile>(t => ETileStatus.NOT_EMPTY.Has(t.Status));
		/// <summary>
		/// Temp occupied. OCCUPIED
		/// </summary>
		private Predicate<Tile> TOCCUPIED = new Predicate<Tile>(t => t.Status == ETileStatus.Occupied);
		/// <summary>
		/// Permanently occupied. POCCUPIED
		/// </summary>
		private Predicate<Tile> POCCUPIED = new Predicate<Tile>(t => t.Status == ETileStatus.POccupied);

		[SerializeField] private Tile m_tile;
		private Tile[,] m_tileGrid;
		private List<Tile> m_tiles;
		private Model m_model;

		private void Awake ()
		{
			this.Assert<Tile>(m_tile, "m_tile must never be null!");

			m_model = Model.Instance;
			m_model.Scrabble = this;
			m_tileGrid = new Tile[BOARD.BOARD_ROWS, BOARD.BOARD_COLS];
			m_tiles = new List<Tile>();

			this.InitializeBoard();

			// initialize event
			ScrabbleEvent.Instance.OnTriggerEvent += this.OnEventListened;
		}

		private void Start ()
		{
			this.InitializeActiveTiles();
		}
		
		private void OnDestroy ()
		{
			ScrabbleEvent.Instance.OnTriggerEvent -= this.OnEventListened;
		}

		private void InitializeBoard ()
		{
			// show peg
			m_tile.gameObject.SetActive(true);

			for (int row = 0; row < BOARD.BOARD_ROWS; row++)
			{
				for (int col = 0; col < BOARD.BOARD_COLS; col++)
				{
					// generate tile
					Tile tile = this.CreateBoardTile(m_tile, ETileType.BK);
					tile.name = "Tile_" + row + "_" + col;
					tile.transform.parent = this.transform;

					// adjust position
					Vector3 position = m_tile.transform.position;
					position.x = (col * BOARD.TILE_OFFSET) - BOARD.TILE_SCREEN_OFFSET;
					position.y = (row * BOARD.TILE_OFFSET) - BOARD.TILE_SCREEN_OFFSET;
					tile.transform.position = position;

					// preload skin
					ETileType type = m_model.Board.MapFrom(row, col);
					tile.PreloadSkin(type, row, col);

					// set tile
					m_tileGrid[row, col] = tile;
					m_tiles.Add(tile);
				}
			}

			// hide peg
			m_tile.gameObject.SetActive(false);
		}

		private void InitializeActiveTiles ()
		{
			// deactivate all tiles
			foreach (Tile tile in m_tiles)
			{
				tile.Deactivate();
			}

			// activate initial tile
			m_tileGrid[m_model.Default.Row, m_model.Default.Col].Activate();
		}

		private void OnEventListened (EEvents p_type, IEventData p_data)
		{
			switch (p_type)
			{
				case EEvents.OnDrop:
				{
					DropEvent drop = (DropEvent)p_data;
					Vector3 pos = drop.Data<Vector3>(DropEvent.POSITION);
					Vector2 newPos = new Vector2(pos.x, pos.y);
					Letter letter = drop.Data<Letter>(DropEvent.LETTER);

					List<Tile> activeTiles = m_tiles.FindAll(ACTIVE);
					bool snapped = false;
					
					foreach (Tile tile in activeTiles)
					{
						//bool contains = tile.Rect.Contains(newPos);
						// +AS:02222015 Note: For some reason, min and size has the correct values. so we used them instead.
						Rect rect = tile.Rect;
						bool contains = rect.min.x <= newPos.x &&
										rect.min.y <= newPos.y &&
										rect.size.x >= newPos.x &&
										rect.size.y >= newPos.y;

						if (contains)
						{
							// if letter is already placed on a tile, unsnap it!
							if (letter.Tile != null)
							{
								break;
							}
							// snap the tile!
							else
							{
								//this.Log(Tags.Log, "Snap!");
								snapped = true;
								
								// TODO: Trigger Snapping
								ScrabbleEvent.Instance.Trigger(EEvents.OnSnapped, new SnapEvent(tile, letter.Type));
								
								// Cleanup rack
								ScrabbleEvent.Instance.Trigger(EEvents.OnCleanUpRack, new SnapEvent(tile, letter.Type));

								// TODO: Trigger active neighbor tiles!
								this.EnableNeighbors();

								break;
							}
						}
					}

					if (!snapped)
					{
						if (letter.Tile == null)
						{
							letter.Reset();
						}
						else
						{
							Model.Instance.Rack.AddLetter(letter);
						}

						this.EnableNeighbors();
					}
				}
				break;
			}
		}

		[Signal]
		private void OnPressedButton (MGButton p_button)
		{
			//this.Log(Tags.Log, "ScrabbleBoard::OnPressedButton Button:{0}", p_button.Button);
			EButton button = p_button.Button;

			switch (button)
			{
				case EButton.Pass:
				{
					ScrabbleEvent.Instance.Trigger(EEvents.OnPressedPass, null);
				}
				break;

				case EButton.Submit:
				{
					// DONE: Add Find the Left/Top most active tile!
					//	TopMost: row-14
					//	LeftMost: col-0
					// TODO: Integrated nearby (1 tile distance) POccupied tile
					List<Tile> occupiedR = m_tiles.FindAll(TOCCUPIED);
					List<Tile> occupiedC = new List<Tile>();
					occupiedC.AddRange(occupiedR);
					
					if (occupiedC.Count <= 0 || occupiedR.Count <= 0)
					{
						this.Log(Tags.Log, "ScrabbleBoard::OnPressedButton SUBMIT No letters to check!");
						return;
					}

					// sort the tiles
					occupiedR.Sort(new SortTileR(Sort.Descending));
					occupiedC.Sort(new SortTileC(Sort.Ascending)); 

					// top to bottom
					Tile top = occupiedR[0];
					Tile left = occupiedC[0];
					Tile nextToTop = this.TileBotOf(top);
					Tile nextToLeft = this.TileRightOf(left);

					// check verticale
					if (nextToTop != null && ETileStatus.NOT_EMPTY.Has(nextToTop.Status))
					{
						this.Log(Tags.Log, "ScrabbleBoard::OnPressedButton SUBMIT Check Vertical Word!");
						this.CheckVerticalWord(top.TileModel.Row, top.TileModel.Col);
					}
					// check horizontal
					else if (nextToLeft != null && ETileStatus.NOT_EMPTY.Has(nextToLeft.Status))
					{
						this.Log(Tags.Log, "ScrabbleBoard::OnPressedButton SUBMIT Check Horizontal Word!");
						this.CheckHorizontalWord(left.TileModel.Row, left.TileModel.Col);
					}
					// single/no letter letter
					else
					{
						this.Log(Tags.Log, "ScrabbleBoard::OnPressedButton SUBMIT Single letter!");
					}
				}
				break;
			}
		}

		private void EnableNeighbors ()
		{
			// deactivate inactive tiles
			this.DisableNeighbors();

			List<Tile> occupiedTiles = m_tiles.FindAll(OCCUPIED);

			foreach (Tile tile in occupiedTiles)
			{
				this.EnableNeighbors(tile.TileModel.Row, tile.TileModel.Col, tile);
			}
		}

		/// <summary>
		/// Flood fill (1x1 neighbor)
		/// </summary>
		private void EnableNeighbors (int p_row, int p_col, Tile p_tile)
		{
			for (int row = -1; row < 2; row++)
			{
				for (int col = -1; col < 2; col++)
				{
					// blocked tiles (center, TL, TR, BL, BR)
					if (row == 0 && col == 0) { continue; }
					if (row == 1 && col == 1) { continue; }
					if (row == -1 && col == -1) { continue; }
					if (row == 1 && col == -1) { continue; }
					if (row == -1 && col == 1) { continue; }

					int nRow = p_tile.TileModel.Row + row;
					int nCol = p_tile.TileModel.Col + col;

					// blocked out of bounds
					if (nRow < 0 || nRow > BOARD.BOARD_ROWS - 1) { continue; }
					if (nCol < 0 || nCol > BOARD.BOARD_COLS - 1) { continue; }

					Tile tile = m_tileGrid[nRow, nCol];

					// activate unoccupied tiles
					if (tile.TileModel.Letter == null)
					{
						tile.Activate();
					}
				}
			}
		}

		private void DisableNeighbors ()
		{
			foreach (Tile tile in m_tiles)
			{
				if (tile.TileModel.Letter == null)
				{
					tile.Deactivate();
				}
			}
			
			// initial tile
			Tile initTile = m_tileGrid[m_model.Default.Row, m_model.Default.Col];
			if (initTile.TileModel.Letter == null)
			{
				initTile.Activate();
			}
		}
		
		private Tile TileBotOf (Tile p_tile)
		{
			int row = p_tile.TileModel.Row-1;
			int col = p_tile.TileModel.Col;

			if (row < 0 || row > BOARD.BOARD_COLS - 1) { return null; }

			return m_tileGrid[row, col];
		}

		private Tile TileRightOf (Tile p_tile)
		{
			int row = p_tile.TileModel.Row;
			int col = p_tile.TileModel.Col+1;
			
			if (col < 0 || col > BOARD.BOARD_COLS - 1) { return null; }
			
			return m_tileGrid[row, col];
		}

		/// <summary>
		/// Word Validation (Horizontal)
		/// </summary>
		private void CheckHorizontalWord (int p_row, int p_col)
		{
			Tile tile = m_tileGrid[p_row, p_col];

			string word = Model.Instance.Board.LetterText(tile.TileModel.Letter.Type);

			while (true)
			{
				int newCol = tile.TileModel.Col + 1;
				tile = m_tileGrid[p_row, newCol];

				if (!ETileStatus.NOT_EMPTY.Has(tile.Status)) { break; }

				word += Model.Instance.Board.LetterText(tile.TileModel.Letter.Type);
			}

			this.Log(Tags.Log, "ScrabbleBoard::CheckHorizontal Word:{0}", word);
		}

		/// <summary>
		/// Word Validation (Vertical)
		/// </summary>
		private void CheckVerticalWord (int p_row, int p_col)
		{
			Tile tile = m_tileGrid[p_row, p_col];
			
			string word = Model.Instance.Board.LetterText(tile.TileModel.Letter.Type);
			
			while (true)
			{
				int newRow = tile.TileModel.Row - 1;
				tile = m_tileGrid[newRow, p_col];
				
				if (!ETileStatus.NOT_EMPTY.Has(tile.Status)) { break; }
				
				word += Model.Instance.Board.LetterText(tile.TileModel.Letter.Type);
			}
			
			this.Log(Tags.Log, "ScrabbleBoard::CheckHorizontal Word:{0}", word);
		}
	}
}