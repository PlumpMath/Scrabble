using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Board
{
	using Ext;
	using Model;

	public class ScrabbleBoard : MonoBehaviour 
	{
		[SerializeField] private Tile m_tile;
		private Model m_model;

		private void Awake ()
		{
			this.Assert<Tile>(m_tile, "m_tile must never be null!");
			m_model = Model.Instance;
			this.InitializeBoard();
		}
		
		private void OnDestroy ()
		{
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
					tile.name = "Tile_" + col + "_" + row;
					tile.transform.parent = this.transform;

					// adjust position
					Vector3 position = m_tile.transform.position;
					position.x = (col * BOARD.TILE_OFFSET) - BOARD.TILE_SCREEN_OFFSET;
					position.y = (row * BOARD.TILE_OFFSET) - BOARD.TILE_SCREEN_OFFSET;
					tile.transform.position = position;

					// preload skin
					tile.PreloadSkin(m_model.Board.MapFrom(row, col));
				}
			}

			// hide peg
			m_tile.gameObject.SetActive(false);
		}
	}
}