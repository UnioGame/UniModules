using System.Collections;

namespace MagicColoring.RuntimeProfiler
{
    public interface IRuntimeProfiler
    {
        IEnumerator StartFrameRateMonitor();
        void Dispose();
    }
}