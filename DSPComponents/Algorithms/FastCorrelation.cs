using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            List<float> y, prod = new List<float>();
            List<Complex> x1 = new List<Complex>(), x2 = new List<Complex>();
            Complex c = new Complex();
            Signal s1, s2;
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            int N, N1, N2;
            float Norm1 = 0, Norm2 = 0, Norm = 0;

            if (InputSignal1 == null)
                InputSignal1 = new Signal(InputSignal2.Samples, false);
            else if (InputSignal2 == null)
                InputSignal2 = new Signal(InputSignal1.Samples, false);
            N1 = InputSignal1.Samples.Count;
            N2 = InputSignal2.Samples.Count;
            if (N1 < N2)
            {
                for (int i = N1; i <= N2; i++)
                    InputSignal1.Samples.Add(0.0f);
                N = N2;
            }
            else if (N1 > N2)
            {
                for (int i = N2; i <= N2; i++)
                    InputSignal2.Samples.Add(0.0f);
                N = N1;
            }
            else
                N = N1;
            for (int i = 0; i < N; i++)
            {
                Norm1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                Norm2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }
            Norm = (float)Math.Sqrt(Norm1 * Norm2) / N;

            dft.InputTimeDomainSignal = InputSignal1;
            dft.Run();
            s1 = dft.OutputFreqDomainSignal;
            dft.InputTimeDomainSignal = InputSignal2;
            dft.Run();
            s2 = dft.OutputFreqDomainSignal;
            for (int i = 0; i < N; i++)
            {
                c = new Complex((float)s1.FrequenciesAmplitudes[i] * Math.Cos(s1.FrequenciesPhaseShifts[i]),
                    -1.0f * (float)s1.FrequenciesAmplitudes[i] * Math.Sin(s1.FrequenciesPhaseShifts[i]));
                x1.Add(c);
                c = new Complex((float)s2.FrequenciesAmplitudes[i] * Math.Cos(s2.FrequenciesPhaseShifts[i]),
                    (float)s2.FrequenciesAmplitudes[i] * Math.Sin(s2.FrequenciesPhaseShifts[i]));
                x2.Add(c);
                prod.Add((float)(x1[i] * x2[i]).Magnitude);
            }
            idft.InputFreqDomainSignal = new Signal(prod, true);
            idft.InputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            idft.InputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            for (int i = 0; i < N; i++)
            {
                c = (x1[i] * x2[i]);
                idft.InputFreqDomainSignal.FrequenciesAmplitudes.Add((float)Math.Sqrt(c.Real * c.Real + c.Imaginary * c.Imaginary));
                idft.InputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)Math.Atan2(c.Imaginary, c.Real));
            }
            idft.Run();
            List<float> out_idft = new List<float>(idft.OutputTimeDomainSignal.Samples);
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            y = new List<float>();
            for (int i = 0; i < out_idft.Count; i++)
            {
                OutputNonNormalizedCorrelation.Add(out_idft[i] / N);
                OutputNormalizedCorrelation.Add((out_idft[i] / N) / Norm);
            }
        }
    }
}