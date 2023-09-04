namespace UniGame.Shared.Runtime.Timeline
{
    using System.Collections.Generic;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    public static class PlayableAssetExtensions
    {
        public static T GetTrack<T>(this PlayableAsset playableAsset) where T : TrackAsset
        {
            if (!(playableAsset is TimelineAsset timelineAsset))
                return null;

            foreach (var outputTrack in timelineAsset.GetOutputTracks())
            {
                if (outputTrack is T track)
                    return track;
            }

            return null;
        }
        
        public static IEnumerable<T> GetTracks<T>(this PlayableAsset playableAsset) where T : TrackAsset
        {
            if (!(playableAsset is TimelineAsset timelineAsset))
                yield break;

            foreach (var outputTrack in timelineAsset.GetOutputTracks())
            {
                if (outputTrack is T track)
                    yield return track;
            }
        }
    }
}