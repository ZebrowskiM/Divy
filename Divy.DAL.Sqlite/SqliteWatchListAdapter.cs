using System;
using System.Data.SQLite;
using System.IO;
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

        public WatchList GetWatchListById(int watchListId)
        {
            throw new NotImplementedException();
        }

        public WatchList UpdateWatchlistById(int watchListId)
        {
            throw new NotImplementedException();
        }

        public void DeleteWatchListById(int watchListId)
        {
            throw new NotImplementedException();
        }

        #region SqlLiteConnection

        private 

        #endregion

        #region CmdStringCreationMethods
        private string UpdateAWatchListInMasterTableCmd(int id, string name, int numberOfHoldings = 0)
        {
            return numberOfHoldings != 0 ?
                $"UPDATE {_masterTable} SET Name = '{name}', NumberOfHoldings = {numberOfHoldings} WHERE ID = {id}" :
                $"UPDATE {_masterTable} SET Name = '{name}' WHERE ID = {id}";
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
                        $"MarketCap INTEGER NOT NULL" +
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
