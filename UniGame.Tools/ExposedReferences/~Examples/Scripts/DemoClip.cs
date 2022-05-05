using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniModules.UniGame.Tools.ExposedReferences
{
    [Serializable]
    public class DemoClip : PlayableAsset, ITimelineClipAsset
    {
        public DemoClipBehaviour clip;
        
        
        public ClipCaps clipCaps => ClipCaps.None;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<DemoClipBehaviour>.Create(graph,clip);
        }

        
        
    }
    
    
    
}
