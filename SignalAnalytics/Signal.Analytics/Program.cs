using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;


namespace SignalAnalytics
{
    class SignalAnalytics
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes Radix 2 FFT
        //Inputs: Complex32 array of signal samples
        //        length of samples array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public static void FFT(ref double[] signalSamples, int length, int[] indeces)
        {
            Fourier.ForwardReal(signalSamples, length);

            //Normalize FFT Results
            for (int i = 0; i < length; i++)
            {
                signalSamples[i] = signalSamples[i] / length;
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes RMS value for a signal samples
        //Inputs: Complex32 array of signal samples
        //        Int of length of array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public static double RMS(double[] signalSamples, int length)
        {
            double RMS = 0;
            for (int i = 0; i < length; i++ )
            {
                RMS += ((double)1 / length) * (signalSamples[i] * signalSamples[i]);
            }

            return Math.Sqrt(RMS);
        }

        public static void IndexScaling(ref int[] indeces, int length, int freq)
        {
            for (int i = 0; i < length; i++)
            {
                indeces[i] = i * (freq / length);
                indeces[i] -= (freq / 2);
            }
        }

        public double[] SignalSamples { get; set; }
        public int LengthSamples { get; set; }
        public int SampleFreq { get; set; }
        private int[] indeces;
        
    }
}
