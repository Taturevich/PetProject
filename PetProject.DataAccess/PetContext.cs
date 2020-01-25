﻿using Microsoft.EntityFrameworkCore;
using PetProject.Domain;

namespace PetProject.DataAccess
{
    public class PetContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public DbSet<TaskType> TaskTypes { get; set; }

        public PetContext(DbContextOptions<PetContext> options)
            : base(options)
        {
        }
    }
}
