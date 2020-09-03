using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Divy.Common.POCOs;

namespace Divy.DAL.Sqlite
{
    public class SqliteDataConverter
    {
        public List<Share> ConvertObjectsIntoShares(List<Object> objects)
        {
            if(objects == null)
                throw new ArgumentException(nameof(objects));
            var shares = new ConcurrentBag<Share>();
            var Errors = new ConcurrentBag<Share>();
            Parallel.ForEach(objects, obj =>
            {
                
            });
            return new List<Share>();

        }
    }
}
