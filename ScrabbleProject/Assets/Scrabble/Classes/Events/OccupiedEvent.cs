using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Events
{
	using Board;
	using Ext;
	using Model;
	
	public class OccupiedEvent : IEventData 
	{
		public static readonly string TILES = "tiles";

		private Dictionary<string, object> m_eventData;
		
		public OccupiedEvent (List<Tile> p_tiles) 
		{
			m_eventData = new Dictionary<string, object>();
			m_eventData.Add(TILES, p_tiles);
		}

		public string ToEventString () { return string.Empty; }
		
		public Dictionary<string, object> EventData ()
		{
			return m_eventData.Clone<string, object>();
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
