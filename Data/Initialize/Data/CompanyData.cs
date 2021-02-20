using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Helpers.HttpClient;
using Data.Initialize.Data.JsonApi;
using Domain.Entities.Companies;

namespace Data.Initialize.Data
{
    public static partial class InitializeData
    {

        public static async Task<List<Company>> BuildCompanyList()
        {
            var companiesToSeed = await HttpClientHelper<List<UserJsonApiData>>.GetAsync("https://jsonplaceholder.typicode.com/users");
            var list = new List<Company>();
            foreach (var company in companiesToSeed.Select(x=>x.Company))
            {
                list.Add(new Company()
                {
                    Name = company.Name,
                    BusinessSegment = company.Bs,
                    CatchPhrase = company.CatchPhrase
                });
            }
            
            return list;
        }
    }

}
