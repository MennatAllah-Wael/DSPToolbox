using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            int n = InputSignal.Samples.Count;
            List<float> x = new List<float>(InputSignal.Samples);
            List<float> samples = new List<float>();
            List<int> idx = new List<int>();
            for (int i = n - 1; i >= 0; i--)
            {
                samples.Add(x[i]);
                idx.Add(InputSignal.SamplesIndices[i] * -1);
            }
            OutputFoldedSignal = new Signal(samples, idx, true);
        }
    }
}
