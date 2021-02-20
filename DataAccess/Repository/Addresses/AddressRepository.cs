using Data;
using Domain.Entities.Addresses;
namespace DataAccess.Repository.Addresses
{
	public class AddressRepository : Repository<Address>,IAddressRepository
	{
		public AddressRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}

























































































