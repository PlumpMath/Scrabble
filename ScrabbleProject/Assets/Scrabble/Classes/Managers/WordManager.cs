using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	public enum WordStatus
	{
		Invalid = 0x0,
		NotAWord = 0x1 << 0,
		Used = 0x1 << 1,
		UnUsed = 0x1 << 2,
		Max = 0x1 << 3,
	};

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
				m_cachedWords.Add(word.Trim(), false);
			}
		}

		public WordStatus IsValid (string p_word)
		{
			string clean = p_word.ToLower().Trim();
			bool contains = m_cachedWords.ContainsKey(clean);

			if (contains)
			{
				if (m_cachedWords[clean]) 
				{ 
					return WordStatus.Used; 
				}
				else
				{
					m_cachedWords[clean] = true;
					return WordStatus.UnUsed;
				}
			}

			return WordStatus.NotAWord;
		}
	}
}