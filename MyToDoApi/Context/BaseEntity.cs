using System.ComponentModel.DataAnnotations;

namespace MyToDoApi.Context
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
