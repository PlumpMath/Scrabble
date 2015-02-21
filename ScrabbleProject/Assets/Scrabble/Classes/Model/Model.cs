using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
	using Ext;

	[FlagsAttribute]
	public enum Letter
	{
		Invalid		= 0x0,

		// 1 pt
		E 			= 0x1 << 0,
		A 			= 0x1 << 1,
		I 			= 0x1 << 2,
		O 			= 0x1 << 3,
		N 			= 0x1 << 4,
		R 			= 0x1 << 5,
		T 			= 0x1 << 6,
		L 			= 0x1 << 7,
		S 			= 0x1 << 8,
		U 			= 0x1 << 9,

		// 2 pt
		D 			= 0x1 << 10,
		G 			= 0x1 << 11,

		// 3 pt
		B 			= 0x1 << 12,
		C 			= 0x1 << 13,
		M 			= 0x1 << 14,
		P 			= 0x1 << 15,

		// 4 pt
		F 			= 0x1 << 16,
		H 			= 0x1 << 17,
		V 			= 0x1 << 18,
		W 			= 0x1 << 19,
		Y 			= 0x1 << 20,

		// 5 pt
		K 			= 0x1 << 21,

		// 8 pt
		J 			= 0x1 << 22,
		X 			= 0x1 << 23,

		// 10 pt
		Q 			= 0x1 << 24,
		Z 			= 0x1 << 25,

		// 0 pt
		Blank 		= 0x1 << 26,
		Max			= 0x1 << 27,
	};

	[FlagsAttribute]
	public enum TileType
	{
		Invalid		= 0x0,

		BK			= 0x1 << 0, // Blank
		TW			= 0x1 << 1,
		DW			= 0x1 << 2,
		TL			= 0x1 << 3,
		DL			= 0x1 << 4,
		ST			= 0x1 << 5, // Start

		Max			= 0x1 << 6,
	};

	public sealed class Model
	{
		private static Model m_instance = null;
		public static Model Instance 
		{ 
			get
			{
				if (m_instance == null)
				{
					m_instance = new Model();
				}

				return m_instance;
			}
		}

		public Model ()
		{
			this.Board = new BOARD();
		}

		public BOARD Board { get; private set; }
	}

	public sealed class BOARD
	{
		public static readonly int TILE_COUNT = 225;
		public static readonly int BOARD_ROWS = 15;
		public static readonly int BOARD_COLS = 15;
		public static readonly float TILE_WIDTH = 50f; // in pixel
		public static readonly float TILE_HEIGHT = 50f;
		public static readonly float TILE_OFFSET = 0.5f; // in unity world
		public static readonly float TILE_SCREEN_OFFSET = 7f * 0.5f;//((float)BOARD_ROWS * TILE_OFFSET) * 0.2f;

		private Dictionary<Letter, int> m_letterPoints = new Dictionary<Letter, int>()
		{
			// 1 pt
			{ Letter.E, 			1 },
			{ Letter.A, 			1 },
			{ Letter.I, 			1 },
			{ Letter.O, 			1 },
			{ Letter.N, 			1 },
			{ Letter.R, 			1 },
			{ Letter.T, 			1 },
			{ Letter.L, 			1 },
			{ Letter.S, 			1 },
			{ Letter.U, 			1 },
					  
			// 2 pt   
			{ Letter.D, 			2 },
			{ Letter.G, 			2 },
					  
			// 3 pt   
			{ Letter.B, 			3 },
			{ Letter.C, 			3 },
			{ Letter.M, 			3 },
			{ Letter.P, 			3 },
					  
			// 4 pt   
			{ Letter.F, 			4 },
			{ Letter.H, 			4 },
			{ Letter.V, 			4 },
			{ Letter.W, 			4 },
			{ Letter.Y, 			4 },
					  
			// 5 pt   
			{ Letter.K, 			5 },
					  
			// 8 pt   
			{ Letter.J, 			8 },
			{ Letter.X, 			8 },

			// 10 pt  
			{ Letter.Q, 			10 },
			{ Letter.Z, 			10 },

			// 0 pt
			{ Letter.Blank, 		0 },
		};

		private Dictionary<Letter, int> m_letterCount = new Dictionary<Letter, int>()
		{
			// 1 pt
			{ Letter.E, 			12 },
			{ Letter.A, 			9 },
			{ Letter.I, 			9 },
			{ Letter.O, 			8 },
			{ Letter.N, 			6 },
			{ Letter.R, 			6 },
			{ Letter.T, 			6 },
			{ Letter.L, 			4 },
			{ Letter.S, 			4 },
			{ Letter.U, 			4 },
			
			// 2 pt   
			{ Letter.D, 			4 },
			{ Letter.G, 			3 },
			
			// 3 pt   
			{ Letter.B, 			2 },
			{ Letter.C, 			2 },
			{ Letter.M, 			2 },
			{ Letter.P, 			2 },
			
			// 4 pt   
			{ Letter.F, 			2 },
			{ Letter.H, 			2 },
			{ Letter.V, 			2 },
			{ Letter.W, 			2 },
			{ Letter.Y, 			2 },
			
			// 5 pt   
			{ Letter.K, 			1 },
			
			// 8 pt   
			{ Letter.J, 			1 },
			{ Letter.X, 			1 },
			
			// 10 pt  
			{ Letter.Q, 			1 },
			{ Letter.Z, 			1 },
			
			// 0 pt
			{ Letter.Blank, 		2 },
		};

		private Dictionary<TileType, int> m_tileCount = new Dictionary<TileType, int>()
		{
			{ TileType.BK, 		198 },
			{ TileType.TW, 		8 },
			{ TileType.DW, 		12 },
			{ TileType.TL, 		12 },
			{ TileType.DL, 		24 },
			{ TileType.ST, 		1 },
		};

		private Dictionary<TileType, string> m_tileSprite = new Dictionary<TileType, string>()
		{
			{ TileType.BK, 		"tile_empty" },
			{ TileType.TW, 		"tile_TW" },
			{ TileType.DW, 		"tile_DW" },
			{ TileType.TL, 		"tile_TL" },
			{ TileType.DL, 		"tile_DL" },
			{ TileType.ST, 		"tile_DW" },
		};

		public static TileType[,] m_boardMap = new TileType[15, 15]
		{
			{ TileType.TW, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.TW, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.TW },
			{ TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK },
			{ TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK },
			{ TileType.DL, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.DL },
			{ TileType.BK, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.BK },
			{ TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK },
			{ TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK },
			{ TileType.TW, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.TW },
			{ TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK },
			{ TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK },
			{ TileType.BK, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.BK },
			{ TileType.DL, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.DL },
			{ TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK, TileType.BK },
			{ TileType.BK, TileType.DW, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.TL, TileType.BK, TileType.BK, TileType.BK, TileType.DW, TileType.BK },
			{ TileType.TW, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.BK, TileType.TW, TileType.BK, TileType.BK, TileType.BK, TileType.DL, TileType.BK, TileType.BK, TileType.TW },
		};

		public int LetterPoints (Letter p_letter)
		{
			return m_letterPoints[p_letter];
		}

		public int LetterCount (Letter p_letter)
		{
			return m_letterCount[p_letter];
		}

		public int TileCount (TileType p_tile)
		{
			return m_tileCount[p_tile];
		}

		public string TileSprite (TileType p_tile)
		{
			return m_tileSprite[p_tile];
		}

		public TileType[,] Map { get { return m_boardMap; } }

		public TileType MapFrom (int p_row, int p_col)
		{
			//this.Log(Tags.Log, "BOARD::MapFrom row:{0} col:{1}", p_row, p_col);
			return m_boardMap[p_row, p_col];
		}
	}
}