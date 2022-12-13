using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            int N = InputTimeDomainSignal.Samples.Count;
            List<float> phase, amplitude;
            //real = new List<float>();
            //img = new List<float>();
            amplitude = new List<float>();
            phase = new List<float>();
            //freq = new List<float>();
            float x, y;


            for (int k = 0; k < N; k++)
            {
                x = 0;
                y = 0;
                for (int n = 0; n < N; n++)
                {
                    x += InputTimeDomainSignal.Samples[n] * ((float)Math.Cos((float)2 * Math.PI * k * n / N));
                    y += InputTimeDomainSignal.Samples[n] * (-1.0f * (float)Math.Sin((float)2 * Math.PI * k * n / N));
                }
                //real.Add(x);
                //img.Add(y);
                amplitude.Add((float)Math.Sqrt(x * x + y * y));
                phase.Add((float)Math.Atan2(y, x));
            }
            //float f = (float)(2 * Math.PI * InputSamplingFrequency / N);
            //for(int i = 0; i < N; i ++)
            //    freq.Add(f * i);
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, true);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>(amplitude);
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>(phase);
            //OutputFreqDomainSignal.Frequencies = new List<float>(freq);
        }
    }
}
