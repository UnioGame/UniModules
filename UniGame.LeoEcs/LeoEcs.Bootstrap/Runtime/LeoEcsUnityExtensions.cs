namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using Abstract;
    using Cysharp.Threading.Tasks;

    public static class LeoEcsUnityExtensions
    {
        public static PlayerLoopTiming ConvertToPlayerLoopTiming(this LeoEcsPlayerUpdateType updateType)
        {
            return updateType switch
            {
                LeoEcsPlayerUpdateType.None => PlayerLoopTiming.Initialization,
                LeoEcsPlayerUpdateType.Update => PlayerLoopTiming.Update,
                LeoEcsPlayerUpdateType.FixedUpdate => PlayerLoopTiming.FixedUpdate,
                LeoEcsPlayerUpdateType.LateUpdate => PlayerLoopTiming.LastUpdate,
                _ => PlayerLoopTiming.Update
            };
        }
    }
}