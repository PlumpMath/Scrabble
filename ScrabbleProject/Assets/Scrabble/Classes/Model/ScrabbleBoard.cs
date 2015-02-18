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

			for (int row = 0; row <= BOARD.BOARD_ROWS; row++)
			{
				for (int col = 0; col <= BOARD.BOARD_COLS; col++)
				{
					Tile tile = this.CreateTile(m_tile, TileType.BK);
					tile.name = "Tile_" + col + "_" + row;
					tile.transform.parent = this.transform;
					Vector3 position = m_tile.transform.position;
					position.x = (col * BOARD.TILE_OFFSET);// - BOARD.TILE_SCREEN_OFFSET;
					position.y = (row * BOARD.TILE_OFFSET);// - BOARD.TILE_SCREEN_OFFSET;
					tile.transform.position = position;
				}
			}

			// hide peg
			m_tile.gameObject.SetActive(false);
		}

		private Tile CreateTile (Tile p_peg, TileType p_type)			         
		{
			GameObject tile = (GameObject)GameObject.Instantiate(p_peg.gameObject);
			return tile.GetComponent<Tile>();
		}
	}
}