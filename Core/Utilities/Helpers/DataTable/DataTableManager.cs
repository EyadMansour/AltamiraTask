using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers.DataTable
{
    public class DataTableManager<T>
    {
        private static bool isAClass<X>(X value)
        {
            return typeof(X).IsClass;
        }
        private static bool CheckWordInColumn<Z>(Z record, string columnName, string searchText)
        {
            var recordType = record.GetType();
            if (recordType.IsGenericType)
            {
                return false;
            }
            var recordValue = recordType.GetProperty(columnName)?.GetValue(record);
            if (recordValue != null)
            {
                if (recordValue.ToString().ToUpper().Contains(searchText))
                {
                    return recordValue.ToString().ToUpper().Contains(searchText);
                }
                else if (!recordValue.GetType().Namespace.StartsWith("System") && isAClass(recordValue))
                {
                    foreach (var item in recordValue.GetType().GetProperties())
                    {
                        if (CheckWordInColumn(recordValue, item.Name, searchText))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public static DataTableResponse<T> CreateTable(List<T> objectList, DataTablesRequest dataTablesRequest,
            List<string> searchColumns = null)
        {
            var recordsTotal = objectList.Count;
            var objectsQuery = objectList.AsQueryable();
            var tempQuer = objectsQuery;
            var searchText = dataTablesRequest.Search.Value?.ToUpper();

            var tempObjectsQuery = objectsQuery;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                if (searchColumns == null)
                {
                    searchColumns = new List<string>();
                    foreach (var property in typeof(T).GetProperties())
                    {
                        searchColumns.Add(property.Name);
                    }
                }

                List<T> tempList = new List<T>();

                foreach (var item in objectsQuery)
                {
                    foreach (var columnName in searchColumns)
                    {
                        if (CheckWordInColumn(item, columnName, searchText))
                        {
                            tempList.Add(item);
                            break;
                        }

                    }
                }
                objectsQuery = tempList.AsQueryable();
            }

            var recordsFiltered = objectsQuery.Count();
            var sortColumnName = dataTablesRequest.Columns.ElementAt(dataTablesRequest.Order.ElementAt(0).Column).Name;
            var sortDirection = dataTablesRequest.Order.ElementAt(0).Dir.ToLower();
            if (sortDirection == "desc")
            {
                objectsQuery = objectsQuery.OrderByDescending(x => x.GetType().GetProperty(sortColumnName));
            }
            else
            {
                objectsQuery = objectsQuery.OrderBy(x => x.GetType().GetProperty(sortColumnName));
            }

            var data = objectsQuery
                .Skip(dataTablesRequest.Start)
                .Take(dataTablesRequest.Length)
                .ToList();
            DataTableResponse<T> response = new DataTableResponse<T>()
            {
                Draw = dataTablesRequest.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
            return response;
        }
    }
}
