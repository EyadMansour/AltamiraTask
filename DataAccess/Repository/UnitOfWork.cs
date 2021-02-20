using System.Threading.Tasks;
using Data;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;

        }
        public Task CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

    }
}
