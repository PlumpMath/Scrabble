using UnityEngine;
using System;
using System.Collections;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	public enum EButton
	{
		Pass,
		Submit,
	};

	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(tk2dUIItem))]
	public class MGButton : MonoBehaviour 
	{
		[SerializeField] private EButton m_button;
		[SerializeField] private bool m_isEnabled;
		[SerializeField] private tk2dTextMesh m_txtLabel;
		[SerializeField] private string m_label;
		private BoxCollider2D m_cachedCollider;

		public Signal OnTriggerAction = new Signal(typeof(MGButton));

		private void Awake ()
		{
			this.Assert<tk2dTextMesh>(m_txtLabel, "m_txtLabel is null!");
			m_txtLabel.text = m_label;
			m_cachedCollider = this.GetComponent<BoxCollider2D>();

			if (m_button == EButton.Pass)
			{
				ScrabbleEvent.Instance.OnTriggerEvent += this.OnEventListened;
			}
		}

		private void OnDestroy ()
		{
			if (m_button == EButton.Pass)
			{
				ScrabbleEvent.Instance.OnTriggerEvent -= this.OnEventListened;
			}
		}

		private void OnClicked ()
		{
			this.OnTriggerAction.Invoke(this);
		}

		public EButton Button { get { return m_button; } }

		private void OnEventListened (EEvents p_type, IEventData p_data)
		{
			switch (p_type)
			{
				case EEvents.OnPassDisabled:
				{
					m_cachedCollider.enabled = false;
					m_txtLabel.color = Color.gray;
				}
				break;

				case EEvents.OnPassEnabled:
				{
					m_cachedCollider.enabled = true;
					m_txtLabel.color = Color.white;
				}
				break;
			}
		}
	}
}