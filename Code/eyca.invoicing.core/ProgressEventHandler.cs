using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core
{
    public class ProgressEventHandler : EventArgs
    {
        public double Current { get; private set; }
        public double Total { get; private set; }
        public string Message { get; private set; }
        public double Percentage { get { return Math.Round((Current / Total) * 100, 2); } }

        public ProgressEventHandler(double current, double total, string message)
        {
            Current = current;
            Total = total;
            Message = message;
        }
    }
}
