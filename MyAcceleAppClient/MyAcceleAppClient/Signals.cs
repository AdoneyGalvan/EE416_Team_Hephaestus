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
        public double[] ComplexFFT(double[] samples, int samplenum)
        {
            Complex32[] data = new Complex32[samplenum];
            double[] caldata = new double[samplenum];

            for (int i = 0; i < samplenum; i++)
            {
                data[i] = new Complex32((float)samples[i], 0);
            }

            //Compute FFT
            Fourier.Radix2Forward(data, FourierOptions.Default);

            for(int i = 0; i < samplenum; i++)
            {
                caldata[i] = data[i].Real;
            }
            return caldata;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Computes magnitude of the complex valued array
        //Inputs: double type array
        //Output: none
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double[] ComplexMagnitude(Complex32[] signalSample, int samplenum)
        {
            double[] magnitudeArr = new double[samplenum];

            for (int i = 0; i < samplenum; i++)
            {
                magnitudeArr[i] = Complex32.Abs(signalSample[i]);
            }

            return magnitudeArr;
        }
    }
}