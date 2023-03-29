using Microsoft.EntityFrameworkCore;

using MyToDoApi.Context.UnitOfWork;

namespace MyToDoApi.Context.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
