using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using Divy.Common;
using Divy.Common.POCOs;
using Divy.DAL.Interfaces;

namespace Divy.DAL.Sqlite
{
    public class SqliteWatchListAdapter : IWatchListAdapter
    {
        private readonly string _folderPath;
        private readonly string _dbName = "DivyBase.db";
        private readonly string _masterTable = "WatchLists";
        private readonly string _connectionString;
        public SqliteWatchListAdapter(string filePath)
        {
            _folderPath =  !Directory.Exists(filePath)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Divy")
                : filePath;
            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);
            var path = Path.Combine(_folderPath, _dbName);
            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);
            _connectionString = $"DataSource={path};Version=3;";
        }

        public int CreateWatchList(WatchList watchList)
        {
            if (watchList == null)
                throw new ArgumentNullException(nameof(watchList), "Watch List cannot be null, cannot Create watch List");
            var id = -1;
            using (var conn = new SQLiteConnection(_connectionString))
            {
               var trans =  conn.BeginTransaction();
              
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    var getTableIdByName = GetTableIdByName(watchList.Name);
                    try
                    {
                        cmd.CommandText =
                            InsertIntoWatchListsTableCmd(watchList.Name, watchList.Shares.Count);
                        var resultMasterTable = cmd.ExecuteNonQuery();
                        cmd.CommandText = CreateAWatchlistTableCmd(watchList.Name);
                        var resultWatchListTable = cmd.ExecuteNonQuery();
                        watchList.Shares.ForEach(x => InsertShareIntoWatchList(x, watchList.Name));
                            cmd.CommandText = getTableIdByName;
                        id =  cmd.ExecuteReader().GetInt32(0);

                    }
                    catch (Exception ex)
                    {
                        Tracing.Error(ex);
                        Tracing.Error("Rolling Back Table Creation and cleaning up");
                        trans.Rollback();
                        throw;
                    }
                }
                trans.Commit();
            }
            return id;
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
                            result.GetInt64(8),//Market Cap 
                            result.GetFloat(9),//ExpenseRatio
                            result.GetInt32(10),//NumOfHoldings
                        };
                        objs.Add(innerObjList);
                        
                    }

                }
            }

            return objs;
        }

        public List<object> UpdateWatchlistById(int watchListId,WatchList watchList)
        {
            if (watchListId < 0)
                throw new ArgumentException($"{watchListId} cannot be less than 0, cannot update watch List");
            if(watchList == null)
                throw new ArgumentNullException(nameof(watchList),"Watch List cannot be null, cannot update watch List");
            if (GetWatchListById(watchListId) == null)
            {
                var createdListId =  CreateWatchList(watchList);
                return GetWatchListById(createdListId);
            }
            var sharesToBeUpdated = watchList.Shares.FindAll(x => GetShareId(x, watchList.Name) != null);
            var sharesToBeAdded = watchList.Shares.FindAll(x => !sharesToBeUpdated.Contains(x));
            sharesToBeAdded.ForEach(x => InsertShareIntoWatchList(x,watchList.Name));
            var stringBuilder = new StringBuilder();
            using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();
                var trans = con.BeginTransaction();
                try
                {
                    using (var cmd = new SQLiteCommand(con))
                    {
                        sharesToBeUpdated.ForEach(x =>
                        {
                            stringBuilder.Append("UPDATE " + watchList.Name + " Set ");
                            stringBuilder.Append(x);
                            stringBuilder.Append(" Where id = " + GetShareId(x, watchList.Name) + ";");
                            cmd.CommandText = stringBuilder.ToString();
                            var result = cmd.ExecuteNonQuery();
                        });
                     
                    }
                }
                catch (Exception ex)
                {
                    Tracing.Error(ex);
                    Tracing.Error("Failed to update table, rolling back any and all changes made.");
                    trans.Rollback();
                    throw;
                }
            }

            return GetWatchListById(watchListId);
        }

        public void DeleteWatchListById(int watchListId)
        {
            if(watchListId < 0)
                throw new ArgumentException("WatchList Id cannot be negative");
            var watchListTableName = GetTableNameById(watchListId);
            if (string.IsNullOrWhiteSpace(watchListTableName))
                throw new  Exception($"Could not Find watchList Name for watchlist with Id {watchListId}");
            using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();
                var trans = con.BeginTransaction();
                try
                {
                    using (var cmd = new SQLiteCommand(con))
                    {
                        cmd.CommandText = DeleteAWatchListTableCmd(watchListTableName);
                        var dropTable = cmd.ExecuteNonQuery();
                        cmd.CommandText = RemoveAWatchListFromMasterTable(watchListId);
                        var removeFromMaster = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Tracing.Error(ex);
                    Tracing.Error("Failed to Delete WatchList, rolling back any and all changes made.");
                    trans.Rollback();
                    throw;
                }
            }
        }

        #region SqlLiteConnection
        /// <summary>
        /// Get the Name of the watch list table using the id of the watchList
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetTableNameById(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var tableName = string.Empty;
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
        /// <summary>
        /// Inserts the given watch list table
        /// </summary>
        /// <param name="share"></param>
        /// <param name="watchListName"></param>
        private void InsertShareIntoWatchList(Share share,string watchListName)
        {
            if(share == null)
                throw new ArgumentNullException(nameof(share),"Cannot Insert a Null Share");
            if (string.IsNullOrWhiteSpace(watchListName))
                throw new ArgumentNullException(nameof(watchListName), "Cannot Insert a Share, watchListName is null");
            var props = string.IsNullOrWhiteSpace(share.GetPropNames()) 
                ? throw new Exception("Error reading Share properties, check logs") 
                : share.GetPropNames();
            var values = string.IsNullOrWhiteSpace(share.GetPropValues())
                ? throw new Exception("Share Cannot be empty, check logs")
                : share.GetPropValues(); 

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Insert INTO " + watchListName + "(");
            stringBuilder.Append(props);
            stringBuilder.Append(") VALUES (");
            stringBuilder.Append(values);
            stringBuilder.Append(");");

            using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();
                var trans = con.BeginTransaction();
                try
                {
                    using (var cmd = new SQLiteCommand(con))
                    {
                        cmd.CommandText = stringBuilder.ToString();
                        var result = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Tracing.Error(ex);
                    Tracing.Error("Failed to insert share into watchList, rolling back any and all changes made.");
                    trans.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// Pulls back a share 
        /// </summary>
        /// <param name="share"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private Share GetShareId(Share share,string tableName)
        {
            if (share == null) throw new ArgumentNullException(nameof(share),"Cannot Get a null Share");
            if(string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException(nameof(tableName), "TableName is required to query back the ShareId");
            Share outShare = null;
                using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();
                var trans = con.BeginTransaction();
                try
                {
                    using (var cmd = new SQLiteCommand(con))
                    {
                        cmd.CommandText = SelectShareByTicker(tableName,share.TickerSymbol,share.Name);
                        var result = cmd.ExecuteReader();
                        while (result.Read())
                        {
                            try
                            {
                                outShare = new Share();
                                try
                                {
                                    if (result.GetFloat(7) != null || result.GetInt32(10) != null)
                                    {
                                        outShare = new Fund
                                        {
                                            ExpenseRatio = result.GetFloat(9),
                                            NumberOfHoldings = result.GetInt32(10)
                                        };
                                    }
                                }
                                catch
                                {
                                } // Let empty to catch if Exp ratio and Num of Holdings Columns are null

                                outShare.TickerSymbol = result.GetString(1);
                                outShare.Name = result.GetString(2);
                                outShare.Description = result.GetString(3);
                                outShare.SharePrice = result.GetFloat(4);
                                outShare.NumberOfShares = result.GetInt32(5);
                                outShare.PriceToEarningsRatio = result.GetFloat(6);
                                outShare.Dividend = result.GetFloat(7);
                                outShare.MarketCap = result.GetInt64(8);
                            }
                            catch (Exception ex)
                            {
                                Tracing.Warning($"Potential Data Issue with {share?.Name} : ticker {share?.TickerSymbol}, See ex for more details ",ex);
                            } // left empty on purpose, Data Issues Ignore and return null
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tracing.Error(ex);
                    Tracing.Error("Failed to update table, rolling back any and all changes made.");
                    trans.Rollback();
                    throw;
                }
            }

            return outShare;
        }
        #endregion
        #region CmdStringCreationMethods
        private string SelectShareByTicker(string tableName, string ticker,string name)
        {
            return $"SELECT * FROM {tableName} WHERE TickerSymbol = {ticker} AND Name = {name};";
        }
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
            return $"SELECT * FROM {_masterTable} WHERE ID = {id} LIMIT 1 ;";
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
                        $"NumberOfHoldings INTEGER NULL" +
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

        #endregion
    }
}
