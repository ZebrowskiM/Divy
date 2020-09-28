using System;
using System.Collections.Generic;
using System.Text;
using Divy.Common.POCOs;

namespace Divy.Common
{
    public class Validations
    {
        // Static methods for validation the data of a fund and share 
        public static bool IsShareValid(Share share)
        {
            if (share == null)
                return false;
            if (string.IsNullOrWhiteSpace(share.Name))
            {
                Tracing.Error("Share Failed to Validate, unable to read Name value");
                return false;
            }
            var strIsNotValid = "Share " + share.Name + " is not valid";
            if (share.AverageCost < 0)
            {
                Tracing.Warning( strIsNotValid+ ",Average cost cannot be less than zero" );
                return false;
            }
            if (share.MarketCap < 0)
            {
                Tracing.Warning(strIsNotValid + ", MarketCap cannot be negative");
                return false;
            }
            if (share.NumberOfShares < 0)
            {
                Tracing.Warning(strIsNotValid + ", Number of shares cannot be negative, Shorting is not supported");
                return false;
            }
            if (string.IsNullOrWhiteSpace(share.TickerSymbol))
            {
                Tracing.Warning(strIsNotValid + ", Share must have valid ticker symbol");
                return false;
            }
            if (share.SharePrice < 0)
            {
                Tracing.Warning(strIsNotValid + ", Share Price cannot be negative ");
                return false;
            }

            return true;

        }

        public static bool IsFundValid(Fund fund)
        {
            if (fund == null)
                return false;

            if (fund.ExpenseRatio < 0)
            {
                Tracing.Warning("Fund cannot have negative expense ratio" );
                return false;
            }

            if (fund.NumberOfHoldings < 0)
            {
                Tracing.Warning("Funds cannot have negative holdings, inverse etfs holdings are counted as a holding");
                return false;
            }

            return IsShareValid(fund);
        }
    }
}
