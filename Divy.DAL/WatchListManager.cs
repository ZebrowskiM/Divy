using Divy.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Divy.Common.POCOs;

namespace Divy.DAL
{
    public class WatchListManager : IWatchListManager
    {
        private readonly IWatchListAdapter _watchListAdapter;
        public WatchListManager(IWatchListAdapter adapter)
        {
            _watchListAdapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }

        public List<WatchList> GetAllWatchLists()
        {
            throw new NotImplementedException();
        }

        public WatchList GetWatchListById(int id)
        {
           var rawObjects =  _watchListAdapter.GetWatchListById(id);
            return null;
        }

    }
}
