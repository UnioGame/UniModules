namespace UniGame.Shared.Runtime.Timeline
{
    using System.Collections.Generic;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    public static class TrackAssetExtensions
    {
        public static IEnumerable<T> GetClipAssets<T>(this TrackAsset trackAsset) where T : PlayableAsset
        {
            if(trackAsset == null)
                yield break;
            
            var clips = trackAsset.GetClips();
            foreach (var timelineClip in clips)
            {
                if (timelineClip.asset is T clipAsset)
                    yield return clipAsset;
            }
        }

        public static IEnumerable<TimelineClip> GetClips<T>(this TrackAsset trackAsset) where T : PlayableAsset
        {
            if(trackAsset == null)
                yield break;
            
            var clips = trackAsset.GetClips();
            foreach (var timelineClip in clips)
            {
                if (timelineClip.asset is T)
                    yield return timelineClip;
            }
        }
    }
}