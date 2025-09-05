using System;
using Microsoft.EntityFrameworkCore;
using BlogApp.Entity;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.Linq;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<BlogContext>();
            if (context == null) return;

            // Migrations varsa uygula
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            // Tags
            if (!context.Tags.Any())
            {
                var tags = new List<Tag>
                {
                    new Tag { TagName = "Web Development", TagUrl = "web-development", TagColor = TagColors.primary },
                    new Tag { TagName = "C#", TagUrl = "c-sharp", TagColor = TagColors.success },
                    new Tag { TagName = "PHP", TagUrl ="php", TagColor = TagColors.info },
                    new Tag { TagName = "Backend", TagUrl="backend", TagColor = TagColors.warning },
                    new Tag { TagName = "Frontend", TagUrl="frontend", TagColor = TagColors.danger }
                };
                context.Tags.AddRange(tags);
                context.SaveChanges();
            }

            // Users
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { UserName = "yusufk", Name = "Yusuf", Surname = "Kara", Email = "yusuf@example.com", Password = "123456", UserUrl="yusuf-kara" },
                    new User { UserName = "nisay", Name = "Nisa", Surname = "Yılmaz", Email = "nisa@example.com", Password = "123456", UserUrl="nisa-yilmaz" },
                    new User { UserName = "aliD", Name = "Ali", Surname = "Demir", Email = "ali@example.com", Password = "123456", UserUrl="ali-demir" },
                    new User { UserName = "veliC", Name = "Veli", Surname = "Çelik", Email = "veli@example.com", Password = "123456", UserUrl="veli-celik" }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            // Posts
            if (!context.Posts.Any())
            {
                var tagsList = context.Tags.Take(3).ToList();

                var posts = new List<Post>
                {
                    new Post
                    {
                        PostName = "Introduction to ASP.NET Core",
                        PostText = "Learn the basics of ASP.NET Core and build your first web application.",
                        PostIsActive = true,
                        PostPublishDate = DateTime.Now.AddDays(-10),
                        PostTags = tagsList,
                        UserId = 1,
                        PostImage = "aspnetcore.jpg",
                        PostUrl = "introduction-to-aspnet-core",
                        PostComments = new List<Comment>
                        {
                            new Comment { CommentText = "Great tutorial!", CommentDate = DateTime.Now.AddDays(-9), UserId = 2 },
                            new Comment { CommentText = "Very helpful, thanks!", CommentDate = DateTime.Now.AddDays(-8), UserId = 3 }
                        }
                    },
                    new Post
                    {
                        PostName = "PHP for Beginners",
                        PostText = "This post covers the basics of PHP programming for new developers.",
                        PostIsActive = true,
                        PostPublishDate = DateTime.Now.AddDays(-7),
                        PostTags = tagsList,
                        UserId = 2,
                        PostImage = "php.jpg",
                        PostUrl = "php-for-beginners",
                        PostComments = new List<Comment>
                        {
                            new Comment { CommentText = "Nice introduction.", CommentDate = DateTime.Now.AddDays(-6), UserId = 1 }
                        }
                    },
                    new Post
                    {
                        PostName = "Getting Started with Django",
                        PostText = "A beginner-friendly guide to creating web applications with Django.",
                        PostIsActive = true,
                        PostPublishDate = DateTime.Now.AddDays(-5),
                        PostTags = tagsList,
                        UserId = 3,
                        PostImage = "django.jpg",
                        PostUrl = "getting-started-with-django",
                        PostComments = new List<Comment>
                        {
                            new Comment { CommentText = "Thanks for the guide!", CommentDate = DateTime.Now.AddDays(-4), UserId = 2 }
                        }
                    }
                };

                context.Posts.AddRange(posts);
                context.SaveChanges();
            }
        }
    }
}
