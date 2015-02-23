using UnityEngine;
using System.Collections;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	public class MGTicker : MonoBehaviour 
	{
		public static readonly float FAST_DURATION = 0.50f;
		public static readonly float NORMAL_DURATION = 1.0f;
		public static readonly float SLOW_DURATION = 1.50f;

		public static MGTicker Instance { get; private set; }

		[SerializeField] private static float m_currentDuration = NORMAL_DURATION;
		[SerializeField] private float m_targetDuration = NORMAL_DURATION;

		private void Awake ()
		{
			MGTicker.Instance = this;
			this.Tick = true;
		}

		public bool Tick { get; private set; }

		private void FixedUpdate ()
		{
			if (m_currentDuration <= 0)
			{
				m_currentDuration = m_targetDuration;
				this.Tick = !this.Tick;
			}
		}

		private void LateUpdate ()
		{
			m_currentDuration -= Time.fixedDeltaTime;
		}
	}
}
