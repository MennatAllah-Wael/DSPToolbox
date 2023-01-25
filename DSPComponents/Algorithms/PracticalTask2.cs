using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            displaySig(InputSignal, 0, "signal");
            saveFile("signal", 0, 0, InputSignal);
            Signal out_ = new Signal(new List<float>(), new List<int>(), true);
            FIR fir = new FIR();
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.InputFS = Fs;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputTimeDomainSignal = InputSignal;
            fir.InputFilterType = FILTER_TYPES.BAND_PASS;
            fir.Run();
            out_ = fir.OutputYn;
            displaySig(out_, 0, "fir");
            saveFile("fir", 0, 0, out_);

            if (newFs >= 2 * maxF)
            {
                Sampling sampling = new Sampling();
                sampling.L = L;
                sampling.M = M;
                sampling.InputSignal = fir.OutputYn;
                sampling.Run();
                out_ = sampling.OutputSignal;
                displaySig(out_, 0, "sampling");
                saveFile("sampling", 0, 0, out_);

            }

            DC_Component dc = new DC_Component();
            dc.InputSignal = out_;
            dc.Run();
            out_ = dc.OutputSignal;
            displaySig(out_, 0, "dc");
            saveFile("dc", 0, 0, out_);

            Normalizer norm = new Normalizer();
            norm.InputSignal = out_;
            norm.Run();
            out_ = norm.OutputNormalizedSignal;
            displaySig(out_, 0, "normalization");
            saveFile("normalization", 0, 0, out_);

            DiscreteFourierTransform dFt = new DiscreteFourierTransform();
            dFt.InputTimeDomainSignal = out_;
            dFt.InputSamplingFrequency = Fs;
            dFt.Run();
            out_ = dFt.OutputFreqDomainSignal;
            displaySig(out_, 1, "dft");
            saveFile("dft", 1, 0, out_);

        }
        public void saveFile(String name, int type, int periodic, Signal outSig)
        {
            String fullpath = "E:\\FCIS\\4th year\\first term\\DSP\\fcisdsp-dsp.toolbox-78ddd969882b\\DSPToolbox" + name + ".txt";
            using (StreamWriter writer = new StreamWriter(fullpath))
            {
                writer.WriteLine(type);
                writer.WriteLine(periodic);
                if (type == 1)
                {
                    writer.WriteLine(outSig.Frequencies.Count);
                    for (int i = 0; i < outSig.Frequencies.Count; i++)
                    {
                        writer.Write(outSig.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(outSig.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.Write(outSig.FrequenciesPhaseShifts[i]);
                        writer.WriteLine();
                    }
                }
                else
                {
                    writer.WriteLine(outSig.Samples.Count);
                    for (int i = 0; i < outSig.Samples.Count; i++)
                    {
                        writer.Write(outSig.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.Write(outSig.Samples[i]);
                        writer.WriteLine();
                    }
                }
                writer.Close();
            }

        }
        public void displaySig(Signal input, int type, String name)
        {
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine(name);
            Console.WriteLine("-----------------------------------------------------------------------");
            if (type == 1)
            {
                for (int i = 0; i < input.Frequencies.Count; i++)
                {
                    Console.Write(input.Frequencies[i]);
                    Console.Write(" ");
                    Console.Write(input.FrequenciesAmplitudes[i]);
                    Console.Write(" ");
                    Console.Write(input.FrequenciesPhaseShifts[i]);
                    Console.WriteLine();
                }
            }
            else
            {
                for (int i = 0; i < input.Samples.Count; i++)
                {
                    Console.Write(input.SamplesIndices[i]);
                    Console.Write(" ");
                    Console.Write(input.Samples[i]);
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);
            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());
            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));
            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }
            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }
            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());
                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }
            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
