using UnityEngine.Timeline;

namespace UniModules.UniGame.Tools.ExposedReferences
{
    using UnityEngine.Playables;

    [TrackColor(0.5f,0.5f,0.5f)]
    [TrackClipType(typeof(DemoClip))]
    public class DemoTrack : TrackAsset
    {
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            base.GatherProperties(director, driver);
        }
    }
    
}
