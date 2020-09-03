using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Divy.Common.POCOs
{
    class Fund : Share
    {
        [DataMember] public double ExpenseRatio;

        [DataMember] public int NumberOfHoldings;

        //fund or shares
    }
}
