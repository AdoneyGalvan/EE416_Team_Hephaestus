using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationSensorAgent
{
    public class Agent
    {
        private int _breakthroughCount; // Number of times 
        private double _average;        // Running average of data
        private double _threshold;      // Threshold for each data point
        private Stack<Action> _actions; // Stack to hold the actions

        public Agent()
        {
            _breakthroughCount = 5;
            _average = 0;
            _threshold = 100;
            _actions = new Stack<Action>();
        }   // End constructor

        public int BreakthroughCount
        {
            get { return _breakthroughCount; }
            set { _breakthroughCount = value; }
        }   // End property

        public double Average
        {
            get { return _average; }
            set { _average = value; }
        }   // End property

        public double Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }   // End property

        public Action Process(double[] dataSample)
        {
            int count = 0;

            if (_actions.Count == 0)
            {
                for (int i = 0; i < dataSample.Length; i++)
                {
                    if (dataSample[i] > _threshold)
                    {
                        count++;    // Increment number of times past threshold
                        if (count > _breakthroughCount)
                        {
                            // Notify the user
                            _actions.Push(new Action() { Notify = true });
                            break;
                        }
                    }
                    else
                        _actions.Push(new Action() { Notify = false });
                }
            }
            Action action = _actions.Peek();
            _actions.Pop();
            return action;
        }   // End function
    }   // End class
}
