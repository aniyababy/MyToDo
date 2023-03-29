using Microsoft.EntityFrameworkCore;

using MyToDoApi.Context.UnitOfWork;

namespace MyToDoApi.Context.Repository
{
    public class MemoRepository : Repository<Memo>,IRepository<Memo>
    {
        public MemoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
