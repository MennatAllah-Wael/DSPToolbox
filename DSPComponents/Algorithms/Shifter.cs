using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            int k = ShiftingValue;
            List<float> samples = new List<float>(InputSignal.Samples);
            List<int> idx = new List<int>(InputSignal.SamplesIndices);
            if (samples[0] != 1)
                k *= -1;
            for (int i = 0; i < InputSignal.SamplesIndices.Count; i++)
                idx[i] -= k;
            OutputShiftedSignal = new Signal(samples, idx, true);
        }
    }
}
