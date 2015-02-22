using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Events
{
	using Board;
	using Ext;
	using Model;

	public class ScoreEvent : IEventData 
	{
		public static readonly string SCORE = "score";
		public static readonly string MULTIPLIER = "multiplier";
		public static readonly string SCRABBLE = "scrabble";

		private Dictionary<string, object> m_eventData;
		
		public ScoreEvent (
			int p_score, 
			Dictionary<ETileType, bool> p_modifier,
			bool p_isScrabble
		) {
			m_eventData = new Dictionary<string, object>();
			m_eventData.Add(SCORE, p_score);
			m_eventData.Add(MULTIPLIER, p_modifier);
			m_eventData.Add(SCRABBLE, p_isScrabble);
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
