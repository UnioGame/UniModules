using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace CustomDefineManagement
{
    public partial class CustomDefineManager : EditorWindow
    {
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<Directive>));
            using(TextWriter writer = new StreamWriter(GetXmlAssetPath()))
            {
                serializer.Serialize(writer, directives);
            }

            AssetDatabase.Refresh();
        }

        private static string GetXmlAssetPath()
        {
            var assetFile = Resources.Load<TextAsset>("CustomDefineManagerData");
            return AssetDatabase.GetAssetPath(assetFile);
        }
    }

    [Serializable]
    public class CustomDefineManagerData
    {        
        public List<Directive> directives = new List<Directive>();
    }
}
