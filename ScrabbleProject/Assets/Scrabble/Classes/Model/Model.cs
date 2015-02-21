using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
	using Ext;

	[FlagsAttribute]
	public enum ELetter
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
	public enum ETileType
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
		public static readonly float PIXEL_IN_METER = 200f;
		public static readonly float TILE_WIDTH = 50f; // in pixel
		public static readonly float TILE_HEIGHT = 50f;
		public static readonly float TILE_OFFSET = 0.5f; // in unity world
		public static readonly float TILE_SCREEN_OFFSET = 7f * 0.5f;//((float)BOARD_ROWS * TILE_OFFSET) * 0.2f;
		public static readonly float LETTER_WITH = 100.0f;
		public static readonly float LETTER_HEIGHT = 100.0f;

		private Dictionary<ELetter, string> m_letterText = new Dictionary<ELetter, string>()
		{
			// 1 pt
			{ ELetter.E, 			"E" },
			{ ELetter.A, 			"A" },
			{ ELetter.I, 			"I" },
			{ ELetter.O, 			"O" },
			{ ELetter.N, 			"N" },
			{ ELetter.R, 			"R" },
			{ ELetter.T, 			"T" },
			{ ELetter.L, 			"L" },
			{ ELetter.S, 			"S" },
			{ ELetter.U, 			"U" },
			
			// 2 pt   
			{ ELetter.D, 			"D" },
			{ ELetter.G, 			"G" },
			
			// 3 pt   
			{ ELetter.B, 			"B" },
			{ ELetter.C, 			"C" },
			{ ELetter.M, 			"M" },
			{ ELetter.P, 			"P" },
			
			// 4 pt   
			{ ELetter.F, 			"F" },
			{ ELetter.H, 			"H" },
			{ ELetter.V, 			"V" },
			{ ELetter.W, 			"W" },
			{ ELetter.Y, 			"Y" },
			
			// 5 pt   
			{ ELetter.K, 			"K" },
			
			// 8 pt   
			{ ELetter.J, 			"J" },
			{ ELetter.X, 			"X" },
			
			// 10 pt  
			{ ELetter.Q, 			"Q" },
			{ ELetter.Z, 			"Z" },
			
			// 0 pt
			{ ELetter.Blank, 		"" },
		};

		private Dictionary<ELetter, int> m_letterPoints = new Dictionary<ELetter, int>()
		{
			// 1 pt
			{ ELetter.E, 			1 },
			{ ELetter.A, 			1 },
			{ ELetter.I, 			1 },
			{ ELetter.O, 			1 },
			{ ELetter.N, 			1 },
			{ ELetter.R, 			1 },
			{ ELetter.T, 			1 },
			{ ELetter.L, 			1 },
			{ ELetter.S, 			1 },
			{ ELetter.U, 			1 },
					  
			// 2 pt   
			{ ELetter.D, 			2 },
			{ ELetter.G, 			2 },
					  
			// 3 pt   
			{ ELetter.B, 			3 },
			{ ELetter.C, 			3 },
			{ ELetter.M, 			3 },
			{ ELetter.P, 			3 },
					  
			// 4 pt   
			{ ELetter.F, 			4 },
			{ ELetter.H, 			4 },
			{ ELetter.V, 			4 },
			{ ELetter.W, 			4 },
			{ ELetter.Y, 			4 },
					  
			// 5 pt   
			{ ELetter.K, 			5 },
					  
			// 8 pt   
			{ ELetter.J, 			8 },
			{ ELetter.X, 			8 },

			// 10 pt  
			{ ELetter.Q, 			10 },
			{ ELetter.Z, 			10 },

			// 0 pt
			{ ELetter.Blank, 		0 },
		};

		private Dictionary<ELetter, int> m_letterCount = new Dictionary<ELetter, int>()
		{
			// 1 pt
			{ ELetter.E, 			12 },
			{ ELetter.A, 			9 },
			{ ELetter.I, 			9 },
			{ ELetter.O, 			8 },
			{ ELetter.N, 			6 },
			{ ELetter.R, 			6 },
			{ ELetter.T, 			6 },
			{ ELetter.L, 			4 },
			{ ELetter.S, 			4 },
			{ ELetter.U, 			4 },
			
			// 2 pt   
			{ ELetter.D, 			4 },
			{ ELetter.G, 			3 },
			
			// 3 pt   
			{ ELetter.B, 			2 },
			{ ELetter.C, 			2 },
			{ ELetter.M, 			2 },
			{ ELetter.P, 			2 },
			
			// 4 pt   
			{ ELetter.F, 			2 },
			{ ELetter.H, 			2 },
			{ ELetter.V, 			2 },
			{ ELetter.W, 			2 },
			{ ELetter.Y, 			2 },
			
			// 5 pt   
			{ ELetter.K, 			1 },
			
			// 8 pt   
			{ ELetter.J, 			1 },
			{ ELetter.X, 			1 },
			
			// 10 pt  
			{ ELetter.Q, 			1 },
			{ ELetter.Z, 			1 },
			
			// 0 pt
			{ ELetter.Blank, 		2 },
		};

		private Dictionary<ETileType, int> m_tileCount = new Dictionary<ETileType, int>()
		{
			{ ETileType.BK, 		198 },
			{ ETileType.TW, 		8 },
			{ ETileType.DW, 		12 },
			{ ETileType.TL, 		12 },
			{ ETileType.DL, 		24 },
			{ ETileType.ST, 		1 },
		};

		private Dictionary<ETileType, string> m_tileSprite = new Dictionary<ETileType, string>()
		{
			{ ETileType.BK, 		"tile_empty" },
			{ ETileType.TW, 		"tile_TW" },
			{ ETileType.DW, 		"tile_DW" },
			{ ETileType.TL, 		"tile_TL" },
			{ ETileType.DL, 		"tile_DL" },
			{ ETileType.ST, 		"tile_DW" },
		};

		public static ETileType[,] m_boardMap = new ETileType[15, 15]
		{
			{ ETileType.TW, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.TW },
			{ ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK },
			{ ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK },
			{ ETileType.DL, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.DL },
			{ ETileType.BK, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.BK },
			{ ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK },
			{ ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK },
			{ ETileType.TW, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.TW },
			{ ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK },
			{ ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK },
			{ ETileType.BK, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.BK },
			{ ETileType.DL, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.DL },
			{ ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK },
			{ ETileType.BK, ETileType.DW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DW, ETileType.BK },
			{ ETileType.TW, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.TW, ETileType.BK, ETileType.BK, ETileType.BK, ETileType.DL, ETileType.BK, ETileType.BK, ETileType.TW },
		};

		public int LetterPoints (ELetter p_letter)
		{
			return m_letterPoints[p_letter];
		}

		public int LetterCount (ELetter p_letter)
		{
			return m_letterCount[p_letter];
		}

		public string LetterText (ELetter p_letter)
		{
			return m_letterText[p_letter];
		}

		public int TileCount (ETileType p_tile)
		{
			return m_tileCount[p_tile];
		}

		public string TileSprite (ETileType p_tile)
		{
			return m_tileSprite[p_tile];
		}

		public ETileType[,] Map { get { return m_boardMap; } }

		public ETileType MapFrom (int p_row, int p_col)
		{
			//this.Log(Tags.Log, "BOARD::MapFrom row:{0} col:{1}", p_row, p_col);
			return m_boardMap[p_row, p_col];
		}
	}
}