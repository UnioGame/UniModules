using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Assets.Editor.Utility;
using UniModule.UnityTools.EditorTools;
using Object = UnityEngine.Object;

namespace CustomDefineManagement
{
    public partial class CustomDefineManager : EditorWindow
    {
        private const string _definesDirectory = "Assets/Resources";
        private const string _definesFileName = "CustomDefineManagerData";
        private const string _extension = "xml";
        
        private static List<Directive> GetDirectivesFromXmlFile()
        {
            var directives = new List<Directive>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Directive>));
            using(TextReader reader = new StreamReader(GetXmlAssetPath()))
            {
                directives = (List<Directive>)serializer.Deserialize(reader);
            }            

            return directives;
        }

        private static void SaveDataToXmlFile(List<Directive> directives)
        {
            if (!Directory.Exists(_definesDirectory))
            {
                Directory.CreateDirectory(_definesDirectory);
            }

            var assetPath = GetXmlAssetPath();

            var serializer = new XmlSerializer(typeof(List<Directive>));
            using(TextWriter writer = new StreamWriter(assetPath))
            {
                serializer.Serialize(writer, directives);
            }

            AssetDatabase.Refresh();
        }

        private static string GetXmlAssetPath()
        {
            return Path.Combine(_definesDirectory,_definesFileName) + "." + _extension;
        }
    }

    [Serializable]
    public class CustomDefineManagerData
    {        
        public List<Directive> directives = new List<Directive>();
    }
}
