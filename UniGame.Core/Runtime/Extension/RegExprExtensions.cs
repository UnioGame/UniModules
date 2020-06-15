using System.Text.RegularExpressions;

namespace UniModules.UniGame.Core.Runtime.Extension
{
    public static class RegExprExtensions
    {
        private const string _validPathCharacters   = "[^a-zA-Z0-9_.]+";
        private const string _validMethodCharacters = "[^a-zA-Z0-9_]+";
    

        public static string RemoveSpecialCharacters(this string str)
        {   
            return Regex.Replace(str, _validPathCharacters, "", RegexOptions.Compiled);
        }
    
        public static string RemoveSpecialAndDotsCharacters(this string str)
        {   
            return Regex.Replace(str, _validMethodCharacters, "", RegexOptions.Compiled);
        }
    
    }
}
