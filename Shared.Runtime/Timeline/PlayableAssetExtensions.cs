namespace UniGame.Shared.Runtime.Timeline
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;
    using Object = UnityEngine.Object;

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
        
        public static TrackAsset GetTrack(this PlayableAsset playableAsset,string trackName)
        {
            if (playableAsset is not TimelineAsset timelineAsset)
                return null;

            foreach (var outputTrack in timelineAsset.GetOutputTracks())
            {
                if (outputTrack.name.Equals(trackName,StringComparison.InvariantCultureIgnoreCase))
                    return outputTrack;
            }

            return null;
        }
        
        public static TrackAsset GetTrack(this PlayableDirector director,string trackName)
        {
            if (director == null) return null;
            
            var timelineAsset = director.playableAsset as TimelineAsset;
            if(timelineAsset == null) return null;
            
            foreach (var track in timelineAsset.GetOutputTracks())
            {
                if (track.name.Equals(trackName,StringComparison.InvariantCultureIgnoreCase))
                    return track;
            }

            return null;
        }
        
        public static T GetTrack<T>(this PlayableAsset playableAsset,string trackName) where T : TrackAsset
        {
            if (playableAsset is not TimelineAsset timelineAsset)
                return null;
            
            foreach (var outputTrack in timelineAsset.GetOutputTracks())
            {
                if (outputTrack is T track && 
                    track.name.Equals(trackName,StringComparison.InvariantCultureIgnoreCase))
                    return track;
            }

            return null;
        }

        public static IEnumerable<TClip> GetClips<TClip, TTrack>(this PlayableAsset animation)
            where TTrack : TrackAsset
            where TClip : PlayableAsset
        {
            foreach (var animationOutput in animation.outputs)
            {
                var source = animationOutput.sourceObject;
                if (source is not TTrack trackAsset) continue;
                var clips = trackAsset.GetClips();
            
                foreach (var clip in clips)
                {
                    if(clip.asset is not TClip clipAsset) continue;
                    yield return clipAsset;
                }
            }
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
        
        public static PlayableDirector SetTrackOutput<TOutput>(
            this PlayableDirector director,
            TrackAsset trackAsset, 
            TOutput output) 
            where TOutput : Object
        {
            if (trackAsset == null || director == null) return director;
            
            foreach (var trackOutput in trackAsset.outputs)
            {
                if (trackOutput.outputTargetType != typeof(TOutput))
                    continue;
                director.SetGenericBinding(trackAsset, output);
            }
            
            return director;
        }
        
        public static PlayableDirector SetTrackOutput(
            this PlayableDirector director,
            string trackName, 
            Object output) 
        {
            if (trackName == null || director == null) return director;

            var track = director.GetTrack(trackName);

            if (track == null) return director;
            director.SetGenericBinding(track, output);
            
            return director;
        }
    }
}