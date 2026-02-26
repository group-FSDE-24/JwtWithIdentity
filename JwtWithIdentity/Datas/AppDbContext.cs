using Microsoft.EntityFrameworkCore;
using JwtWithIdentity.Models.Entities.Concretes;

namespace JwtWithIdentity.Datas;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ToDoItem>()
            .HasOne(x => x.User)
            .WithMany(x => x.ToDoItems)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }



    // DBset

    public DbSet<ToDoItem> ToDoItems { get; set; }
}
