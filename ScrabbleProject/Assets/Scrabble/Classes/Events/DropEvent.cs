using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Events
{
	using Board;
	using Ext;
	using Model;

	public class DropEvent : IEventData 
	{
		public static readonly string POSITION = "pos";
		public static readonly string LETTER = "letter";

		private Dictionary<string, object> m_eventData;

		public DropEvent (Vector3 p_pos, Letter p_tile)
		{
			m_eventData = new Dictionary<string, object>();
			m_eventData.Add(POSITION, p_pos);
			m_eventData.Add(LETTER, p_tile);
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
