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
		ExistingWord,
	};

	public class MGEffects : MonoBehaviour 
	{
		public static readonly Dictionary<ETileType, EScrabbleEffects> EFFECTS = new Dictionary<ETileType, EScrabbleEffects>()
		{
			{ ETileType.TW, EScrabbleEffects.TripleWord },
			{ ETileType.DW, EScrabbleEffects.DoubleWord },
			{ ETileType.TL, EScrabbleEffects.TripleLetter },
			{ ETileType.DL, EScrabbleEffects.DoubleWord },
			{ ETileType.ST, EScrabbleEffects.DoubleWord },
		};
		
		[SerializeField] private tk2dSprite m_scrabble;
		[SerializeField] private tk2dSprite m_tripleWord;
		[SerializeField] private tk2dSprite m_doubleWord;
		[SerializeField] private tk2dSprite m_tripleLetter;
		[SerializeField] private tk2dSprite m_doubleLetter;
		[SerializeField] private tk2dSprite m_invalidWord;
		[SerializeField] private tk2dSprite m_singleLetter;
		[SerializeField] private tk2dSprite m_noLetters;
		[SerializeField] private tk2dSprite m_existingWord;

		private Dictionary<EScrabbleEffects, tk2dSprite> m_effects = new Dictionary<EScrabbleEffects, tk2dSprite>();
		private Vector3 m_effectDefaultPos = new Vector3(-0.071638f, 0f, 0f);
		private Vector3 m_moveFrom = new Vector3(7.665298f, 0f, 0f);
		private Vector3 m_moveTo = new Vector3(-7.665298f, 0f, 0f);

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
			m_effects.Add(EScrabbleEffects.ExistingWord, m_existingWord);

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

					this.StartCoroutine(this.ShowPositive(effects));
				}
				break;

				case EEvents.OnShowNegativeEffects:
				{
					EffectsEvent evtEffects = (EffectsEvent)p_data;
					List<EScrabbleEffects> effects = evtEffects.Data<List<EScrabbleEffects>>(EffectsEvent.EFFECTS);
					effects.Sort(new SortEffects(Sort.Ascending));
					
					this.StartCoroutine(this.ShowNegative(effects[0]));
				}
				break;
			}
		}

		private IEnumerator ShowPositive (List<EScrabbleEffects> p_effects)
		{
			yield return new WaitForSeconds(0.25f);

			foreach (EScrabbleEffects effect in p_effects)
			{
				this.StartCoroutine(this.ShowPositive(effect));
				yield return new WaitForSeconds(1.25f);
			}

			yield break;
		}

		private IEnumerator ShowPositive (EScrabbleEffects p_effect)
		{
			GameObject effects = m_effects[p_effect].gameObject;
			
			effects.gameObject.SetActive(true);
			//iTween.FadeTo(effects.gameObject, 1f, 0.25f);
			iTween.MoveFrom(effects, m_moveFrom, 0.25f);
			
			yield return new WaitForSeconds(0.75f);
			//iTween.FadeTo(effects.gameObject, 0f, 0.25f);
			iTween.MoveTo(effects, m_moveTo, 0.25f);
			
			yield return new WaitForSeconds(0.25f);

			effects.transform.position = m_effectDefaultPos;
			effects.SetActive(false);

			yield break;
		}

		private IEnumerator ShowNegative (EScrabbleEffects p_effect)
		{
			GameObject effects = m_effects[p_effect].gameObject;

			effects.gameObject.SetActive(true);
			//iTween.FadeTo(effects.gameObject, 1f, 0.5f);

			yield return new WaitForSeconds(1f);
			//iTween.FadeTo(effects.gameObject, 0f, 0.5f);

			yield return new WaitForSeconds(0.5f);
			effects.gameObject.SetActive(false);
			yield break;
		}
	}
}