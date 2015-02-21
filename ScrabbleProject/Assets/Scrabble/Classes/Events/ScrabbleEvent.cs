using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Events
{
	using Ext;

	public enum EEvents
	{
		Invalid 	= 0x0,

		/// <summary>
		/// Letter is dropped to board.
		/// It must contain the following data
		/// Drop Data:
		/// 	Vect3	- World Pos
		/// 	Tile	- Tile
		/// </summary>
		OnDrop		= 0x1 << 0,
	}; 

	public class ScrabbleEvent
	{
		private static ScrabbleEvent m_instance = null;

		public static ScrabbleEvent Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new ScrabbleEvent();
				}

				return m_instance;
			}
		}

		public Action<EEvents, IEventData> OnTriggerEvent;

		public void Trigger (EEvents p_event, IEventData p_eventData)
		{
			if (this.OnTriggerEvent != null)
			{
				this.OnTriggerEvent(p_event, p_eventData);
			}
		}
	}
}