using UnityEngine;
using System.Collections;

public enum EButton
{
	Pass,
	Submit,
};

public class MGButton : MonoBehaviour 
{
	[SerializeField] private EButton m_button;
	[SerializeField] private bool m_isEnabled;
	private tk2dSprite m_sprite;

	public Signal OnTriggerAction = new Signal(typeof(MGButton));

	private void Awake ()
	{
		m_sprite = this.GetComponent<tk2dSprite>();
	}

	private void OnClicked ()
	{
		this.OnTriggerAction.Invoke(this);
	}

	public EButton Button { get { return m_button; } }

	public bool IsEnabled 
	{
		get { return m_isEnabled; }
		set 
		{ 
			m_isEnabled = value;

			if (!m_isEnabled)
			{
				m_sprite.color = Color.gray;
			}
			else
			{
				m_sprite.color = Color.white;
			}
		}
	}
}
