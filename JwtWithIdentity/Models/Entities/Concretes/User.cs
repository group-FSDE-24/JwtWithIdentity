using JwtWithIdentity.Models.Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace JwtWithIdentity.Models.Entities.Concretes
{
    public class User : IdentityUser, IBaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }


        public ICollection<ToDoItem> ToDoItems { get; set; }
}
}
