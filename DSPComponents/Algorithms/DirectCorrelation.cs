using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            float Norm1 = 0, Norm2 = 0, Norm = 0;
            int N = InputSignal1.Samples.Count, N2;

            if (InputSignal2 == null)
                InputSignal2 = new Signal(InputSignal1.Samples, false);
            N2 = InputSignal2.Samples.Count;
            N = Math.Max(N, N2);

            for (int i = 0; i < N; i++)
            {
                Norm1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                Norm2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
                InputSignal2.Samples.Add(0);
                InputSignal1.Samples.Add(0);
            }
            Norm = (float)Math.Sqrt(Norm1 * Norm2) / N;

            int k;
            float Ans;

            for (int i = 0; i < N; i++)
            {
                Ans = 0;
                for (int j = 0; j < N; j++)
                {
                    if (InputSignal1.Periodic)
                        k = (i + j) % N;
                    else
                        k = i + j;
                    Ans += InputSignal1.Samples[j] * InputSignal2.Samples[k];
                }
                Ans /= N;
                OutputNonNormalizedCorrelation.Add(Ans);
                OutputNormalizedCorrelation.Add(Ans / Norm);
            }
        }
    }
}