using Core.Utilities.Helpers.HttpClient;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Initialize.Data.JsonApi;
using Domain.Entities.Addresses;
using NetTopologySuite.Geometries;

namespace Data.Initialize.Data
{
    public static partial class InitializeData
    {
        
        public static async Task<List<User>> BuildUserList(ApplicationDbContext context)
        {
            var usersToSeed =await HttpClientHelper<List<UserJsonApiData>>.GetAsync("https://jsonplaceholder.typicode.com/users");


            var list = new List<User>()
            {
                new User { Id = Guid.NewGuid().ToString(), UserName = "admin", Email = "admin@gmail.com", DateCreated = DateTime.Now},
            };
            foreach (var user in usersToSeed)
            {
                list.Add(new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    Email = user.Email,
                    UserName = user.UserName,
                    Detail = new UserDetail()
                    {
                        Name = user.Name.Split(' ')[0],
                        SurName = user.Name.Split(' ')[1],
                        Phone = user.Phone,
                        Website = user.Website,
                        CompanyId = context.Companies.FirstOrDefault(x=>x.Name==user.Company.Name)?.Id,
                        Address = new Address()
                        {
                            City = user.Address.City,
                            Street = user.Address.Street,
                            Suite = user.Address.Suite,
                            ZipCode = user.Address.Zipcode,
                            Geo = new Point(Convert.ToDouble(user.Address.Geo.Lng) , Convert.ToDouble(user.Address.Geo.Lat)) { SRID = 4326 }
                        }
                    }
                });
            }
            return list;

        }

    }
}
