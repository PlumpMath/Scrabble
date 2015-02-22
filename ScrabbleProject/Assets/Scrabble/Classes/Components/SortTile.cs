using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	public enum Sort
	{
		Ascending,
		Descending,
	};

	/// <summary>
	/// Sorts tile based on Row
	/// </summary>
	public class SortTileR : IComparer<Tile>
	{
		private Sort m_sortType;
		
		public SortTileR (Sort p_sort)
		{
			m_sortType = p_sort;
		}
		
		#region IComparer<PokerHandResult> Hands
		public int Compare (Tile x, Tile y)
		{
			if (m_sortType == Sort.Ascending) { return this.CompareAsc(x, y); }
			return this.CompareDes(x, y);
		}
		
		public int CompareAsc (Tile x, Tile y)
		{
			if ( x.TileModel.Row > y.TileModel.Row ) { return 1; }
			else if ( x.TileModel.Row < y.TileModel.Row ) { return -1; }
			return 0;
		}
		
		public int CompareDes (Tile x, Tile y)
		{
			if ( x.TileModel.Row < y.TileModel.Row ) { return 1; }
			else if ( x.TileModel.Row > y.TileModel.Row ) { return -1; }
			return 0;
		}
		#endregion
	}

	/// <summary>
	/// Sorts tile based on Col
	/// </summary>
	public class SortTileC : IComparer<Tile>
	{
		private Sort m_sortType;
		
		public SortTileC (Sort p_sort)
		{
			m_sortType = p_sort;
		}
		
		#region IComparer<PokerHandResult> Hands
		public int Compare (Tile x, Tile y)
		{
			if (m_sortType == Sort.Ascending) { return this.CompareAsc(x, y); }
			return this.CompareDes(x, y);
		}
		
		public int CompareAsc (Tile x, Tile y)
		{
			if ( x.TileModel.Col > y.TileModel.Col ) { return 1; }
			else if ( x.TileModel.Col < y.TileModel.Col ) { return -1; }
			return 0;
		}
		
		public int CompareDes (Tile x, Tile y)
		{
			if ( x.TileModel.Col < y.TileModel.Col ) { return 1; }
			else if ( x.TileModel.Col > y.TileModel.Col ) { return -1; }
			return 0;
		}
		#endregion
	}
}