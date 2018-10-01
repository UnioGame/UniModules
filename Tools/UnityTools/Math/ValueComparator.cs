using System;
using System.Collections.Generic;

namespace Tools.MathTools {

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


		public static bool IsBetween(float value, float from, float to) {

			if (value >= from && value <= to)
				return true;
			if (value >= to && value <= from)
				return true;

			return false;
		}
		
	}
	

}