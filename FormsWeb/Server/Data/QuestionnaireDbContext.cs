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
        }

        public DbSet<Questionnaire> QuestionSets { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<ResponseSet> ResponseSets { get; set; }
        public DbSet<Response> Responses { get; set; }

    }
    
}
