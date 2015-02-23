using UnityEngine;
using System.Collections;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	[RequireComponent(typeof(MeshRenderer))]
	public class MGBlinker : MonoBehaviour 
	{
		private MeshRenderer m_cachedRenderer;

		private void Awake ()
		{
			m_cachedRenderer = this.GetComponent<MeshRenderer>();
		}

		private void LateUpdate ()
		{
			if (m_cachedRenderer.enabled != MGTicker.Instance.Tick)
			{
				m_cachedRenderer.enabled = MGTicker.Instance.Tick;
			}
		}
	}
}