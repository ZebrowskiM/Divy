using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Divy.Common;
using Divy.Common.POCOs;
using Divy.DAL.Interfaces;
using Serilog;

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
            using (var conn = new SQLiteConnection(_connectionString))
            {
               var trans =  conn.BeginTransaction();
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    var resultMasterTable = 0; 
                    var resultWatchListTable = 0;
                    var getTableIdByName = GetTableIdByName(watchList.Name);
                    try
                    {
                        cmd.CommandText =
                            InsertIntoWatchListsTableCmd(watchList.Name, watchList._shares.Count);
                        resultMasterTable = cmd.ExecuteNonQuery();
                        cmd.CommandText = CreateAWatchlistTableCmd(watchList.Name);
                        resultWatchListTable = cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Tracing.Error(ex);
                        Tracing.Error("Rolling Back Table Creation and cleaning up");
                        trans.Rollback();
                    }
                }
                trans.Commit();

            }
            
            throw new NotImplementedException();
        }

        public List<object> GetWatchListById(int watchListId)
        {
            if(watchListId < 0) 
                throw new ArgumentOutOfRangeException(nameof(watchListId));
            var tableName = GetTableNameById(watchListId);
            var objs = new List<object>();
            using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = SelectAllFromWatchListTable(tableName);
                    var result = cmd.ExecuteReader();
                    while (result.Read())
                    {
                        var innerObjList = new List<object>
                        {
                            result.GetString(1),//ticker
                            result.GetString(2),//Name
                            result.GetString(3),//Desc
                            result.GetFloat(4),//SharePrice
                            result.GetInt32(5),//NumberOfShares
                            result.GetFloat(6),//PR Ratio
                            result.GetFloat(7),//Div Yield
                            result.GetInt16(8),//Market Cap 
                            result.GetFloat(9),//ExpenseRatio
                            result.GetInt32(10),//NumOfHoldings
                            result.GetBoolean(11)//isFund
                        };
                        objs.Add(innerObjList);
                        
                    }

                }
            }

            return objs;
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

        private string GetTableIdByName(string name)
        {
            return $"SELECT * FROM {_masterTable} WHERE Name = {name};";
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
                        $"Dividend REAL NULL," +
                        $"MarketCap INTEGER NOT NULL," +
                        $"ExpenseRatio REAL NULL,"+
                        $"NumberOfHoldings INTEGER NULL," +
                        $"IsFund INTEGER NOT NULL" +
                        $");";
        }

        private string InsertIntoWatchListsTableCmd(string tableName,int numberOfHoldings = 0)
        {
            return $"INSERT INTO {_masterTable}( Name,NumberOfHoldings) VALUES({numberOfHoldings},{tableName}); ";
        }

        private string CreateTheMasterWatchListTableCmd()
        {
            return  $"CREATE TABLE {_masterTable}(" +
                    $"ID INT PRIMARY KEY NOT NULL," +
                    $"NumberOfHoldings INTEGER NOT NULL," +
                    $"Name TEXT NOT NULL"  +
                    $");";
        }

        private string DeleteAWatchListTableCmd(string tableName)
        {
            return $"DROP TABLE {tableName};";
        }

        private string RemoveAWatchListFromMasterTable(int id)
        {
            return $"DELETE FROM {_masterTable} WHERE id = {id};";
        }

        private string CreateInsertStringsFromShares(WatchList watchList)
        {
            var cmd = new StringBuilder();
            watchList._shares.ForEach(x =>
            {
                cmd.Append("Insert INTO " + watchList.Name + "(");
                cmd.Append(x.GetPropNames());
                cmd.Append(") VALUES (");
                cmd.Append(x.GetPropValues());
                cmd.Append(");");
            });
            return cmd.ToString();
        }
        #endregion
    }
}
