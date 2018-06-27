using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR	
using UnityEditor;
#endif

namespace Assets.Scripts.ProfilerTools
{
    public static class GameProfiler {

        private static long _currentId = 0;

        private const string WatchMessage = "[{0} ID: {1}] STOPWATCH DURATION : {2}";
        private static Dictionary<long, Stopwatch> _stopwatches = new Dictionary<long, Stopwatch>();
        private static Stack<Stopwatch> _freeStopwatches = new Stack<Stopwatch>();
        private static Dictionary<long, string> _stopwatchesTags = new Dictionary<long, string>();

        private static int ProfileEnabled = -1;
        private const string ProfileEditorKey = "ProfileEditorKey";

        public static bool Enabled = true;


        // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        public static bool IsProfileEnabled
        {
            get
            {
#if UNITY_EDITOR
                if (ProfileEnabled == -1)
                    ProfileEnabled = EditorPrefs.GetBool(ProfileEditorKey, true) ? 1 : 0;

                return ProfileEnabled != 0;
#else
                return false;
#endif
            }
            set
            {
#if UNITY_EDITOR

                var newValue = value ? 1 : 0;
                if (newValue != ProfileEnabled)
                {
                    ProfileEnabled = newValue;
                    EditorPrefs.SetBool(ProfileEditorKey, value);
                }

                Enabled = newValue == 1;
#endif
            }
        }

#if UNITY_EDITOR

        private const string _profileMode = "Tools/Profile/Enable Profiling";

        [MenuItem(_profileMode)]
        public static void ToggleSimulationMode()
        {
            IsProfileEnabled = !IsProfileEnabled;
        }

        [MenuItem(_profileMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(_profileMode, IsProfileEnabled);
            return true;
        }

#endif

        [Conditional("ENABLE_UNITY_PROFILING")]
        public static void BeginSample(string key)
        {
            if (!Enabled) return;
            Profiler.BeginSample(key);
        }

        [Conditional("ENABLE_UNITY_PROFILING")]
        public static void EndSample()
        {
            if (!Enabled) return;
            Profiler.EndSample();
        }
        
        public static long BeginWatch(string name, bool logProfilingStart = false) {
#if ENABLE_PROFILING
            if (!Enabled) return -1;
            return BeginWatchRunTime(name, logProfilingStart);
#endif
            return -1;
        }

        public static long BeginWatchFormat(string name, params object[] args)
        {
#if ENABLE_PROFILING
            if (!Enabled) return -1;
            return BeginWatchRunTime(string.Format(name,args));
#endif
            return -1;
        }

        [Conditional("ENABLE_PROFILING")]
        public static void StopWatch(long id)
        {
            if (!Enabled) return;
            StopWatchRunTime(id);
        }

        public static long BeginWatchRunTime(string name, bool logStart = false) {

            if (!Enabled || name == null) return -1;

            _currentId++;
            if (_currentId < 0) _currentId = 0;

            var id = _currentId;

            if(logStart) Debug.LogFormat("[{0} ID {1}] STOPWATCH STARTED",name,id);

            var watch = _freeStopwatches.Count == 0 ? new Stopwatch()
                : _freeStopwatches.Pop();

            _stopwatches[id] = watch;
            _stopwatchesTags[id] = name;

            watch.Reset();
            watch.Start();

            return id;
        }
        
        public static void StopWatchRunTime(long id) {

            if (!Enabled || id <= 0) return;

            Stopwatch watch = null;
            if (_stopwatches.TryGetValue(id, out watch)) {

                var name = _stopwatchesTags[id];
               Debug.LogFormat(WatchMessage, name,id, watch.ElapsedMilliseconds);
                watch.Stop();
                watch.Reset();

                _freeStopwatches.Push(watch);
                _stopwatches.Remove(id);
                _stopwatchesTags.Remove(id);
            }
            else {
                GameLog.LogErrorFormat("STOPWATCH error with id [{0}]",id);
            }
       
        }

    }
}
