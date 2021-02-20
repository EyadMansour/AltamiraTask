using Data;
using Domain.Entities.Companies;
namespace DataAccess.Repository.Companies
{
	public class CompanyRepository : Repository<Company>,ICompanyRepository
	{
		public CompanyRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}

























































































