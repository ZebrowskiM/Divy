using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Divy.Common.POCOs
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Share : ObjectBase
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
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

#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            if (obj?.GetType() != typeof(Share))
                return false;
            var share = obj as Share;
            return string.Equals(share?.Name, Name) && string.Equals(share?.TickerSymbol,TickerSymbol);
        }
    }
}
