using System;
using System.Collections;
using System.Collections.Generic;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	/// <summary>
	/// Sorts tile based on Row
	/// </summary>
	public class SortEffects : IComparer<EScrabbleEffects>
	{
		private Sort m_sortType;
		
		public SortEffects (Sort p_sort)
		{
			m_sortType = p_sort;
		}
		
		#region IComparer<PokerHandResult> Hands
		public int Compare (EScrabbleEffects x, EScrabbleEffects y)
		{
			if (m_sortType == Sort.Ascending) { return this.CompareAsc(x, y); }
			return this.CompareDes(x, y);
		}
		
		public int CompareAsc (EScrabbleEffects x, EScrabbleEffects y)
		{
			if ( x > y ) { return 1; }
			else if ( x < y ) { return -1; }
			return 0;
		}
		
		public int CompareDes (EScrabbleEffects x, EScrabbleEffects y)
		{
			if ( x < y ) { return 1; }
			else if ( x > y ) { return -1; }
			return 0;
		}
		#endregion
	}
}
