using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            // throw new NotImplementedException();
            //Math.Round(Frequencies[i], 1)
            // l : up , M : down
            int N = InputSignal.Samples.Count;
            List<float> list = new List<float>();
            FIR FIR_Obj_Name = new FIR();
            FIR_Obj_Name.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            FIR_Obj_Name.InputFS = 8000;
            FIR_Obj_Name.InputStopBandAttenuation = 50;
            FIR_Obj_Name.InputCutOffFrequency = 1500;
            FIR_Obj_Name.InputTransitionBand = 500;
            FIR_Obj_Name.InputTimeDomainSignal = new Signal(new List<float>(), new List<int>(), false);
            int idx = InputSignal.SamplesIndices[0];
            List<int> index = new List<int>();
            OutputSignal = new Signal(new List<float>(), new List<int>(), true);
            if (M == 0 && L != 0)
            {
                for (int i = 0; i < N; i++)
                {
                    list.Add(InputSignal.Samples[i]);
                    index.Add(idx);
                    idx++;
                    for (int j = 0; j < L - 1; j++)
                    {
                        list.Add(0);
                        index.Add(idx);
                        idx++;
                    }
                }
                FIR_Obj_Name.InputTimeDomainSignal = new Signal(list, index, true);
                FIR_Obj_Name.InputTimeDomainSignal.SamplesIndices = new List<int>(index);
                FIR_Obj_Name.Run();
                OutputSignal = new Signal(FIR_Obj_Name.OutputYn.Samples, FIR_Obj_Name.OutputYn.SamplesIndices, true);

            }
            else if (M != 0 && L == 0)
            {
                FIR_Obj_Name.InputTimeDomainSignal = InputSignal;
                FIR_Obj_Name.Run();
                idx = FIR_Obj_Name.OutputYn.SamplesIndices[0];
                //M--;
                for (int i = 0; i < FIR_Obj_Name.OutputYn.Samples.Count; i += M)
                {
                    list.Add(FIR_Obj_Name.OutputYn.Samples[i]);
                    index.Add(idx);
                    idx++;
                }
                OutputSignal = new Signal(list, index, true);
            }
            else if (M != 0 && L != 0)
            {
                List<float> list1 = new List<float>();
                for (int i = 0; i < N; i++)
                {
                    list1.Add(InputSignal.Samples[i]);
                    index.Add(idx);
                    idx++;
                    for (int j = 0; j < L - 1; j++)
                    {
                        list1.Add(0);
                        index.Add(idx);
                        idx++;
                    }
                }
                FIR_Obj_Name.InputTimeDomainSignal = new Signal(list1, index, false);

                FIR_Obj_Name.Run();
                idx = FIR_Obj_Name.OutputYn.SamplesIndices[0];
                index = new List<int>();
                //M--;
                for (int i = 0; i < FIR_Obj_Name.OutputYn.Samples.Count; i += M)

                {
                    list.Add(FIR_Obj_Name.OutputYn.Samples[i]);
                    index.Add(idx);
                    idx++;
                }
                OutputSignal = new Signal(list, index, true);
            }
        }
    }

}