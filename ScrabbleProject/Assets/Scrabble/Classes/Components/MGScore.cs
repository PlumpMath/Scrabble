using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;
	using Model;

	public class MGScore : MonoBehaviour 
	{
		private static readonly string SCORE_FORMAT = "score: {0}";

		[SerializeField] private EEvents m_eventToListen; 
		[SerializeField] private tk2dTextMesh m_txtLabel;
		[SerializeField] private int m_totalScore;

		private void Awake ()
		{
			this.Assert<tk2dTextMesh>(m_txtLabel, "m_txtLabel must not be null!");

			ScrabbleEvent.Instance.OnTriggerEvent += this.OnEventListened;
		}

		private void Start ()
		{
			this.UpdateScore();
		}

		private void OnDestroy ()
		{
			ScrabbleEvent.Instance.OnTriggerEvent -= this.OnEventListened;
		}

		private void UpdateScore ()
		{
			m_txtLabel.text = string.Format(SCORE_FORMAT, m_totalScore);
		}

		private void OnEventListened (EEvents p_type, IEventData p_data)
		{
			switch (p_type)
			{
				//case m_eventToListen:
				case EEvents.OnScoreComputed:
				{
					ScoreEvent data = (ScoreEvent)p_data;
					int score = data.Data<int>(ScoreEvent.SCORE);
					Dictionary<ETileType, bool> scoreModifiers = data.Data<Dictionary<ETileType, bool>>(ScoreEvent.MULTIPLIER);
					bool scrabble = data.Data<bool>(ScoreEvent.SCRABBLE);

					// Add the score
					m_totalScore += score;

					this.UpdateScore();
				}
				break;
			}
		}
	}
}