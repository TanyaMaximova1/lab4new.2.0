using System;
using System.Linq;

namespace lab3
{
	public class SemVersion
	{
		private int v1;
		private int v2;
		private int v3;

		public int Minor { get; set; }
		public int Major { get; set; }
		public int Patch { get; set; }
		public int? PreRelease_Part_1 { get; set; }
		public int? PreRelease_Part_2 { get; set; }
		public int? PreRelease_Part_3 { get; set; }

		public SemVersion(string Versions)
		{
			if (IsCorrect(Versions))
			{
				string[] splitedUnits = Versions.Split('-');
				string[] splitedPartsOfUnit1 = splitedUnits[0].Split('.');
				v1 = Convert.ToInt32(splitedPartsOfUnit1[0]);
				v2 = Convert.ToInt32(splitedPartsOfUnit1[1]);
				v3 = Convert.ToInt32(splitedPartsOfUnit1[2]);
			}
			else
			{
				throw new ArgumentException("Недопустимый формат версии");
			}         
		}

		private static bool IsCorrect(string Versions)
        {
			string[] splitedByDashVersion = Versions.Split('-');
			if (splitedByDashVersion.Length == 1)
			{
				int n = 0;
				string[] splitedByDotVersion = Versions.Split('.');
				if (splitedByDotVersion.Length == 3)
				{
					if (Int32.TryParse(splitedByDotVersion[0], out n) == true)
					{
						if (Int32.TryParse(splitedByDotVersion[1], out n) == true)
						{
							if (Int32.TryParse(splitedByDotVersion[2], out n) == true)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			if (splitedByDashVersion.Length == 2)
			{
				bool flagOfPt1 = false;
				bool flagOfPt2 = false;
				int n = 0;
				string[] splitedByDotVersionPt1 = splitedByDashVersion[0].Split('.');
				if (splitedByDotVersionPt1.Length == 3)
				{
					if (Int32.TryParse(splitedByDotVersionPt1[0], out n) == true)
					{
						if (Int32.TryParse(splitedByDotVersionPt1[1], out n) == true)
						{
							if (Int32.TryParse(splitedByDotVersionPt1[2], out n) == true)
							{
								flagOfPt1 = true;
							}
						}
					}
				}
				
				if (flagOfPt1 == true && flagOfPt2 == true)
				{
					return true;
				}
			}
			return false;
		}

		public static bool operator >(SemVersion version1, SemVersion version2)
		{
			return IsMore(version1, version2);
		}
		public static bool operator <(SemVersion version1, SemVersion version2)
		{
			return !IsMoreOrEqual(version1, version2);
		}

		private static bool IsMore(SemVersion v1, SemVersion v2) //>
		{
			if (v1.v1 > v2.v1)
			{
				return true;
			}
			else if (v1.v1 == v2.v1)
			{
				if (v1.v2 > v2.v2)
				{
					return true;
				}
				else if (v1.v2 == v2.v2)
				{
					if (v1.v3 > v2.v3)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool operator >=(SemVersion version1, SemVersion version2)
		{
			return IsMoreOrEqual(version1, version2);
		}


		private static bool IsMoreOrEqual(SemVersion v1, SemVersion v2) //>=
		{
			if (v1.v1 > v2.v1)
			{
				return true;
			}
			else if (v1.v1 == v2.v1)
			{
				if (v1.v2 > v2.v2)
				{
					return true;
				}
				else if (v1.v2 == v2.v2)
				{
					if (v1.v3 >= v2.v3)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool operator <=(SemVersion version1, SemVersion version2)
		{
			return IsLessOrEqual(version1, version2);
		}

		private static bool IsLessOrEqual(SemVersion v1, SemVersion v2) //<=
		{
			if (v1.v1 > v2.v1)
			{
				return false;
			}
			else if (v1.v1 == v2.v1)
			{
				if (v1.v2 > v2.v2)
				{
					return false;
				}
				else if (v1.v2 == v2.v2)
				{
					if (v1.v3 > v2.v3)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static bool operator ==(SemVersion version1, SemVersion version2)
		{
			return IsEqual(version1, version2);
		}

		public static bool operator !=(SemVersion version1, SemVersion version2)
		{
			return !IsEqual(version1, version2);
		}

		private static bool IsEqual(SemVersion v1, SemVersion v2)
		{
            if (v1.ToString() == v2.ToString())
            {
                return true;
            }
            return false;
        }

		public override string ToString()
		{
			return $"{v1}.{v2}.{v3}";			
		}

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}


        
    
