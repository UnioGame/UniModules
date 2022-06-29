namespace UniGame.Utils.Runtime
{
    using System.Text.RegularExpressions;

    public static class StringUtility
    {
        public static string RemoveTags(string dialoguePhrase)
        {
            return Regex.Replace(dialoguePhrase, @"<.*?>", string.Empty);
        }
    }
}