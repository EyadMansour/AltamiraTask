using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers.DataTable
{
    public class DataTableResponse<T>
    {
        public DataTableResponse()
        {
            Data=new List<T>();
        }
        public int Draw { get; set; }
        public long RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; }
    }
}