using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Divy.Common.POCOs
{
    public class Fund : Share
    {
        public double ExpenseRatio { get; set; }

        public int NumberOfHoldings { get; set; }

    }
}
