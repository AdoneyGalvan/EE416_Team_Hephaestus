using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing
{
    public class SignalProcessingAccess
    {
        public Complex32[] ComplexSignalSamples { get; set; }
        public double[] RealSignalSamples { get; set; }
        public int LengthSamples { get; set; }
        public int SampleFreq { get; set; }
        public int[] indeces;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes Radix 2 FFT
        //Inputs: Complex32 array of signal samples
        //        length of samples array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double[] ComplexFFT(double[] samples, int samplenum)
        {
            Complex32[] data = new Complex32[samplenum];
            double[] caldata = new double[samplenum];

            for (int i = 0; i < samplenum; i++)
            {
                data[i] = new Complex32((float)samples[i], 0);
            }

            //Compute FFT
            Fourier.Forward(data, FourierOptions.Matlab);

            for (int i = 0; i < samplenum; i++)
            {
                caldata[i] = data[i].Magnitude;
            }

            double max = caldata.Max();

            for (int i = 0; i < samplenum; i++)
            {
                caldata[i] = caldata[i] / max;
            }

            return caldata;
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
        public void IndexScaling(ref double[] indeces, int length, int sampling_freq)
        {
            for (int i = 0; i < length; i++)
            {
                indeces[i] = i * ((double)sampling_freq / length);
                indeces[i] -= ((double)sampling_freq / 2);
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Generates indeces for the FFT
        //Inputs: length of samples
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void GenerateIndeces(ref double[] indeces, int LengthSamples)
        {
            indeces[0] = 0;
            for (int i = 0; i < LengthSamples; i++)
            {
                indeces[i] = i;
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Returns circshift FFT array
        //Inputs: array of doubles, length of array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void FFT_shift(ref double[] arr, int length)
        {
            int half = length / 2;

            //create new temp array
            double[] temp = new double[length + 1];

            //populate temp array with current values
            for (int i = 0; i < length; i++)
            {
                temp[i] = arr[i];
            }

            //for first half of the FFT
            for (int i = 0; i < half + 1; i++)
            {
                arr[i] = temp[half + i];
            }

            //for last half of the FFT
            for (int i = half; i < length - 1; i++)
            {
                arr[i] = temp[i - half];
            }
        }

    }
}

