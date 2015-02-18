using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ext
{
   
	public enum Tags
	{
		Invalid				= 0x0,
		Log					= 0x1 << 0,
		Warn				= 0x1 << 1,
		Error				= 0x1 << 2,
		Max					= 0x1 << 3,
		ALL					= ~Invalid >> 1,
	};

	public static class Extensions
    {
		private static Tags tags = Tags.Log | Tags.Warn | Tags.Error ;

		public static void Log<T> (this T p_self, Tags p_tag, string p_format, params object[] p_args)
		{
			if (p_tag == Tags.Invalid) { return; }
			if (!tags.Has(p_tag)) { return; }
			string logs = string.Format(p_format, p_args);
			Debug.Log(string.Format("<color=yellow>[{0}]</color> {1}\n", logs));
		}

		public static void LogWarn<T> (this T p_self, Tags p_tag, string p_format, params object[] p_args)
		{
			if (p_tag == Tags.Invalid) { return; }
			if (!tags.Has(p_tag)) { return; }
			string logs = string.Format(p_format, p_args);
			Debug.Log(string.Format("<color=green>[{0}]</green> {1}\n", logs));
		}

		public static void LogError<T> (this T p_self, Tags p_tag, string p_format, params object[] p_args)
		{
			if (p_tag == Tags.Invalid) { return; }
			if (!tags.Has(p_tag)) { return; }
			string logs = string.Format(p_format, p_args);
			Debug.Log(string.Format("<color=red>[{0}]</color> {1}\n", logs));
		}

        #region INTEGER EXTENSION
        public static T Clamp<T> (this T p_val, T p_min, T p_max) where T : IComparable<T>
		{
			if (p_val.CompareTo(p_min) < 0) return p_min;
			else if(p_val.CompareTo(p_max) > 0) return p_max;
			else return p_val;
		}

		public static int ClampInt (this int p_val, int p_min, int p_max) 
		{
			if (p_val < p_min) return p_min;
			else if(p_val > p_max) return p_max;
			else return p_val;
		}

        public static uint ToUINT (this int p_val)
        {
            return (uint)p_val;
        }
        #endregion

        #region ENUM EXTENSION
        public static bool Has<T> (this Enum type, T value) 
        {
			try {
				return (((int)(object)type & (int)(object)value) == (int)(object)value);
			} 
			catch {
				return false;
			}
		}
		
		public static bool Is<T> (this Enum type, T value) 
        {
			try {
				return (int)(object)type == (int)(object)value;
			}
			catch {
				return false;
			}    
		}
		
		public static T Add<T> (this Enum type, T value) 
        {
			try {
				return (T)(object)(((int)(object)type | (int)(object)value));
			}
			catch(Exception ex) {
				throw new ArgumentException(
					string.Format(
					"Could not append value from enumerated type '{0}'.",
					typeof(T).Name
					), ex);
			}    
		}
		
		public static T Remove<T> (this Enum type, T value) 
        {
			try {
				return (T)(object)(((int)(object)type & ~(int)(object)value));
			}
			catch (Exception ex) {
				throw new ArgumentException(
					string.Format(
					"Could not remove value from enumerated type '{0}'.",
					typeof(T).Name
					), ex);
			}
        }
		
		public static IEnumerable<Enum> GetFlags (this Enum p_value)
		{
			return GetFlags(p_value, Enum.GetValues(p_value.GetType()).Cast<Enum>().ToArray());
		}
		
		public static IEnumerable<Enum> GetIndividualFlags (this Enum p_value)
		{
			return GetFlags(p_value, GetFlagValues(p_value.GetType()).ToArray());
		}
		
		private static IEnumerable<Enum> GetFlags (Enum p_value, Enum[] p_values)
		{
			ulong bits = Convert.ToUInt64(p_value);
			List<Enum> results = new List<Enum>();
			for (int i = p_values.Length - 1; i >= 0; i--)
			{
				ulong mask = Convert.ToUInt64(p_values[i]);
				if (i == 0 && mask == 0L) { break; }
				if ((bits & mask) == mask)
				{
					results.Add(p_values[i]);
					bits -= mask;
				}
			}
			if (bits != 0L) { return Enumerable.Empty<Enum>(); }
			if (Convert.ToUInt64(p_value) != 0L) { return results.Reverse<Enum>(); }
			if (bits == Convert.ToUInt64(p_value) 
			&& 	p_values.Length > 0 
			&& 	Convert.ToUInt64(p_values[0]) == 0L
			) { 
				return p_values.Take(1); 
			}

			return Enumerable.Empty<Enum>();
		}
		
		private static IEnumerable<Enum> GetFlagValues (Type p_enumType)
		{
			ulong flag = 0x1;
			foreach (var value in Enum.GetValues(p_enumType).Cast<Enum>())
			{
				ulong bits = Convert.ToUInt64(value);

				if (bits == 0L) { continue; }// skip the zero value
				while (flag < bits) { flag <<= 1; }

				if (flag == bits)
				{
					yield return value;
				}
			}
		}
        #endregion

        #region VECTOR EXTENSION
        public static void SetX (this Transform p_trans, float p_x)
        {
            p_trans.position = new Vector3(p_x, p_trans.position.y, p_trans.position.z);
        }

        public static void SetY (this Transform p_trans, float p_y)
        {
            p_trans.position = new Vector3(p_trans.position.y, p_y, p_trans.position.z);
        }

        public static void SetZ (this Transform p_trans, float p_z)
        {
            p_trans.position = new Vector3(p_trans.position.x, p_trans.position.y, p_z);
        }
        #endregion

        #region MONO EXTENSION
        public static void Assert<T> (this MonoBehaviour p_self, T p_item, string p_message)
        {
            if (p_item == null)
            {
				p_self.LogError(Tags.Error, "Message: {0}", p_message);
            }
        }

        public static void Assert<T> (this T p_self, bool p_isSatisfied, string p_message)
        {
            if (!p_isSatisfied)
            {
				p_self.LogError(Tags.Error, "Message: {0}", p_message);
            }
        }
        #endregion
    }
}