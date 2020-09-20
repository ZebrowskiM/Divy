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

        [TestMethod]
        public void Ctor_GivenAValidFilePath_CreatesFile()
        {
            var testPath = Path.Combine(Path.GetTempPath(), "DivTestPath");
            Directory.CreateDirectory(testPath);

            var adapter = new SqliteWatchListAdapter(testPath);

            Assert.IsTrue(File.Exists(Path.Combine(testPath,_dbName)));

            Directory.Delete(testPath,true);
        }

        [TestMethod]
        public void Ctor_GivenANullFilePath_CreatesFileInAppDataDir()
        {
            var adapter = new SqliteWatchListAdapter(null);

            var testPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Divy");
            Assert.IsTrue(File.Exists(Path.Combine(testPath, _dbName)));

            Directory.Delete(testPath, true);
        }
        [DataRow("AUtoMatedTests<Make>ComputerGoBrrrr")]
        [DataRow("111111111")]
        [DataRow("BANNANNA")]
        [DataRow("SuP3rS3cr3TGovSite@N0TAR3A51")]
        [DataRow(null)]
        [DataTestMethod]
        public void Ctor_GivenANonValidFilePath_CreatesFileInAppDataDir(string path)
        {
            var adapter = new SqliteWatchListAdapter(path);

            var testPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Divy");
            Assert.IsTrue(File.Exists(Path.Combine(testPath, _dbName)));

            Directory.Delete(testPath, true);
        }
    }
}
