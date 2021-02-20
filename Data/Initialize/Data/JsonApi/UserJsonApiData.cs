using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Initialize.Data.JsonApi
{
    public class UserJsonApiData
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public CompanyJsonApiData Company { get; set; }
        public AddressJsonApiData Address { get; set; }

    }
}
