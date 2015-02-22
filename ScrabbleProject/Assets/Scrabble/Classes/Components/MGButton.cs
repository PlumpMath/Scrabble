using UnityEngine;
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
	
	public class MGButton : MonoBehaviour 
	{
		[SerializeField] private EButton m_button;
		[SerializeField] private bool m_isEnabled;
		[SerializeField] private tk2dTextMesh m_txtLabel;
		[SerializeField] private string m_label;

		public Signal OnTriggerAction = new Signal(typeof(MGButton));

		private void Awake ()
		{
			this.Assert<tk2dTextMesh>(m_txtLabel, "m_txtLabel is null!");
			m_txtLabel.text = m_label;
		}

		private void OnClicked ()
		{
			this.OnTriggerAction.Invoke(this);
		}

		public EButton Button { get { return m_button; } }
	}
}
