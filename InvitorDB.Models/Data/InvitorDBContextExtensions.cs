using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace InvitorDB.Models.Data
{
    public static class InvitorDBContextExtensions
    {
        //private static List<Event> _events = new List<Event> {
        //        new Event {Id = 1,Code = "MCT", Name = "Media and Communication Technology" },
        //        new Event {Id =2,Code ="DEV", Name = "Digital Design and Development" },
        //        new Event {Id = 3, Code ="DAE", Name = "Digital Arts and Entertainment" }
        //};

        public async static Task SeedRoles(RoleManager<Role> RoleMgr)
        {
            IdentityResult roleResult;
            string[] roleNames = { "Admin", "Member", "SuperUser" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleMgr.RoleExistsAsync(roleName);
                if (!roleExist)
                {              
                    roleResult = await RoleMgr.CreateAsync(new Role(roleName));
                }
            }
        }

        public async static Task SeedUsers(UserManager<Person> userMgr)
        {
            //1. Admin aanmaken ---------------------------------------------------
            if (await userMgr.FindByNameAsync("Docent@MCT") == null)  //controleer de UserName
            {
                var user = new Person()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Docent@MCT",
                    Email = "docent@howest.be",
                };

                var userResult = await userMgr.CreateAsync(user, "Docent@1");
                var roleResult = await userMgr.AddToRoleAsync(user, "Admin");
                // var claimResult = await userMgr.AddClaimAsync(user, new Claim("DocentWeb", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
            //2. meerdere users  aanmaken --------------------------------------------
            //2a. persons met rol "Student" aanmaken
            //var nmbrStudents = 9;
            //for (var i = 1; i <= nmbrStudents; i++)
            //{
            if (await userMgr.FindByNameAsync("Student@MCT") == null)
            {
                Person student = new Person
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = ("Student@MCT"),
                    Email = "student@howest.be",
                    Gender = (Person.GenderType)new Random().Next(0, 2), //GenderType.Male
                };

                var userResult = await userMgr.CreateAsync(student, "Student@1");
                var roleResult = await userMgr.AddToRoleAsync(student, "Member");
                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build " + student.UserName);
                }
            }
            //}
            //2b. persons met rol "Docent" aanmaken
            //var nmbrTeachers = nmbrStudents / 3;
            //for (var i = 1; i <= nmbrTeachers; i++)
            //{
            //    if (await userMgr.FindByNameAsync("Docent@" + i) == null)
            //    {
            //        Person teacher = new Person
            //        {
            //            Id = Guid.NewGuid().ToString(),
            //            UserName = "Docent@" + i,
            //            Name = "nameDocent" + i,
            //            Email = "emailDocent" + i + "@howest.be",
            //            Gender = (Person.GenderType)new Random().Next(0, 2), //GenderType.Male
            //        };

            //        var userResult = await userMgr.CreateAsync(teacher, "Docent@" + i);
            //        var roleResult = await userMgr.AddToRoleAsync(teacher, "Teacher");
            //        if (!userResult.Succeeded || !roleResult.Succeeded)
            //        {
            //            throw new InvalidOperationException("Failed to build " + teacher.UserName);
            //        }
            //    }
            //}
        }

        //public async static Task SeedData(this InvitorDBContext context)
        //{

        //    //1. Educations aanvullen met hardcoded data -----------------------------------
        //    // Id is van het type  [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //    if (!context.Event.Any())
        //    {
        //        //await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Educations] ON");
        //        //await context.SaveChangesAsync();

        //        Debug.WriteLine("Seeding Educations");
        //        foreach (Event e in _educations)
        //        {
        //            if (!context.Educations.Any(edu => edu.Id == e.Id))
        //                await context.Educations.AddAsync(e);
        //        }
        //        await context.SaveChangesAsync();
        //    }


        //    //2. EducationsPersons aanvullen met random data ------------------------------
        //    // Id is van het type  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //    if (!context.PersonsEducations.Any())
        //    {
        //        Debug.WriteLine("Seeding PersonsEducations");
        //        for (int i = 0; i <= 15; i++)
        //        {
        //            //1. Random education en random person ophalen voor tussentabel
        //            var randomEducation = await (context.Educations.OrderBy(e => Guid.NewGuid())).FirstAsync();
        //            var randomPerson = await (context.Persons.OrderBy(e => Guid.NewGuid())).FirstAsync();

        //            PersonsEducations personEducation = new PersonsEducations
        //            {

        //                EducationId = randomEducation.Id,
        //                PersonId = randomPerson.Id
        //            };

        //            //2. aangemaakte personseducations aanmaken indien onbestaand.
        //            if (!context.PersonsEducations.Any(pe => pe.EducationId == personEducation.EducationId))
        //                await context.PersonsEducations.AddAsync(personEducation);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //}
    }
}
