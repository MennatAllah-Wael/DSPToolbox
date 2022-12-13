using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;
namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            int N = 0;
            //if(InputFreqDomainSignal.FrequenciesAmplitudes != null)
            N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            Complex c, e, sum;
            List<Complex> x = new List<Complex>();
            List<float> output = new List<float>();
            double real, img;
            for (int i = 0; i < N; i++)
            {
                real = (double)InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                img = (double)InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                c = new Complex(real, img);
                x.Add(c);
            }
            for (int k = 0; k < N; k++)
            {
                sum = new Complex();
                for (int n = 0; n < N; n++)
                {
                    real = Math.Cos(2 * Math.PI * k * n / N);
                    img = Math.Sin(2 * Math.PI * k * n / N);
                    e = new Complex(real, img);
                    sum += x[n] * e;
                }
                sum /= N;
                output.Add((float)sum.Real);
            }
            OutputTimeDomainSignal = new Signal(output, false);
        }
    }
}
