using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Divy.Common.POCOs;

namespace Divy.DAL
{
    /// <summary>
    /// Takes Lists of objects and covert them into usable Shares
    /// </summary>
    public class DataConverter
    {
        public List<Share> ConvertObjectsIntoShares(List<Object> objects)
        {
            if(objects == null)
                throw new ArgumentException(nameof(objects));
            var shares = new ConcurrentBag<Share>();
            var Errors = new ConcurrentBag<Share>();
            Parallel.ForEach(objects, obj =>
            {
                //Maybe have the adapter pull the schema and then throw it into an object collection, then break it down here 
                // Also do a speed these here to see if i pulled like the entire s and p 500 how long it would take 
            });
            return new List<Share>();

        }
    }
}
