using JwtWithIdentity.Models.Entities.Common;

namespace JwtWithIdentity.Models.Entities.Concretes
{
    public class ToDoItem : IBaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;


        public string UserId { get; set; }


        public User User { get; set; }
    }
}
