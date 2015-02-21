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
			this.AddLetter(ELetter.A);
		}

		public void AddLetter (ELetter p_letter)
		{
			m_letters.Add(p_letter);

			// generate letter view
			Letter letter = this.CreateBoardLetter(m_letter, p_letter);
			letter.transform.parent = this.transform;
			letter.transform.localPosition = m_letter.transform.position;

			// update letter view
			letter.PreloadSkin(p_letter);

			m_letterViews.Add(letter);
		}
		
	}
}
