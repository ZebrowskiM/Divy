using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using Divy.Common.POCOs;
using Divy.DAL.Interfaces;

namespace Divy.DAL.Sqlite
{
    public class SqliteWatchListAdapter : IWatchListAdapter
    {
        private readonly string _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Divy");
        private readonly string _dbName = "DivyBase.db";
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

        private bool CreateWatchlistTable(string tableName)
        {
            var query = $"CREATE TABLE {tableName}(ID INT PRIMARY KEY NOT NULL," +
                        $"TickerSymbol VARCHAR NOT NULL," +
                        $"Name VARCHAR NOT NULL," +
                        $"Description VARCHAR NULL," +
                        $"AverageCost REAL NOT NULL," +
                        $"SharePrice REAL NOT NULL," +
                        $"NumberOfShares INTEGER NOT NULL," +
                        $"PriceToEarningsRatio REAL NULL," +
                        $"DividendYield REAL NULL," +
                        $"MarketCap INTEGER NOT NULL" +
                        $");";

            return true;
        }

        private bool DeleteWatchListTable(string tableName)
        {
            var query = $"DROP TABLE {tableName};";

            return true;
        }
    }
}
