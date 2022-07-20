using System;

namespace ILCDirectory.Enums
{
    public static class MaritalStatus
    {
        public const string Unspecified = "0";
        public const string Divorced	= "D";
        public const string Married	    = "M";
        public const string Single	    = "S";
        public const string Unmarried	= "U";
        public const string Widowed     = "W";

        public static Dictionary<string, string> GetMaritalStatusDictionary () { 
            return new Dictionary<string, string> { 
                {"Unspecified","0"},
                {"Divorced","D"},
                {"Married","M"},
                {"Single","S"},
                {"Unmarried","U"},
                {"Widowed","W"}
            }; 
        }
    }
}
