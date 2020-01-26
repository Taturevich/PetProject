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

        public DbSet<PetTaskTypeAssignment> PetTaskTypeAssignments { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserFeatureAssignment> UserFeatureAssignments { get; set; }

        public DbSet<UserFeature> UserFeatures { get; set; }

        public DbSet<UserSocialNetwork> UserSocialNetworks { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Adopt> Adopts { get; set; }
        
        public PetContext(DbContextOptions<PetContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<PetTaskTypeAssignment>()
                .HasOne(petTaskAssignment => petTaskAssignment.Pet)
                .WithMany(pet => pet.PetTaskTypeAssignments)
                .HasForeignKey(petTaskAssignment => petTaskAssignment.PetId);

            modelBuilder.Entity<PetTaskTypeAssignment>()
                .HasOne(petTaskAssignment => petTaskAssignment.TaskType)
                .WithMany(taskType => taskType.PetTaskTypeAssignments)
                .HasForeignKey(petTaskAssignment => petTaskAssignment.TaskTypeId);

            modelBuilder.Entity<UserFeatureAssignment>()
                .HasOne(userFeatureAssignment => userFeatureAssignment.User)
                .WithMany(user => user.UserFeatureAssignments)
                .HasForeignKey(userFeatureAssignment => userFeatureAssignment.UserId);

            modelBuilder.Entity<UserFeatureAssignment>()
                .HasOne(userFeatureAssignment => userFeatureAssignment.UserFeature)
                .WithMany(userFeature => userFeature.UserFeatureAssignments)
                .HasForeignKey(userFeatureAssignment => userFeatureAssignment.UserFeatureId);

            modelBuilder.Entity<UserSocialNetwork>()
                .HasOne(userSocialNetwork => userSocialNetwork.User)
                .WithMany(user => user.UserSocialNetworks)
                .HasForeignKey(userSocialNetwork => userSocialNetwork.UserId);

            modelBuilder.Entity<Task>()
                .HasOne(task => task.TaskType)
                .WithMany(taskType => taskType.Tasks)
                .HasForeignKey(task => task.TaskTypeId);

            modelBuilder.Entity<Task>()
                .HasOne(task => task.User)
                .WithMany(user => user.Tasks)
                .HasForeignKey(task => task.UserId);

            modelBuilder.Entity<Task>()
                .HasOne(task => task.Pet)
                .WithMany(pet => pet.Tasks)
                .HasForeignKey(task => task.PetId);

            modelBuilder.Entity<Image>()
                .HasOne(image => image.Pet)
                .WithMany(pet => pet.Images)
                .HasForeignKey(image => image.PetId);
        }
    }
}
