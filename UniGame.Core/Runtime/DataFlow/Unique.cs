namespace UniGreenModules.UniGame.Core.Runtime.DataFlow
{
    using System.Threading;

    public static class Unique
    {
        private static int nextId = 0;
        
        public static int GetId()
        {
            return Interlocked.Add(ref nextId, 1);
        }
        
    }
}
