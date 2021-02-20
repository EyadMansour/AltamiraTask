using Core.Utilities.Helpers.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Entities
{
    public interface IBaseService<GetData,CreateUpdateData,T>
    {
        public Task CreateAsync(CreateUpdateData createData);
        public Task<List<GetData>> ListAsync();
        public Task<GetData> GetAsync(T id);
        public Task<GetData> UpdateAsync(CreateUpdateData updateData);
        public Task DeleteAsync(T id);
       
    }
}
