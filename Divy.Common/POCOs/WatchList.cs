using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Divy.Common.POCOs
{
    public class WatchList : ObjectBase
    {
        private List<Share> _shares = new List<Share>();

        public string TableName { get; }

        public WatchList(string tableName)
        {
            TableName = tableName ?? string.Empty;
        }
    }
}
