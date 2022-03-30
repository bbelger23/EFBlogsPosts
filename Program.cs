using System;
using NLog.Web;
using System.IO;
using System.Linq;
namespace EFBlogsPosts
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string selection = "";
            var db = new BloggingContext();
            logger.Info("Program started");

            Console.WriteLine("Enter your selection:");
            Console.WriteLine("1. Display all blogs");
            Console.WriteLine("2. Add blog");
            Console.WriteLine("3. Create Post");
            Console.WriteLine("4. Display Posts");
            Console.WriteLine("Press q to quit");

            selection = Console.ReadLine();

            if (selection == "1") 
            {
                // Display all Blogs from the database
                
                var query = db.Blogs.OrderBy(b => b.Name);

                var total = db.Blogs.Count();
                Console.WriteLine($"{total} blogs returned");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }
            } 
            else if (selection == "2")
            {
                // Create and save a new Blog
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                if (name == "")
                {
                    logger.Error("Blog name cannot be null");
                }
                else 
                {
                    var blog = new Blog { Name = name };

                    db.AddBlog(blog);
                    logger.Info("Blog added - {name}", name);
                }
                
            }
            logger.Info("Program ended");
        }
    }
}
