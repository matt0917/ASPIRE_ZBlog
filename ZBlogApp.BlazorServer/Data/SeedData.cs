using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZBlogApp.BlazorServer.Data;
using ZBlogApp.Library;


namespace SeedIdentity.Data;

public static class SeedData {
    public static async Task Initialize(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager) {
        await context.Database.EnsureCreatedAsync(); // ensure database is created

        string adminRole = "admin";
        string memberRole = "contributor";
        string password4all = "P@$$w0rd";

        if (await roleManager.FindByNameAsync(adminRole) == null) 
        {
            ApplicationRole admin = new ApplicationRole(adminRole);
            admin.Description = "Administrator";
            await roleManager.CreateAsync(admin);
        }

        if (await roleManager.FindByNameAsync(memberRole) == null) 
        {
            ApplicationRole member = new ApplicationRole(memberRole);
            member.Description = "Contributor : can create articles, edit, delete own articles, update account";
            await roleManager.CreateAsync(member);
        }

        if (await userManager.FindByNameAsync("a@a.a") == null){
            var user = new ApplicationUser {
                UserName = "a@a.a",
                Email = "a@a.a",
                FirstName = "James",
                LastName = "Smith",
                PhoneNumber = "6902341234"
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded) {
                await userManager.AddPasswordAsync(user, password4all);
                await userManager.AddToRoleAsync(user, adminRole);
            }
        }

        if (await userManager.FindByNameAsync("c@c.c") == null) {
            var user = new ApplicationUser {
                UserName = "c@c.c",
                Email = "c@c.c",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "6041234567"
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded) {
                await userManager.AddPasswordAsync(user, password4all);
                await userManager.AddToRoleAsync(user, memberRole);
            }
        }

        if (!await context.Articles.AnyAsync())
        {
            var articles = new List<Article>
            {
                new Article
                {
                    Title = "Climate Crisis Update",
                    Body = "“What the Latest IPCC Report Means for the World,” breaking down key findings from recent climate reports and discussing actionable steps. Another example could be “Space Exploration Milestone: NASA’s Europa Probe Set for Launch,” covering exciting developments in space exploration.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    ContributorUserName = "James.Smith"
                },
                new Article
                {
                    Title = "New Tech Gadgets",
                    Body = "“The Best New Tech Gadgets of 2022,” highlighting the latest and greatest in technology, from smartphones to smart home devices. Another example could be “How AI is Revolutionizing Healthcare,” exploring the impact of artificial intelligence on the medical field.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    ContributorUserName = "John.Doe"
                },
                new Article
                {
                    Title = "Healthy Living Tips",
                    Body = "“10 Healthy Living Tips for a Happier Life,” offering practical advice on nutrition, exercise, and mental well-being. Another example could be “The Benefits of Mindfulness Meditation,” explaining the science behind mindfulness and its positive effects on mental health.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(60),
                    ContributorUserName = "James.Smith"
                },
                new Article
                {
                    Title = "Travel Destinations",
                    Body = "“The Top Travel Destinations of 2022,” featuring must-visit locations around the world and travel tips for each destination. Another example could be “How to Plan the Perfect Road Trip,” with advice on route planning, packing essentials, and safety tips.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(60),
                    ContributorUserName = "John.Doe"
                }
            };

            await context.Articles.AddRangeAsync(articles);
            await context.SaveChangesAsync();
        }
    }
}