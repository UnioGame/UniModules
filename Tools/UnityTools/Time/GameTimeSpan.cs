namespace Modules.UnityToolsModule.Tools.UnityTools.Time
{
    public struct GameTimeSpan
    {
        public float StartTime;
        public float ActiveTime;
        public float DeltaTime;
        public float LastUpdateTime;

        public void Update(float realTime)
        {
            ActiveTime = realTime - StartTime;
            DeltaTime = realTime - LastUpdateTime;
            LastUpdateTime = realTime;
        }

        public void Reset()
        {
            StartTime = UnityEngine.Time.realtimeSinceStartup;
            LastUpdateTime = StartTime;
            ActiveTime = 0;
            DeltaTime = 0;
        }
    }
}
