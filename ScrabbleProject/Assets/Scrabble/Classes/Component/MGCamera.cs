using UnityEngine;
using System.Collections;

namespace MGTools
{
	using Ext;

	public class MGCamera : MonoBehaviour 
	{
		public static MGCamera Instance { get; private set; }

		[SerializeField] private Camera m_scrabbleCamera;

		private void Awake ()
		{
			this.Assert<Camera>(m_scrabbleCamera, "m_scrabbleCamera must not be null!");
			MGCamera.Instance = this;
		}

		public Camera ScrabbleCamera
		{
			get { return m_scrabbleCamera; }
		}
	}
}
