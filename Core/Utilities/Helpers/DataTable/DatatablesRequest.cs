 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers.DataTable
{
    public class DataTablesRequest
    {
        public int Draw { get; set; }

        public List<Column> Columns { get; set; }

        public List<Order> Order { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public List<string> SearchColumns { get; set; }

        public Search Search { get; set; }
    }
}
