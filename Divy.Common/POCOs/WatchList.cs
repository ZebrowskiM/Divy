using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Divy.Common.POCOs
{
    public class WatchList : ObjectBase
    {

        public List<Share> Shares { get; set; }

        public string Name { get; set; }

    }
}
