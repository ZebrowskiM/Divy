using Divy.Common.POCOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Divy.DAL.Interfaces
{
    /// <summary>
    /// Manages the the watchList tables
    /// </summary>
    public interface IWatchListAdapter
    {
        /// <summary>
        /// Creates a WatchList and Returns the ID of the watchlist
        /// </summary>
        /// <param name="watchList"></param>
        /// <returns>ID of the created watchList</returns>
        int  CreateWatchList(WatchList watchList);

        /// <summary>
        /// Gets the watchList by the WatchList Id
        /// </summary>
        /// <param name="watchListId"></param>
        /// <returns>WatchList at the given Id</returns>
        List<Object> GetWatchListById(int watchListId);

        /// <summary>
        /// Updates a given watch List at the given Id and returns it. If the tables does not exist, it will be created and returned. 
        /// </summary>
        /// <param name="watchListId"></param>
        /// <param name="watchList"></param>
        /// <returns></returns>
        List<object> UpdateWatchlistById(int watchListId, WatchList watchList);

        /// <summary>
        /// Deletes the watchList by the WatchListId
        /// </summary>
        /// <param name="watchListId"></param>
        void DeleteWatchListById(int watchListId);

    }
}
