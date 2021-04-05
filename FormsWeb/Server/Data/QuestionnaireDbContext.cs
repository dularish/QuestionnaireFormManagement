using FormsWeb.Server.Models;
using FormsWeb.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsWeb.Server.Data
{
    public class QuestionnaireDbContext : DbContext
    {
        public QuestionnaireDbContext(DbContextOptions<QuestionnaireDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Response>()
                .HasOne(s => s.Question)
                .WithMany(s => s.Responses)
                .HasForeignKey(s => s.QuestionId)
                .OnDelete(DeleteBehavior.ClientCascade);//Learning: For two cascade delete foreign key relationships to one table, 
                                                        //it is necessary to one of them as Client cascade so that the database could
                                                        //be setup
        }

        public DbSet<Questionnaire> QuestionSets { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<ResponseSet> ResponseSets { get; set; }
        public DbSet<Response> Responses { get; set; }

    }
    
}
