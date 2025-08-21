using System;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore;

public static class SeedData
{
    public static void TestVerileriniDoldur(IApplicationBuilder app)
    {
        var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();
        if (context != null)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Tags.Any())
            {
                context.Tags.AddRange(
                    new Entity.Tag { TagName = "web programlama",
                        TagUrl = "web-programlama" },
                    new Entity.Tag { TagName = "c programlama", TagUrl = "c-programlama" },

                    new Entity.Tag { TagName = "php programlama", TagUrl ="php-programlama" },

                    new Entity.Tag { TagName = "backend programlama", TagUrl="backend-programlama" }

                );
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new Entity.User { UserName = "Yusa" },
                    new Entity.User { UserName = "nisa" },
                    new Entity.User { UserName = "ali" },
                    new Entity.User { UserName = "veli" }
                    );
                context.SaveChanges();

            }
            if (!context.Posts.Any())
            {
                context.Posts.AddRange(
                    new Entity.Post{PostName="AspNetcore",PostText="AspNetCorePostTText",PostIsActive=true,PostPublishDate=DateTime.Now,PostTags=context.Tags.Take(3).ToList(),UserId=1,PostImage="1.jpg" , PostUrl="aspnetcore0"},
                    new Entity.Post{PostName="php programlama",PostText="PhpProgramlamaPostText",PostIsActive=true,PostPublishDate=DateTime.Now,PostTags=context.Tags.Take(3).ToList(),UserId=1,PostImage="1.jpg" , PostUrl="php-programlama"},
                    new Entity.Post{PostName="Django programlama",PostText="DjangoProgramlamaPostText",PostIsActive=true,PostPublishDate=DateTime.Now,PostTags=context.Tags.Take(3).ToList(),UserId=1,PostImage="1.jpg" , PostUrl="django-programlama"}
                );
                
                context.SaveChanges();



            }
        }
    }




}
