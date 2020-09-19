using System.IO;
using System.Runtime.InteropServices;
using Divy.Common.POCOs;
using NUnit.Framework;

namespace Divy.Tests
{
    public class BaseObjectTests
    {
        private Share _share;
        private Fund _fund;
        [SetUp]
        public void Setup()
        {
            _share = new Share
            {
                TickerSymbol = "MSFT",
                Name = "Microsoft Corporation",
                Description = "They magic boxes that do very fast math",
                AverageCost = 100.55,
                SharePrice = 200.00,
                NumberOfShares = 500,
                PriceToEarningsRatio = 35.42,
                Dividend = 2.04,
                MarketCap = 2400000000000 // 2.4T 
            };
            _fund = new Fund
            {
                TickerSymbol = "SPY",
                Name = "SPDR S&P 500 ETF Trust",
                Description = "Companies that might make a profit sometimes",
                AverageCost = 5.75,
                SharePrice = 350.00,
                Dividend = 5.73,
                PriceToEarningsRatio = 28.72,
                NumberOfShares = 50000,
                NumberOfHoldings = 500,
                ExpenseRatio = 0.09,
                MarketCap = 274450000000 // 274.45B
            };
        }

        [Test]
        public void ToStringMethod_GivenAShare_CreatesAValidString()
        {
            Assert.AreEqual(_share.ToString(), " TickerSymbol = 'MSFT' \t, Name = 'Microsoft Corporation' \t," +
                                               " Description = 'They magic boxes that do very fast math' \t," +
                                               " AverageCost = 100.55 \t, SharePrice = 200 \t, NumberOfShares = 500 \t," +
                                               " PriceToEarningsRatio = 35.42 \t, Dividend = 2.04 \t," +
                                               " MarketCap = 2400000000000 \t",
                "Failed String Comparison check formatting");
        }

        [Test]
        public void ToStringMethod_GivenAFund_CreatesAValidString()
        {
            Assert.AreEqual(_fund.ToString(), " ExpenseRatio = 0.09 \t, NumberOfHoldings = 500 \t," +
                                              " TickerSymbol = 'SPY' \t, Name = 'SPDR S&P 500 ETF Trust' \t," +
                                              " Description = 'Companies that might make a profit sometimes' \t," +
                                              " AverageCost = 5.75 \t, SharePrice = 350 \t, NumberOfShares = 50000 \t," +
                                              " PriceToEarningsRatio = 28.72 \t, Dividend = 5.73 \t," +
                                              " MarketCap = 274450000000 \t",
                "Failed String Comparison check formatting");
        }

        [Test]
        public void GetPropNames_GivenAShare_CreatesAValidString()
        {
            Assert.AreEqual(_share.GetPropNames(),
                "TickerSymbol\t,Name\t,Description\t,AverageCost\t,SharePrice\t,NumberOfShares\t,PriceToEarningsRatio\t,Dividend\t,MarketCap\t",
                "Failed string compare, check formatting");
        }

        [Test]
        public void GetPropValues_GivenAShare_CreatesAValidString()
        {
            Assert.AreEqual(_share.GetPropValues(),
                "'MSFT'\t,'Microsoft Corporation'\t,'They magic boxes that do very fast math'\t,100.55\t,200\t,500\t,35.42\t,2.04\t,2400000000000\t",
                "Failed string compare, check formatting");

        }

        [Test]
        public void GetPropNames_GivenAFund_CreatesAValidString()
        {
            Assert.AreEqual(_fund.GetPropNames(),
                "ExpenseRatio\t,NumberOfHoldings\t,TickerSymbol\t,Name\t,Description\t,AverageCost\t,SharePrice\t,NumberOfShares\t,PriceToEarningsRatio\t,Dividend\t,MarketCap\t"
                ,"Failed string compare, check formatting");
        }

        [Test]
        public void GetPropValues_GivenAFund_CreatesAValidString()
        {
            Assert.AreEqual(_fund.GetPropValues(),
                "0.09\t,500\t,'SPY'\t,'SPDR S&P 500 ETF Trust'\t,'Companies that might make a profit sometimes'\t,5.75\t,350\t,50000\t,28.72\t,5.73\t,274450000000\t",
                "Failed string compare, check formatting");
        }
    }
}