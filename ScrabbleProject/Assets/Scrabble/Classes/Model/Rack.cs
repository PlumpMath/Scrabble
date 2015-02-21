using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Board
{
	using Ext;
	using Model;

	public class Rack : MonoBehaviour 
	{
		[SerializeField] private Letter m_letter;
		[SerializeField] private List<ELetter> m_letters; 
		private List<Letter> m_letterViews;

		private void Awake ()
		{
			m_letterViews = new List<Letter>();

			// test add dummy letter
			this.AddLetter(ELetter.K);
			this.AddLetter(ELetter.A);
			this.AddLetter(ELetter.B);
			this.AddLetter(ELetter.D);
		}

		public void AddLetter (ELetter p_letter)
		{
			m_letters.Add(p_letter);

			// generate letter view
			Letter letter = this.CreateBoardLetter(m_letter, p_letter);
			letter.transform.parent = this.transform;
			letter.transform.localPosition = m_letter.transform.position;
			letter.name = "Letter_" + Model.Instance.Board.LetterText(p_letter);

			// update letter view
			letter.PreloadSkin(p_letter);

			if (m_letterViews.Count > 0)
			{
				float newX = m_letterViews[m_letterViews.Count - 1].transform.position.x + 1f;
				letter.transform.SetX(newX);
			}

			m_letterViews.Add(letter);
			this.AdjustPosition();
		}

		public void RemoveLetter (ELetter p_letter)
		{
			int index = m_letters.IndexOf(p_letter);
			Letter letter = m_letterViews[index];

			// remove letters
			m_letters.RemoveAt(index);
			m_letterViews.RemoveAt(index);

			GameObject.Destroy(letter);
			this.AdjustPosition();
		}

		private void AdjustPosition ()
		{
			if (m_letterViews.Count > 1)
			{
				float distance = m_letterViews[m_letterViews.Count - 1].transform.position.x - m_letterViews[0].transform.position.x;

				foreach (Letter letter in m_letterViews)
				{
					float newX = letter.transform.position.x - (distance * 0.25f);
					letter.transform.SetX(newX);
				}
			}
			else if (m_letterViews.Count == 1)
			{
				m_letterViews[0].transform.localPosition = m_letter.transform.position;
			}
		}
	}
}
