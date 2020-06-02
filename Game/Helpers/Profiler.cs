using System.Diagnostics;

namespace PixelGlueCore.Helpers
{
    public class Profiler
    {
        public Stopwatch stopwatch;
        public double Time = 0;
        public Profiler() => stopwatch = new Stopwatch();
        public void StartMeasuring() => stopwatch.Restart();

        public void StopMeasuring()
        {
            stopwatch.Stop();
            Time = stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}