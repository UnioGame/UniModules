using UnityEngine;
using UnityAtlasGenerator.Helper;
using System.Text.RegularExpressions;

public enum AtlasGeneratorRuleMatchType
{
    /// <summary>
    /// Simple wildcard
    /// *, matches any number of characters
    /// ?, matches a single character
    /// </summary>
    [Tooltip("Simple wildcard.\n\"*\" matches any number of characters.\n\"?\" matches a single character.")]
    Wildcard = 0,

    /// <summary>
    /// Regex pattern
    /// </summary>
    [Tooltip("A regular expression pattern.")]
    Regex
}

public enum AtlasTemplateApplicationMode
{
    ApplyOnAtlasCreationOnly,
    AlwaysOverwriteAtlasSettings
}

[System.Serializable]
public class AtlasGeneratorRule
{
    /// <summary>
    /// Path pattern.
    /// </summary>
    [Tooltip("The assets in this path will be processed.")]
    public string path = string.Empty;

    /// <summary>
    /// Method used to parse the Path.
    /// </summary>
    [Tooltip("The path parsing method.")]
    public AtlasGeneratorRuleMatchType matchType;

    /// <summary>
    /// The atlas the asset will be added.
    /// </summary>
    [Tooltip("The path to atlas in which the sprites will be added. Leave blank for the default atlas.")]
    public string pathToAtlas = string.Empty;

    /// <summary>
    /// Cleaned atlas name.
    /// </summary>
    string CleanedPathToAtlas
    {
        get
        {
            return pathToAtlas.Trim();
        }
    }

    /// <summary>
    /// Atlas template to use. Default atlas settings will be used if empty.
    /// </summary>
    //[Tooltip("Atlas template that will be applied to the atlas. Leave none to use the Default Atlas's settings.")]
    //public AddressableAssetGroupTemplate groupTemplate = null;

    /// <summary>
    /// Controls wether group template will be applied only on group creation, or also to already created groups.
    /// </summary>
    [Tooltip("Defines if the group template will only be applied to new groups, or will also overwrite existing groups settings.")]
    public AtlasTemplateApplicationMode aatlasTemplateApplicationMode = AtlasTemplateApplicationMode.ApplyOnAtlasCreationOnly;

    /// <summary>
    /// Returns True if given assetPath matched with the rule.
    /// </summary>
    public bool Match(string assetPath)
    {
        path = path.Trim();
        if (string.IsNullOrEmpty(path))
            return false;
        if (matchType == AtlasGeneratorRuleMatchType.Wildcard)
        {
            if (path.Contains("*") || path.Contains("?"))
            {
                var regex = "^" + Regex.Escape(path).Replace(@"\*", ".*").Replace(@"\?", ".");
                return Regex.IsMatch(assetPath, regex);
            }
            else
                return assetPath.StartsWith(path);
        }
        else if (matchType == AtlasGeneratorRuleMatchType.Regex)
            return Regex.IsMatch(assetPath, path);
        return false;
    }

    /// <summary>
    /// Parse assetPath and replace all elements that match this.path regex
    /// with the atlasName string.
    /// Returns null if this.path or atlasName is empty.
    /// </summary>
    public string ParseAtlasReplacement(string assetPath)
    {
        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(pathToAtlas))
            return null;
        // Parse path elements.
        var replacement = AtlasGeneratorRegex.ParsePath(assetPath, CleanedPathToAtlas);
        // Parse this.path regex.
        if (matchType == AtlasGeneratorRuleMatchType.Regex)
        {
            string pathRegex = path;
            replacement = Regex.Replace(assetPath, pathRegex, replacement);
        }
        return replacement;
    }

    /// <summary>
    /// Full path to atlas.
    /// </summary>
    public string GetFullPathToAtlas(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return path;
        }
        return "Assets/GameContent/Atlases/" + path + ".spriteatlas";
    }

    /// <summary>
    /// Helper class for regex replacement.
    /// </summary>
    static class AtlasGeneratorRegex
    {
        const string pathregex = @"\$\{PATH\[\-{0,1}\d{1,3}\]\}"; // ie: ${PATH[0]} ${PATH[-1]}

        static public string[] GetPathArray(string path)
        {
            return path.Split('/');
        }

        static public string GetPathAtArray(string path, int idx)
        {
            return GetPathArray(path)[idx];
        }

        /// <summary>
        /// Parse assetPath and replace all matched path elements (i.e. `${PATH[0]}`)
        /// with a specified replacement string.
        /// </summary>
        static public string ParsePath(string assetPath, string replacement)
        {
            var _path = assetPath;
            int i = 0;
            var slashSplit = _path.Split('/');
            var len = slashSplit.Length - 1;
            var matches = Regex.Matches(replacement, pathregex);
            string[] parsedMatches = new string[matches.Count];
            foreach (var match in matches)
            {
                string v = match.ToString();
                var sidx = v.IndexOf('[') + 1;
                var eidx = v.IndexOf(']');
                int idx = int.Parse(v.Substring(sidx, eidx - sidx));
                while (idx > len)
                {
                    idx -= len;
                }
                while (idx < 0)
                {
                    idx += len;
                }
                //idx = Mathf.Clamp(idx, 0, slashSplit.Length - 1);
                parsedMatches[i++] = GetPathAtArray(_path, idx);
            }

            i = 0;
            var splitpath = Regex.Split(replacement, pathregex);
            string finalPath = string.Empty;
            foreach (var split in splitpath)
            {
                finalPath += splitpath[i];
                if (i < parsedMatches.Length)
                {
                    finalPath += parsedMatches[i];
                }
                i++;
            }
            return finalPath;
        }
    }
}
