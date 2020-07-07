using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pixel.ECS
{
    public class Metrics
    {
        public DateTime Created;
        public List<float> Values = new List<float>();
        public float Max;
        public double Sum;
        public float Avg => (float)(Sum / Values.Count);

        public Metrics() => Created = DateTime.UtcNow;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void AddValue(float val)
        {
            Values.Add(val);
            if (val > Max)
                Max = val;
            Sum += val;
        }
    }
}