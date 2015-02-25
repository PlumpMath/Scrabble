using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	public enum EScrabbleEffects
	{
		Scrabble,
		TripleWord,
		DoubleWord,
		TripleLetter,
		DoubleLetter,
		InvalidWord,
		SingleLetter,
		NoLetters,
	};

	public class MGEffects : MonoBehaviour 
	{
		public static readonly string IMG_SB = "txt_scrabble";
		public static readonly string IMG_TW = "txt_triple_word";
		public static readonly string IMG_DW = "txt_double_word";
		public static readonly string IMG_TL = "txt_triple_letter";
		public static readonly string IMG_DL = "txt_double_letter";
		public static readonly string IMG_NL = "txt_no_letters";
		public static readonly string IMG_SL = "txt_single_letters";
		public static readonly string IMG_IW = "txt_invalid_word";
		
		[SerializeField] private tk2dSprite m_scrabble;
		[SerializeField] private tk2dSprite m_tripleWord;
		[SerializeField] private tk2dSprite m_doubleWord;
		[SerializeField] private tk2dSprite m_tripleLetter;
		[SerializeField] private tk2dSprite m_doubleLetter;
		[SerializeField] private tk2dSprite m_invalidWord;
		[SerializeField] private tk2dSprite m_singleLetter;
		[SerializeField] private tk2dSprite m_noLetters;

		private Dictionary<EScrabbleEffects, tk2dSprite> m_effects = new Dictionary<EScrabbleEffects, tk2dSprite>();

		private void Awake ()
		{
			m_effects.Add(EScrabbleEffects.Scrabble, m_scrabble);
			m_effects.Add(EScrabbleEffects.TripleWord, m_tripleWord);
			m_effects.Add(EScrabbleEffects.DoubleWord, m_doubleWord);
			m_effects.Add(EScrabbleEffects.TripleLetter, m_tripleLetter);
			m_effects.Add(EScrabbleEffects.DoubleLetter, m_doubleLetter);
			m_effects.Add(EScrabbleEffects.InvalidWord, m_invalidWord);
			m_effects.Add(EScrabbleEffects.SingleLetter, m_singleLetter);
			m_effects.Add(EScrabbleEffects.NoLetters, m_noLetters);

			ScrabbleEvent.Instance.OnTriggerEvent += this.OnEventListened;
		}

		private void OnDestroy ()
		{
			ScrabbleEvent.Instance.OnTriggerEvent -= this.OnEventListened;
		}

		private void OnEventListened (EEvents p_type, IEventData p_data)
		{
			switch (p_type)
			{
				case EEvents.OnShowPositiveEffects:
				{
					EffectsEvent evtEffects = (EffectsEvent)p_data;
					List<EScrabbleEffects> effects = evtEffects.Data<List<EScrabbleEffects>>(EffectsEvent.EFFECTS);
					effects.Sort(new SortEffects(Sort.Ascending));
				}
				break;

				case EEvents.OnShowNegativeEffects:
				{
					EffectsEvent evtEffects = (EffectsEvent)p_data;
					List<EScrabbleEffects> effects = evtEffects.Data<List<EScrabbleEffects>>(EffectsEvent.EFFECTS);
					effects.Sort(new SortEffects(Sort.Ascending));
				}
				break;
			}
		}

		private void ShowPositive (List<EScrabbleEffects> p_effects)
		{
		}

		private IEnumerator ShowPositive (EScrabbleEffects p_effect)
		{
			yield break;
		}

		private IEnumerator ShowNegative (EScrabbleEffects p_effect)
		{
			yield break;
		}
	}
}