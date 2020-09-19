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
         public string TickerSymbol { get; set; }

         public string Name { get; set; }

         public string Description { get; set; }

         public double AverageCost { get; set; }

         public double SharePrice { get; set; }

         public int NumberOfShares { get; set; }

         public double PriceToEarningsRatio { get; set; }

         public double Dividend { get; set; }

         public long MarketCap { get; set; }

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
