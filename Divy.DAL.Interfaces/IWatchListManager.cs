using System;
using System.Collections.Generic;
using System.Text;
using Divy.Common.POCOs;

namespace Divy.DAL.Interfaces
{
    public interface IWatchListManager
    {
        /// <summary>
        /// Returns all watch lists from the database
        /// </summary>
        /// <returns>List of WatchLists in the system</returns>
        List<WatchList> GetAllWatchLists();

        /// <summary>
        /// Gets a specific WatchList Based on an Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Watch List</returns>
        WatchList GetWatchListById(int id);


    }
}
