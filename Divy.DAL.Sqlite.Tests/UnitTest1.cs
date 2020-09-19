using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Divy.DAL.Sqlite.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Divy");
        private readonly string dbName = "DivyBase.db"; 
        [TestMethod]
        public void TestMethod1()
        {
            //Data Source=database.sqlite;Version=3;
            var path = Path.Combine(folderPath, dbName);
            if(!System.IO.File.Exists(path))
                SQLiteConnection.CreateFile(path);
            using var con = new SQLiteConnection($"DataSource={path};Version=3;");
            con.Open();
            var tran = con.BeginTransaction();
            
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = @"CREATE TABLE cars(id INTEGER PRIMARY KEY,
                    name TEXT, price INT)";
            cmd.ExecuteNonQuery();

            //cmd.CommandText = "BEGIN TRANSACTION;" +
            //                  "INSERT INTO cars(name, price) VALUES('Volkswagen',21600);" + //never works
            //                  "INSERT INTO cars(name, price) VALUES('Mercedes',57127);" +
            //                  "INSERT INTO cars(name, price) VALUES('Skoda',9000);" +
            //                  "INSERT INTO cars(name, price) VALUES('Hummer',41400);" +
            //                  "COMMIT;";
            cmd.CommandText = "INSERT INTO cars(name, price) VALUES" +
                              "('Volkswagen',21600)," +
                              "('Mercedes',57127)," +
                              "('Skoda',9000)," +
                              "('Hummer',41400);";
                              cmd.ExecuteNonQuery();
            tran.Rollback();
            //cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Mercedes',57127)";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Skoda',9000)";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Volvo',29000)";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Bentley',350000)";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Citroen',21000)";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Hummer',41400)";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Volkswagen',21600)";
            //cmd.ExecuteNonQuery();


        }

        [TestMethod]
        public void drop()
        {
            var path = Path.Combine(folderPath, dbName);
            using var con = new SQLiteConnection($"DataSource={path};Version=3;");
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "DROP TABLE cars";
            var result = cmd.ExecuteNonQuery();
        }

        [TestMethod]
        public void test2()
        {
            //System.Data.Common.DataRecordInternal
            var path = Path.Combine(folderPath, dbName);
            using var con = new SQLiteConnection($"DataSource={path};Version=3;");
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "Select * FROM cars";
            var result = cmd.ExecuteReader();
            var resultList = new List<object>();
            while (result.Read())
            {
            
               
                foreach (var VARIABLE in result)
                {
                    resultList.Add(VARIABLE);
                }
            }
            //  var name = resultList[1].D
            Assert.IsFalse(resultList.Count == 0);
            Assert.IsTrue(resultList.Count != 0);

        }

        [TestMethod]
        public void Test3()
        {
            var path = Path.Combine(folderPath, dbName);
            using var con = new SQLiteConnection($"DataSource={path};Version=3;");
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "Select * FROM WatchLists";
            var result = cmd.ExecuteReader();

            

        }
    }
}
