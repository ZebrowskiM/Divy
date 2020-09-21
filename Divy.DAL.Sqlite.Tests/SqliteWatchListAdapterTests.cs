using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using Divy.Common.POCOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Divy.DAL.Sqlite.Tests
{
    [TestClass]
    public class SqliteWatchListAdapterTests
    {
        private readonly string _dbName = "DivyBase.db";
        private readonly string _masterTable = "WatchLists";
        private readonly string _connectionString;

        [TestInitialize]
        public void SetUp()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {
          

            var testPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Divy");
            if (Directory.Exists(testPath))
            {
                try
                {
                    Directory.Delete(testPath, true);
                }
                catch //SqlLite does some internal functions, this waits for them to finish so clean up works
                {
                    System.Threading.Thread.Sleep(2000);
                    Directory.Delete(testPath, true);
                }
            }
        }

        #region Ctor
        [TestMethod]
        public void Ctor_GivenAValidFilePath_CreatesFile()
        {
            var testPath = Path.Combine(Path.GetTempPath(), "DivTestPath");
            Directory.CreateDirectory(testPath);

            var adapter = new SqliteWatchListAdapter(testPath);

            Assert.IsTrue(File.Exists(Path.Combine(testPath, _dbName)));

            Directory.Delete(testPath, true);
        }

        [DataRow("AUtoMatedTests<Make>ComputerGoBrrrr")]
        [DataRow("111111111")]
        [DataRow("BANNANNA")]
        [DataRow("SuP3rS3cr3TGovSite@N0TAR3A51")]
        [DataRow(null)]
        [DataRow("")]
        [DataTestMethod]
        public void Ctor_GivenANonValidFilePath_CreatesFileInAppDataDir(string path)
        {
            var adapter = new SqliteWatchListAdapter(path);

            var testPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Divy");
            Assert.IsTrue(File.Exists(Path.Combine(testPath, _dbName)));

            Directory.Delete(testPath, true);
        }
        #endregion

        #region CreateWatchList
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWatchList_NullWatchList_ThrowsNullArgExp()
        {

            var adapter = new SqliteWatchListAdapter(null);

            adapter.CreateWatchList(null);
        }

        [TestMethod]
        public void CreateWatchList_ValidWatchList_SuccessfullyCreatesWatchList()
        {
            var adapter = new SqliteWatchListAdapter(null);
            var watchList = getValidWatchList();

            var watchListId = adapter.CreateWatchList(watchList);
            Assert.AreEqual(watchListId,1);

        }
        #endregion

        #region GetWatchListById
        [DataRow(-1)]
        [DataRow(-999)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [DataTestMethod]
        public void GetWatchListById_watchListIdIsNegative_throwsArgOutOfRangeEx(int id)
        {
            var adapter = new SqliteWatchListAdapter(null);

            adapter.GetWatchListById(id);
        }
        #endregion

        #region UpdateWatchListbyId
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateWatchListById_watchListIdIsNegative_ThrowsArgEx()
        {
            var adapter = new SqliteWatchListAdapter(null);

            adapter.UpdateWatchlistById(-1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateWatchListById_watchListIsNull_throwsArgNullEx()
        {
            var adapter = new SqliteWatchListAdapter(null);

            adapter.UpdateWatchlistById(2, null);
        }
        #endregion

        #region DeleteWatchListById

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteWatchListById_watchListIdIsNegative_ThrowsArgEx()
        {
            var adapter = new SqliteWatchListAdapter(null);

            adapter.DeleteWatchListById(-1);
        }

        #endregion

        #region TestHelperMethods

        private WatchList getValidWatchList()
        {
            return new WatchList
            {
                Name = "ValidWatchList",
                Shares = new List<Share>
                {
                    new Share
                    {
                        AverageCost = 15.00,
                        Description = "TestShare",
                        Dividend = 1.0,
                        MarketCap = 200000,
                        Name = "Microsoft",
                        TickerSymbol = "MSFT",
                        NumberOfShares = 5,
                        PriceToEarningsRatio = 5.0,
                        SharePrice = 200
                    },
                    new Fund
                    {
                        AverageCost = 30,
                        Description = "TestFund",
                        MarketCap = 20000,
                        Dividend = 2.5,
                        ExpenseRatio = .03,
                        Name = "S&P 500 Div fund",
                        NumberOfHoldings = 500,
                        NumberOfShares = 50,
                        PriceToEarningsRatio = 50.0,
                        SharePrice = 30,
                        TickerSymbol = "SPHD"
                    }
                }
            };
        }

        #endregion
    }
}
