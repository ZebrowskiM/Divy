using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Divy.Common.POCOs
{
    public class Share : ObjectBase
    {
        [DataMember] public string TickerSymbol;

        [DataMember] public string Name;

        [DataMember] public string Description;

        [DataMember] public double AverageCost;

        [DataMember] public double SharePrice;

        [DataMember] public int NumberOfShares;

        [DataMember] public double PriceToEarningsRatio;

        [DataMember] public double DividendYield;

        [DataMember] public long MarketCap;
    }
}
