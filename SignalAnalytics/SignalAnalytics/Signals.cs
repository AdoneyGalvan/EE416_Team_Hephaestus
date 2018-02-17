//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//Remove if not debugging, keeps console open
#define DEBUG
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;



namespace SignalAnalytics
{
    class SigFunctions
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes Radix 2 FFT
        //Inputs: Complex32 array of signal samples
        //        length of samples array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void FFT(ref double[] signalSamples, int length)
        {
            Fourier.ForwardReal(signalSamples, length);

            //Normalize FFT Results
            //for (int i = 0; i < length; i++)
            //{
            //    signalSamples[i] = signalSamples[i] / length;
            //}
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes RMS value for a signal samples
        //Inputs: Complex32 array of signal samples
        //        Int of length of array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double RMS(double[] signalSamples, int length)
        {
            double RMS = 0;
            for (int i = 0; i < length; i++)
            {
                RMS += (signalSamples[i] * signalSamples[i]);
            }

            RMS = ((double)1 / length) * RMS;
            return Math.Sqrt(RMS);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Scales the indeces to be frequencies of FFT
        //Inputs: ref int[] indeces, int length, int freq
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void IndexScaling(ref int[] indeces, int length, int freq)
        {
            for (int i = 0; i < length; i++)
            {
                indeces[i] = i * (freq / length);
                indeces[i] -= (freq / 2);
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Generates indeces for the FFT
        //Inputs: length of samples
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void GenerateIndeces(int LengthSamples, int[] indeces)
        {
            indeces[0] = 0;
            for (int i = 0; i < LengthSamples; i++)
            {
                indeces[i] = i;
            }
        }

        public double[] SignalSamples { get; set; }
        public int LengthSamples { get; set; }
        public int SampleFreq { get; set; }
        public int[] indeces;
    }


    //Test driver for SigFunctions Class
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    class Program
    {
        static void Main(string[] args)
        {
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            double[] deg = new double[1000];
            deg[0] = 0;

            double[] samples = new double[1000];

            for (int i = 1; i < 1000; i++)
            {
                deg[i] = deg[i - 1] + 0.01;

                samples[i] = Math.Sin(deg[i]);

                //Console.WriteLine(deg[i]);
                //Console.WriteLine(samples[i]);
                //Console.Write('\n');
            }
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                SigFunctions Sig = new SigFunctions
                {
                SignalSamples = samples,
                LengthSamples = 1000,
                SampleFreq = 1000,
                indeces = new int[1000]
                };


            Sig.GenerateIndeces(Sig.LengthSamples, Sig.indeces);
            //Sig.FFT(ref amp, 14);

            double RMS_val = Sig.RMS(samples, Sig.LengthSamples);

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();

            Console.WriteLine(RMS_val);

            //foreach (double element in amp) Console.WriteLine(element);
            //Console.WriteLine();
            //foreach (double element in Sig.indeces) Console.WriteLine(element);




            #if DEBUG
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
            #endif
        }
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
}