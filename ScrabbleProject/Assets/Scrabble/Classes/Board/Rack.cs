using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Board
{
	using Events;
	using Ext;
	using Model;

	public class Rack : MonoBehaviour 
	{
		public static readonly int RACK_LIMIT = 7;

		[SerializeField] private Letter m_letter;
		[SerializeField] private List<ELetter> m_letters; 
		private List<Letter> m_letterViews;
		private Model m_model;

		private void Awake ()
		{
			m_model = Model.Instance;
			m_model.Rack = this;

			m_letterViews = new List<Letter>();

			// test add dummy letter
			/*
			this.CreateLetter(Letters.Instance.Letter());
			this.CreateLetter(Letters.Instance.Letter());
			this.CreateLetter(Letters.Instance.Letter());
			this.CreateLetter(Letters.Instance.Letter());
			this.CreateLetter(Letters.Instance.Letter());
			this.CreateLetter(Letters.Instance.Letter());
			this.CreateLetter(Letters.Instance.Letter());
			*/

			ScrabbleEvent.Instance.OnTriggerEvent += this.OnEventListened;
		}

		private void OnDestroy ()
		{
			ScrabbleEvent.Instance.OnTriggerEvent -= this.OnEventListened;
		}

		public void CreateLetter (ELetter p_letter)
		{
			m_letters.Add(p_letter);

			// generate letter view
			Letter letter = this.CreateBoardLetter(m_letter, p_letter);
			letter.transform.parent = this.transform;
			letter.transform.localPosition = m_letter.transform.position;
			letter.name = "Letter_" + Model.Instance.Board.LetterText(p_letter);

			// update letter view
			letter.PreloadSkin(p_letter);
			
			m_letterViews.Add(letter);
			this.AdjustPosition();
		}

		/// <summary>
		/// Adds an existing letter
		/// </summary>
		public void AddLetter (Letter p_letter)
		{
			p_letter.Tile.Activate();
			p_letter.Tile = null;
			p_letter.transform.localScale = new Vector3(BOARD.LETTER_OFFSET, BOARD.LETTER_OFFSET, 0f);
			p_letter.transform.localPosition = m_letter.transform.position;
			m_letters.Add(p_letter.Type);
			m_letterViews.Add(p_letter);
			this.AdjustPosition();
		}

		public void RemoveLetter (ELetter p_letter)
		{
			int index = m_letters.IndexOf(p_letter);
			Letter letter = m_letterViews[index];

			// remove letters
			m_letters.RemoveAt(index);
			m_letterViews.RemoveAt(index);
			
			this.AdjustPosition();
		}

		public Letter Letter (ELetter p_letter)
		{
			int index = m_letters.IndexOf(p_letter);
			Letter letter = m_letterViews[index];
			return letter;
		}

		private void AdjustPosition ()
		{
			// fixe init pos
			for (int i = 0; i < m_letterViews.Count; i++)
			{
				float newX = m_letter.transform.position.x + (BOARD.LETTER_OFFSET * (float)i);
				m_letterViews[i].transform.SetX(newX);
			}

			if (m_letterViews.Count > 1)
			{
				Vector3 head = m_letterViews[m_letterViews.Count - 1].transform.position;
				head.y = 0;
				head.z = 0;
				Vector3 tail = m_letterViews[0].transform.position;
				tail.y = 0;
				tail.z = 0;

				float distance = Vector3.Distance(head, tail);
				float computedDist = distance * 0.5f;
				
				foreach (Letter letter in m_letterViews)
				{
					float newX = letter.transform.position.x - computedDist;
					letter.transform.SetX(newX);
					letter.Ready(); 
				}
			}
			else if (m_letterViews.Count == 1)
			{
				m_letterViews[0].transform.localPosition = m_letter.transform.position;
				m_letterViews[0].Ready();
			}
		}

		private void OnEventListened (EEvents p_type, IEventData p_data)
		{
			switch (p_type)
			{
				case EEvents.OnCleanUpRack:
				{
					SnapEvent snap = (SnapEvent)p_data;
					Tile tile = snap.Data<Tile>(SnapEvent.TILE);
					ELetter eletter = snap.Data<ELetter>(SnapEvent.LETTER);
					
					// update the tile of the letter
					Letter letter = this.Letter(eletter);	
					letter.Tile = tile;	

					// remove the letter from rack
					this.RemoveLetter(eletter);
				}
				break;

				case EEvents.OnPressedPass:
				{
					int limit = (RACK_LIMIT - m_letters.Count);
					this.Log(Tags.Log, "Rack::OnEventListened OnPressedPass limit:{0}", limit);
					
					if (limit > 0)
					{
						for (int i = 0; i < limit; i++)
						{
							this.CreateLetter(Letters.Instance.Letter());
						}
					}
				}
				break;
			}
		}
	}
}
