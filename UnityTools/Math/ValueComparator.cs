using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.UnityTools.Math {

	[Flags]
	public enum CompareTypes
	{
		More = 1<<0,
		Less = 1<<1,
		Equal = 1<<2,
		Any = 1<<3,		
	}

	public static class ValueComparator {


		public static bool Compare<T>(T x, T y,Comparer<T> comparer, 
			CompareTypes options) 
		{

			if ((options & CompareTypes.Any) > 0)
				return true;
			
			var compareResult = comparer.Compare(x, y);

			if ((options & CompareTypes.Equal) > 0 && compareResult==0)
				return true;
			if ((options & CompareTypes.More) > 0 && compareResult>0)
				return true;
			if ((options & CompareTypes.Less) > 0 && compareResult<0)
				return true;
			
			return false;

		}


		public static bool Compare<T>(T x, T y, CompareTypes options)
			where  T : struct
		{
			var comparer = Comparer<T>.Default;

			if ((options & CompareTypes.Any) > 0)
				return true;
			
			var compareResult = comparer.Compare(x, y);

			if ((options & CompareTypes.Equal) > 0 && compareResult==0)
				return true;
			if ((options & CompareTypes.More) > 0 && compareResult>0)
				return true;
			if ((options & CompareTypes.Less) > 0 && compareResult<0)
				return true;
			
			return false;

		}
		
		public static bool IsInRange(float value, float from, float to) {

			if (value >= from && value <= to)
				return true;
			if (value >= to && value <= from)
				return true;

			return false;
		}
		
		public static bool IsInRange(int value, int from, int to) {

			if (value >= from && value <= to)
				return true;
			if (value >= to && value <= from)
				return true;

			return false;
		}

	
		public static bool IsInRange(this float value, Vector2 range)
		{
			return IsInRange(value, range.x, range.y);
		}
		
		public static bool IsInRange(this int value, Vector2Int range)
		{
			return IsInRange(value, range.x, range.y);
		}
		
		public static bool IsAbsInRange(this float value, Vector2 range)
		{
			return IsInRange(Mathf.Abs(value), range.x, range.y);
		}

		public static bool ValidateMask(this int mask, int value) 
		{
			var result = (mask & value) > 0;
			return result;
		}
		
		public static bool ValidateLayerMask(this int mask, int layer) 
		{
			var result = (mask & (1 << layer)) > 0;
			return result;
		}
		
	}
	

}