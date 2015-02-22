using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	public class WordManager : MonoBehaviour 
	{
		public static readonly string ENGLISH = "en";
		public static readonly Char NEXT_LINE = '\n';

		public static WordManager Instance { get; private set; }

		private Dictionary<string, bool> m_cachedWords;

		private void Awake ()
		{
			WordManager.Instance = this;
			m_cachedWords = new Dictionary<string, bool>();
			this.InitializeWords();
		}

		private void InitializeWords ()
		{
			string[] words = (Resources.Load(ENGLISH) as TextAsset).text.Split(NEXT_LINE);

			foreach (string word in words)
			{
				m_cachedWords.Add(word.Trim() ,true);
			}
		}

		public bool IsValid (string p_word)
		{
			string clean = p_word.ToLower().Trim();
			return m_cachedWords.ContainsKey(clean);
		}
	}
}