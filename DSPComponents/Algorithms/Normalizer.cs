using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> output = new List<float>();
            float range = InputMaxRange - InputMinRange;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                output.Add((range * (InputSignal.Samples[i] - InputSignal.Samples.Min())
                    / (InputSignal.Samples.Max() - InputSignal.Samples.Min()))
                    + InputMinRange);
            }
            //(b - a) * (x[i] - minx)/(maxx - minx)  + a
            OutputNormalizedSignal = new Signal(output, false);
        }
    }
}
