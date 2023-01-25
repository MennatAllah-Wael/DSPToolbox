using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            List<float> y, prod = new List<float>();
            List<Complex> x1 = new List<Complex>(), x2 = new List<Complex>();
            Complex c = new Complex();
            Signal s1, s2;
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            int N, N1, N2;
            if (InputSignal1 == null)
                InputSignal1 = new Signal(InputSignal2.Samples, false);
            else if (InputSignal2 == null)
                InputSignal2 = new Signal(InputSignal1.Samples, false);
            N1 = InputSignal1.Samples.Count;
            N2 = InputSignal2.Samples.Count;
            N = N1 + N2 - 1;
            if (N1 != N2)
            {
                for (int i = N1; i < N; i++)
                    InputSignal1.Samples.Add(0.0f);
                for (int i = N2; i < N; i++)
                    InputSignal2.Samples.Add(0.0f);
            }
            dft.InputTimeDomainSignal = InputSignal1;
            dft.Run();
            s1 = dft.OutputFreqDomainSignal;
            dft.InputTimeDomainSignal = InputSignal2;
            dft.Run();
            s2 = dft.OutputFreqDomainSignal;
            for (int i = 0; i < N; i++)
            {
                c = new Complex((float)s1.FrequenciesAmplitudes[i] * Math.Cos(s1.FrequenciesPhaseShifts[i]),
                    (float)s1.FrequenciesAmplitudes[i] * Math.Sin(s1.FrequenciesPhaseShifts[i]));
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
            y = new List<float>();
            OutputConvolvedSignal = new Signal(out_idft, false);
        }
    }
}