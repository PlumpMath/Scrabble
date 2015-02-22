using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Events
{
	using Board;
	using Ext;
	using Model;

	public class SnapEvent : IEventData 
	{
		public static readonly string TILE = "tile";
		public static readonly string LETTER = "letter";
		
		private Dictionary<string, object> m_eventData;

		public SnapEvent (Tile p_tile, ELetter p_letter)
		{
			m_eventData = new Dictionary<string, object>();
			m_eventData.Add(TILE, p_tile);
			m_eventData.Add(LETTER, p_letter);
		}

		public string ToEventString () { return string.Empty; }
		
		public Dictionary<string, object> EventData ()
		{
			//return m_eventData.Clone<string, object>();
			return null;
		}
		
		public bool Has (string p_key)
		{
			return m_eventData.ContainsKey(p_key);
		}
		
		public T Data<T> (string p_key)
		{
			return (T)m_eventData[p_key];
		}
	}
}
