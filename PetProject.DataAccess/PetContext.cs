using Microsoft.EntityFrameworkCore;
using PetProject.Domain;

namespace PetProject.DataAccess
{
    public class PetContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public DbSet<TaskType> TaskTypes { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<PetStatus> PetStatuses { get; set; }

        public DbSet<PetFeature> PetFeatures { get; set; }

        public DbSet<PetFeatureAssignment> PetFeatureAssignments { get; set; }

        public PetContext(DbContextOptions<PetContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasOne(task => task.TaskType)
                .WithMany(taskType => taskType.Tasks)
                .HasForeignKey(task => task.TaskTypeId);

            modelBuilder.Entity<Pet>()
                .HasOne(pet => pet.PetStatus)
                .WithMany(petStatus => petStatus.Pets)
                .HasForeignKey(pet => pet.PetStatusId);

            modelBuilder.Entity<PetFeatureAssignment>()
                .HasOne(petFeatureAssignment => petFeatureAssignment.Pet)
                .WithMany(pet => pet.PetFeatureAssignments)
                .HasForeignKey(petFeatureAssignment => petFeatureAssignment.PetId);

            modelBuilder.Entity<PetFeatureAssignment>()
                .HasOne(petFeatureAssignment => petFeatureAssignment.PetFeature)
                .WithMany(petFeature => petFeature.PetFeatureAssignments)
                .HasForeignKey(petFeatureAssignment => petFeatureAssignment.PetFeatureId);
        }
    }
}
