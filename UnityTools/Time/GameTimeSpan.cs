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
            DeltaTime = realTime - LastUpdateTime;
            ActiveTime = realTime - StartTime;
            LastUpdateTime = realTime;
        }

        public void Reset(float time)
        {
            StartTime = time;
            LastUpdateTime = StartTime;
            ActiveTime = 0;
            DeltaTime = 0;
        }
    }
}
