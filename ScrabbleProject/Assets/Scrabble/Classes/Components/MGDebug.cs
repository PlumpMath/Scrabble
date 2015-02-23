using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	public class MGDebug : MonoBehaviour 
	{
		public static readonly int MAX_LETTERS = 7;

		public static MGDebug Instance { get; private set; }

		[SerializeField] private string m_debugLetters;
		[SerializeField] private bool m_debugStartingWord;
		[SerializeField] private bool m_debugGeneratedWord;

		private void Awake ()
		{
			MGDebug.Instance = this;
		}

		public void ValidateLetters ()
		{
			m_debugLetters = Regex.Replace(m_debugLetters, @"[^a-zA-Z0-9 ]", "");

			// trim the characters
			if (m_debugLetters.Length > 7)
			{
				m_debugLetters = this.NormalizeLength(m_debugLetters, MAX_LETTERS);
			}
		}

		private string NormalizeLength (string p_string, int p_len)
		{
			return p_string.Substring(0, p_len);
		}

		public bool IsStartingWord
		{
			get { return m_debugStartingWord; }
			private set { m_debugStartingWord = value; }
		}

		public bool IsGeneratedWord
		{
			get { return m_debugGeneratedWord; }
			private set { m_debugGeneratedWord = value; }
		}
	}
}