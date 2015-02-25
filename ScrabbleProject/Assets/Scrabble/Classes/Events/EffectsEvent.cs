using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Events
{
	using Board;
	using Ext;
	using Model;
	using MGTools;

	public class EffectsEvent : IEventData 
	{
		public static readonly string EFFECTS = "effects";

		private Dictionary<string, object> m_eventData;
		
		public EffectsEvent (List<EScrabbleEffects> p_effects)
		{
			m_eventData = new Dictionary<string, object>();
			m_eventData.Add(EFFECTS, p_effects);
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