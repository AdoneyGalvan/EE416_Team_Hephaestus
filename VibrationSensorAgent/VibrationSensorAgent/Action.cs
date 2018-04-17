using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationSensorAgent
{
    public class Action
    {
        private bool _notify;    // Notification boolean

        public Action() { }      // End constructor

        public bool Notify
        {
            get { return _notify; }
            set { _notify = value; }
        }   // End property

        public string PrintAction(Action action)
        {
            string message;
            if (action.Notify == true)
            {
                // Notify the user
                return message = "Notify maintenance!";
            }
            else
            {
                // Notify the user
                return message = "Pass!";
            }
        }   // End function
    }   // End class
}
