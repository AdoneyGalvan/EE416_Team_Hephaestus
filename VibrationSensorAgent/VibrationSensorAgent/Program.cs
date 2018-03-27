using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationSensorAgent
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Sample data
            double[] sample = { 99, 101, 99, 102, 99, 103, 99, 104, 105 };
            Agent agent = new Agent();  // Initialize agent

            Action action = agent.Process(sample);  // Process the data
            action.PrintAction(action); // Print results
            Console.ReadLine();
        }   // End main
    }   // End class
}
