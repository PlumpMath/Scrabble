using UnityEngine;
using System.Collections;

namespace MGTools
{
	using Board;
	using Events;
	using Ext;

	public class MGSingleInstance : MonoBehaviour 
	{
		private void Awake ()
		{
			GameObject.DontDestroyOnLoad(this.gameObject);
		}
	}
}