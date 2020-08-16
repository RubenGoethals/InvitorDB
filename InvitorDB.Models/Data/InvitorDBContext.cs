using InvitorDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvitorDB.Data
{
    public class InvitorDBContext : IdentityDbContext<Person, Role, string, IdentityUserClaim<string>, PersonRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public InvitorDBContext(DbContextOptions<InvitorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<PersonsEvents> PersonsEvents { get; set; }

        public virtual DbSet<EvaluationForms> EvaluationForms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //must voor identity

            // tussentabel UserRole: KEYS: 
            modelBuilder.Entity<PersonRole>(entity => 
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                //entity.HasKey(e => new { e.UserId, e.RoleId, e.EducationId }); 
                
                //tussentabel UserRole: RELATIES: 
                //Gezien de aangepaste tussentabellen(!) is het NOODZAKELIJK om de PK/FK 
                // instructies aan te maken voor SQL SERVER. Zoniet gebruikt/maakt het 
                // EF zijn eigen relaties aan en krijg je dubbel aangemaakte properties 
                entity.HasOne(d => d.Role).WithMany(p => p.PersonRoles).HasForeignKey(d => d.RoleId);
                entity.HasOne(d => d.User).WithMany(p => p.PersonRoles).HasForeignKey(d => d.UserId); 
            });

            //composite key
                modelBuilder.Entity<PersonsEvents>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.EventId });

            });

            modelBuilder.Entity<EvaluationForms>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.EventId });

            });

            modelBuilder.Entity<Person>().HasMany(p => p.PersonRoles).WithOne().HasForeignKey(p => p.UserId).IsRequired();

            modelBuilder.Entity<Person>().Ignore(t => t.ImgUrl);

            //uniek maken           
            //modelBuilder.Entity<Event>().HasIndex(e => e.Name).IsUnique();

            modelBuilder.Entity<Event>().Property(e => e.Id).UseIdentityColumn();
        }


    }
}
