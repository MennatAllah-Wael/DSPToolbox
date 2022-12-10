using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> output = new List<float>();
            int n;
            if (InputSignal1.Samples.Count >= InputSignal2.Samples.Count)
                n = InputSignal1.Samples.Count;
            else
                n = InputSignal2.Samples.Count;
            for (int i = 0; i < n; i++)
                output.Add(InputSignal1.Samples[i] + (-1) * InputSignal2.Samples[i]);
            OutputSignal = new Signal(output, true);
        }
    }
}