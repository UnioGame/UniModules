using System;
using UniGreenModules.UniGame.Core.Runtime.DataStructure;

namespace UniModules.UniGame.Core.Runtime.DataStructure
{
    using System.Collections.Generic;

    [Serializable]
    public class SerializableStringsMap : 
        SerializableDictionary<string,string>
    {
        public SerializableStringsMap(int capacity) : base(capacity)
        {
            
        }
    }
}
