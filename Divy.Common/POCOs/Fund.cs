using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Divy.Common.POCOs
{
    public class Fund : Share
    {
        [DataMember] public double ExpenseRatio;

        [DataMember] public int NumberOfHoldings;

    }
}
