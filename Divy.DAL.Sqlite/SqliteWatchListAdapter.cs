using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Divy.Common;
using Divy.Common.POCOs;
using Divy.DAL.Interfaces;

namespace Divy.DAL.Sqlite
{
    public class SqliteWatchListAdapter : IWatchListAdapter
    {
        private readonly string _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Divy");
        private readonly string _dbName = "DivyBase.db";
        private readonly string _masterTable = "WatchLists";
        private readonly string _connectionString;
        public SqliteWatchListAdapter()
        {
            var path = Path.Combine(_folderPath, _dbName);
            if(!File.Exists(path))
                SQLiteConnection.CreateFile(path);
            _connectionString = $"DataSource={path};Version=3;";

        }

        public int CreateWatchList(WatchList watchList)
        {
            var tableName = $"_{Guid.NewGuid()}";

            //BEGIN TRANSACTION;
            //Maybe surround the call with these or run after the table is created
            //COMMIT;
            throw new NotImplementedException();
        }

        public List<object> GetWatchListById(int watchListId)
        {
            if(watchListId < 0) 
                throw new ArgumentOutOfRangeException(nameof(watchListId));
            var tableName = GetTableNameById(watchListId);
            var objs = new List<Object>();
            using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = SelectAllFromWatchListTable(tableName);
                    var result = cmd.ExecuteReader();
                    while (result.Read())
                    {
                        var innerObjList = new List<Object>();
                        innerObjList.Add(result.GetString(1));//ticker
                        innerObjList.Add(result.GetString(2));//Name
                        innerObjList.Add(result.GetString(3));//Desc
                        innerObjList.Add(result.GetFloat(4));//SharePrice
                        innerObjList.Add(result.GetInt32(5));//NumberOfShares
                        innerObjList.Add(result.GetFloat(6));//PR Ratio
                        innerObjList.Add(result.GetFloat(7));//Div Yield
                        innerObjList.Add(result.GetInt16(8));//Market Cap 
                        innerObjList.Add(result.GetFloat(9));//ExpenseRatio
                        innerObjList.Add(result.GetInt32(10));//NumOfHoldings
                        innerObjList.Add(result.GetBoolean(11));//isFund
                        objs.Add(innerObjList);
                        
                    }

                }
            }

            return objs;

            return null;
        }

        public List<object> UpdateWatchlistById(int watchListId)
        {
            throw new NotImplementedException();
        }

        public void DeleteWatchListById(int watchListId)
        {
            throw new NotImplementedException();
        }

        #region SqlLiteConnection

        private string GetTableNameById(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var tableName = "";
            using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = FindTableById(id);
                    var result = cmd.ExecuteReader();
                    while (result.Read())
                    {
                        tableName = result.GetString(1);
                    }

                }
            }

            return tableName;
        }
        #endregion

        #region CmdStringCreationMethods
        private string UpdateAWatchListInMasterTableCmd(int id, string name, int numberOfHoldings = 0)
        {
            return numberOfHoldings != 0 ?
                $"UPDATE {_masterTable} SET Name = '{name}', NumberOfHoldings = {numberOfHoldings} WHERE ID = {id}" :
                $"UPDATE {_masterTable} SET Name = '{name}' WHERE ID = {id}";
        }

        private string SelectAllFromWatchListTable(string watchListTableName)
        {
            return $"SELECT * FROM {watchListTableName};";
        }

        private string FindTableById(int id)
        {
            return $"SELECT TOP(1) FROM {_masterTable} WHERE ID = {id};";
        }

        private string CreateAWatchlistTableCmd(string tableName)
        {
            return $"CREATE TABLE {tableName}(ID INT PRIMARY KEY NOT NULL," +
                        $"TickerSymbol TEXT NOT NULL," +
                        $"Name TEXT NOT NULL," +
                        $"Description TEXT NULL," +
                        $"AverageCost REAL NOT NULL," +
                        $"SharePrice REAL NOT NULL," +
                        $"NumberOfShares INTEGER NOT NULL," +
                        $"PriceToEarningsRatio REAL NULL," +
                        $"DividendYield REAL NULL," +
                        $"MarketCap INTEGER NOT NULL," +
                        $"ExpenseRatio REAL NULL,"+
                        $"NumberOfHoldings INTEGER NULL," +
                        $"IsFund INTEGER NOT NULL" +
                        $");";
        }

        private string InsertIntoWatchListsTableCmd(string tableName,int numberOfHoldings = 0)
        {
            return $"INSERT INTO {_masterTable}( Name,NumberOfHoldings) VALUES({tableName},{numberOfHoldings}); ";
        }

        private string CreateTheMasterWatchListTableCmd()
        {
            return  $"CREATE TABLE {_masterTable}(" +
                    $"ID INT PRIMARY KEY NOT NULL," +
                    $"Name TEXT NOT NULL," +
                    $"NumberOfHoldings INTEGER NOT NULL"  +
                    $");";
        }

        private string DeleteAWatchListTableCmd(string tableName)
        {
            return $"DROP TABLE {tableName};";
        }
        #endregion
    }
}
