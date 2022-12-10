using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }
        /*public int to_binary(int n)
        {
            int x = 0;
            while(n != 0)
            {
                x *= 10;
                x += n % 10;
                n /= 10;
            }
            return x;
        }*/
        public override void Run()
        {
            List<float> midpoints = new List<float>();
            List<float> intervals = new List<float>();
            List<float> samples = new List<float>();

            OutputSamplesError = new List<float>();
            OutputEncodedSignal = new List<string>();
            OutputIntervalIndices = new List<int>();

            if (InputNumBits != 0)
                InputLevel = (int)Math.Pow(2, InputNumBits);
            else if (InputLevel != 0)
                InputNumBits = (int)Math.Log(InputLevel, 2);

            float maxi, mini, delta;
            maxi = InputSignal.Samples.Max();
            mini = InputSignal.Samples.Min();
            delta = (maxi - mini) / InputLevel;

            for (int i = 0; i <= InputLevel; i++)
            {
                intervals.Add(mini);
                if (i < InputLevel)
                    midpoints.Add((mini * 2 + delta) / 2);
                mini += delta;
            }
            // midpoints & intervals are calculated
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] >= intervals[j] && InputSignal.Samples[i] <= intervals[j + 1] + 0.0001)
                    {
                        OutputIntervalIndices.Add(j + 1);
                        OutputSamplesError.Add(midpoints[j] - InputSignal.Samples[i]);
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0'));
                        samples.Add(midpoints[j]);
                        break;
                    }
                }
            }
            OutputQuantizedSignal = new Signal(samples, false);
        }
    }
}
