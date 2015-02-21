using UnityEngine;
using System;
using System.Collections;

namespace MGTools
{
	using Ext;

	[RequireComponent(typeof(BoxCollider2D))]
	public class MGDraggable : MonoBehaviour 
	{
		private Vector3 m_screenPoint;
		private Vector3 m_offset;
		private Vector3 m_rackos;

		private void OnMouseDown ()
		{
			this.Log(Tags.Log, "Draggable::OnMouseDown pos:{0}", this.transform.position);
			m_rackos = this.transform.position;
			m_screenPoint =  MGCamera.Instance.ScrabbleCamera.WorldToScreenPoint(m_rackos);
			m_offset = m_rackos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z));
		}

		private void OnMouseDrag ()
		{
			Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z);
			Vector3 pos = Camera.main.ScreenToWorldPoint(screenPos);
			this.transform.position = pos;
		}

		private void OnMouseUp ()
		{
			this.Log(Tags.Log, "Draggable::OnMouseUp pos:{0}", this.transform.position);
			// TODO: Check for valid position
			this.transform.position = m_rackos;
		}
	}
}
