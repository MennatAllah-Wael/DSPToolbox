using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int N = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            List<float> x = new List<float>();
            List<int> idx = new List<int>();
            int n = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();
            for (int cnt = 0; cnt < N; cnt++)
            {
                float sum = 0;
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    for (int j = 0; j < InputSignal2.Samples.Count; j++)
                        if (InputSignal1.SamplesIndices.Count > 0 && InputSignal2.SamplesIndices.Count > 0)
                        {
                            if (InputSignal1.SamplesIndices[i] + InputSignal2.SamplesIndices[j] == n)
                                sum += InputSignal1.Samples[i] * InputSignal2.Samples[j];
                        }
                        else
                            if (i + j == n)
                            sum += InputSignal1.Samples[i] * InputSignal2.Samples[j];

                x.Add(sum);
                idx.Add(n);
                n++;
            }
            for (int i = N - 1; i >= 0; i--)
                if (x[i] == 0)
                {
                    x.RemoveAt(i);
                    idx.RemoveAt(i);
                }
                else
                    break;
            OutputConvolvedSignal = new Signal(x, idx, false);
        }
    }
}
