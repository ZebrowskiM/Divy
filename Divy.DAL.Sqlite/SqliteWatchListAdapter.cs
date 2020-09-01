using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
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
            var tableName = "";
            if (string.IsNullOrWhiteSpace(watchList.TableName))
                tableName = $"_{Guid.NewGuid()}";
            else
                tableName = watchList.TableName;

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

        private bool UpdateAWatchListInMasterTable(int id, string Name, int NumberOfHoldings = 0)
        {
            //Update the row in the table with a new name and if NumberOfHoldings is not equal to zero add that too
            var query = $"";
            return true;
        }

        private bool CreateAWatchlistTable(string tableName)
        {
            var query = $"CREATE TABLE {tableName}(ID INT PRIMARY KEY NOT NULL," +
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

            return true;
        }

        private int InsertIntoWatchListsTable(string tableName,int NumberOfHoldings)
        {
            var query = $"INSERT INTO {_masterTable}( Name,NumberOfHoldings) VALUES({tableName},{NumberOfHoldings}); ";
            return 0;
        }

        private bool CreateTheMasterWatchListTable()
        {
            var query = $"CREATE TABLE {_masterTable}(" +
                        $"ID INT PRIMARY KEY NOT NULL," +
                        $"Name TEXT NOT NULL," +
                        $"NumberOfHoldings INTEGER NOT NULL"  +
                        $");";
            return true;
        }

        private bool DeleteAWatchListTable(string tableName)
        {
            var query = $"DROP TABLE {tableName};";

            return true;
        }
    }
}
