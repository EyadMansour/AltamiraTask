using Shared.Resources.Addresses;
using Shared.Resources.Companies;

namespace Shared.Resources.User
{
    public class UserDetailGetData: IGetData
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public AddressGetData Address { get; set; }=new AddressGetData();
        public CompanyGetData Company { get; set; }=new CompanyGetData();
    }
}
