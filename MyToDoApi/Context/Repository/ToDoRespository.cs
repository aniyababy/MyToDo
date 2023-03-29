using Microsoft.EntityFrameworkCore;

using MyToDoApi.Context.UnitOfWork;

namespace MyToDoApi.Context.Repository
{
    public class ToDoRepository : Repository<ToDo>,IRepository<ToDo>
    {
        public ToDoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
