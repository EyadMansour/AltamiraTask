using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
