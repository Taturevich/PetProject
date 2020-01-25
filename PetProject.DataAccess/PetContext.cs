using Microsoft.EntityFrameworkCore;
using PetProject.Domain;

namespace PetProject.DataAccess
{
    public class PetContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public DbSet<TaskType> TaskTypes { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public PetContext(DbContextOptions<PetContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasOne<TaskType>(task => task.TaskType)
                .WithMany(taskType => taskType.Tasks)
                .HasForeignKey(task => task.TaskTypeId);
        }
    }
}
