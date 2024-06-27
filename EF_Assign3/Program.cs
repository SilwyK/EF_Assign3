using Core.Entities;
using EF_Assign3.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EF_Assign3
{
    public class Program
    {
        static void Main(string[] args)
        {

            //var name = "khristis@mymacewan.ca";

            //var dataCreator = new DataCreator(name);
            //var container = dataCreator.GetData();
            //Console.ReadLine();

            var context = new DataContext();
            if (context.Users.Count() == 0)
            {
                PopulateData(context);
            }

            var outcomes = context.Blogs
               .Include(blog => blog.BlogType)
               .Include(blog => blog.Posts)
                   .ThenInclude(post => post.PostType)
               .Include(blog => blog.Posts)
                   .ThenInclude(post => post.User)
               .Where(blog => !blog.IsDeleted && blog.BlogType.Status == Statuses.Active && !string.IsNullOrEmpty(blog.Url))
               .SelectMany(blog => blog.Posts.Where(post => !post.IsDeleted && post.PostType.Status == Statuses.Active),
                          (blog, post) => new
                          {
                              BlogUrl = blog.Url,
                              BlogIsPublic = blog.IsPublic,
                              BlogTypeName = blog.BlogType.Name,
                              PostUserName = post.User.Name,
                              PostUserEmailAddress = post.User.EmailAddress,
                              TotalBlogPostsByUser = post.User.Posts.Count,
                              PostTypeName = post.PostType.Name
                          })
               .OrderBy(outcomes => outcomes.PostUserName)
               .ToList();

            foreach (var item in outcomes)
            {
                Console.WriteLine($"Blog URL: {item.BlogUrl}");
                Console.WriteLine($"Blog IsPublic: {item.BlogIsPublic}");
                Console.WriteLine($"Blog Type Name: {item.BlogTypeName}");
                Console.WriteLine($"Post User Name: {item.PostUserName}");
                Console.WriteLine($"Post User Email Address: {item.PostUserEmailAddress}");
                Console.WriteLine($"Total Blog Posts by User: {item.TotalBlogPostsByUser}");
                Console.WriteLine($"Post Type Name: {item.PostTypeName}");
            }
        }
        private static void PopulateData(DataContext context)
        {
            var creator = new DataCreator("khristis@mymacewan.ca");
            var container = creator.GetData();
            context.Users.AddRange(container.Users);
            context.PostTypes.AddRange(container.PostTypes);
            context.Posts.AddRange(container.Posts);
            context.BlogTypes.AddRange(container.BlogTypes);
            context.Blogs.AddRange(container.Blogs);
            context.SaveChanges();
        }
    }
}
