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
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Class contains:
    //      Radix2FFT
    //      RMS analysis
    //      Frequency axis scaling
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    class SigFunctions
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes Radix 2 FFT
        //Inputs: Complex32 array of signal samples
        //        length of samples array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ComplexFFT(Complex32[] signalSamples, double[] magSamples)
        {
            //Compute FFT
            Fourier.Radix2Forward(signalSamples, FourierOptions.Default);

            ComplexMagnitude(magSamples);

            //Returns max value of array
            double peak = Max(magSamples);

            //Normalizes array
            for (int i = 0; i < LengthSamples; i++)
            {
                magSamples[i] = magSamples[i] / peak;
            }
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes magnitude of the complex valued array
        //Inputs: double type array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ComplexMagnitude(double[] magnitudeArr)
        {
            for (int i = 0; i < LengthSamples; i++)
            {
                magnitudeArr[i] = Complex32.Abs(ComplexSignalSamples[i]);
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes RMS value for a signal samples
        //Inputs: Complex32 array of signal samples
        //        Int of length of array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double RMS(double[] signalSamples, int length)
        {
            //Initialize RMS
            double RMS = 0;
            for (int i = 0; i < length; i++)
            {
                //            n
                //RMS = sqrt{(E Xn)/n} 
                //           i=0
                RMS += (signalSamples[i] * signalSamples[i]);
            }

            RMS = ((double)1 / length) * RMS;

            //Returns RMS value
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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Returns maximum value of the array
        //Inputs: array of doubles
        //Output: double of max value
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double Max(double[] arr)
        {
            double maxVal = arr[0];

            for (int i = 0; i < LengthSamples - 1; i++)
            {
                //Assigns new max value if next in array is greater than prev
                if (arr[i + 1] > arr[i]) maxVal = arr[i + 1];
            }

            return maxVal;
        }

        public Complex32[] ComplexSignalSamples { get; set; }
        public double[] RealSignalSamples { get; set; }
        public int LengthSamples { get; set; }
        public int SampleFreq { get; set; }
        public int[] indeces;
    }
}