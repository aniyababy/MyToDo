using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyToDoApi.Context
{
    public class MyToDoContext:DbContext
    {
        public MyToDoContext(DbContextOptions<MyToDoContext> options):base(options) 
        {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<Memo> Memo { get; set; }
        public DbSet<ToDo> ToDo { get; set; }
        

    }
}
