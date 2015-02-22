using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Events
{
	public interface IEventData 
	{
		string ToEventString ();
		Dictionary<string, object> EventData ();
		bool Has (string p_key);
		T Data<T> (string p_key);
	}
}
