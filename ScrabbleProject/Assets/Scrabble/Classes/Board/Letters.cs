using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Board
{
	using Ext;
	using Model;

	public class Letters 
	{
		private static Letters m_instance;
		public static Letters Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new Letters();
				}

				return m_instance;
			}
		}

		private List<ELetter> m_letters;

		public Letters ()
		{
			m_letters = new List<ELetter>();
			this.InitializeLetters();
		}

		private void InitializeLetters ()
		{
			int counter = 0;
			ELetter letter = (ELetter)(0x1 << counter);
			while (letter < ELetter.Max)
			{
				int len = Model.Instance.Board.LetterCount(letter);

				for (int i = 0; i < len; i++)
				{
					m_letters.Add(letter);
				}

				counter++;
				letter = (ELetter)(0x1 << counter);
			}

			// Shuffle letters
			m_letters.Shuffle<ELetter>();
		}

		public ELetter Letter ()
		{
			if (m_letters.Count > 0)
			{
				ELetter letter = m_letters[0];
				m_letters.RemoveAt(0);
				return letter;
			}

			return ELetter.Invalid;
		}
	}
}