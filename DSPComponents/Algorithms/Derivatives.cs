using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> x = new List<float>(InputSignal.Samples);
            int N = InputSignal.Samples.Count;
            List<float> y1 = new List<float>();
            List<float> y2 = new List<float>();
            //Y(n) = x(n)-x(n-1)
            //Y(n)= x(n+1)-2x(n)+x(n-1)
            y1.Add(x[0]);
            y2.Add(x[1] - 2 * x[0]);
            for (int i = 1; i < N - 1; i++)
            {
                y1.Add(x[i] - x[i - 1]);
                y2.Add(x[i + 1] - 2 * x[i] + x[i - 1]);
            }
            FirstDerivative = new Signal(y1, false);
            SecondDerivative = new Signal(y2, false);
        }
    }
}
