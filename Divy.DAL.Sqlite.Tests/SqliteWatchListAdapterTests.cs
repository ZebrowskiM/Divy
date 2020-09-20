using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
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
                Directory.Delete(testPath, true);
        }

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

        #region CreateWatchList
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWatchList_NullWatchList_ThrowsNullArgExp()
        {

            var adapter = new SqliteWatchListAdapter(null);

            adapter.CreateWatchList(null);
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
    }
}
