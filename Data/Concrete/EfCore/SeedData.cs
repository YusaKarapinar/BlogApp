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
                        TagUrl = "web-programlama" , TagColor = Entity.TagColors.primary },
                    new Entity.Tag { TagName = "c programlama", TagUrl = "c-programlama" ,TagColor = Entity.TagColors.success },

                    new Entity.Tag { TagName = "php programlama", TagUrl ="php-programlama" , TagColor = Entity.TagColors.info },

                    new Entity.Tag { TagName = "backend programlama", TagUrl="backend-programlama" , TagColor = Entity.TagColors.warning }

                );
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new Entity.User { UserName = "Yusa",UserImage="1.jpg", Name="Yusuf",Surname="Kara",Email="nisa5403@gmail.com", Password="123456", UserUrl="yusuf-kara"},
                    new Entity.User { UserName = "nisa",UserImage="2.jpg", Name="Nisa",Surname="Yılmaz",Email="nisa5403@gmail.com", Password="123456", UserUrl="nisa-yilmaz" },
                    new Entity.User { UserName = "ali",UserImage="1.jpg", Name="Ali",Surname="Demir",Email="ali@gmail.com", Password="123456", UserUrl="ali-demir" },
                    new Entity.User { UserName = "veli",UserImage="2.jpg", Name="Veli",Surname="Çelik",Email="veli@gmail.com", Password="123456", UserUrl="veli-celik" }
                    );
                context.SaveChanges();

            }
            if (!context.Posts.Any())
            {
                context.Posts.AddRange(
                    new Entity.Post{PostName="AspNetcore",PostText="AspNetCorePostTText",PostIsActive=true,PostPublishDate=DateTime.Now,PostTags=context.Tags.Take(3).ToList(),UserId=1,PostImage="1.jpg" , PostUrl="aspnetcore0",PostComments= new List<Entity.Comment>{ new Entity.Comment{CommentText="aspnetcore comment 1",CommentDate=DateTime.Now,UserId=2},new Entity.Comment{CommentText="aspnetcore comment 2",CommentDate=DateTime.Now,UserId=3} }},

                    new Entity.Post { PostName = "php programlama", PostText = "PhpProgramlamaPostText", PostIsActive = true, PostPublishDate = DateTime.Now, PostTags = context.Tags.Take(3).ToList(), UserId = 1, PostImage = "1.jpg", PostUrl = "php-programlama", PostComments = new List<Entity.Comment> { new Entity.Comment { CommentText = "php programlama comment 1", CommentDate = DateTime.Now, UserId = 2 } } },
                    

                    new Entity.Post { PostName = "Django programlama", PostText = "DjangoProgramlamaPostText", PostIsActive = true, PostPublishDate = DateTime.Now, PostTags = context.Tags.Take(3).ToList(), UserId = 1, PostImage = "1.jpg", PostUrl = "django-programlama", PostComments = new List<Entity.Comment> { new Entity.Comment { CommentText = "django programlama comment 1", CommentDate = DateTime.Now, UserId = 2 } } }
                );
                
                context.SaveChanges();



            }
        }
    }




}
