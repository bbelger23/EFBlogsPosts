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
                logger.Info("Option \"1\" selected");
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
                logger.Info("Option \"2\" selected");
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
            else if (selection == "3")
            {
                logger.Info("Option \"3\" selected");
                // Create and save new Post
                Console.WriteLine("Select the blog you would like to post to");

                // Display all Blogs for user to chose from
                foreach (var b in db.Blogs)
                {
                    Console.WriteLine($"{b.BlogId}. {b.Name}");
                }

                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter the Post title");

                    var title = Console.ReadLine();

                    if (title == "")
                    {
                        logger.Error("Post title cannot be null");
                    }
                    else
                    {
                        Console.WriteLine("Enter the Post content");

                        var content = Console.ReadLine();

                        var post = new Post();

                        post.Title = title;
                        post.Content = content;
                        post.BlogId = option;

                        db.Posts.Add(post);
                        db.SaveChanges();

                        logger.Info($"Post added - {title}");
                    }
                }
                catch
                {
                    logger.Error("Invalid Blog ID");
                }
            }
            else if (selection == "4")
            {
                logger.Info("Option \"4\" selected");
                // Display posts
                Console.WriteLine("Select the bolg's posts to display");
                Console.WriteLine("0. Posts from all blogs");
                foreach (var b in db.Blogs)
                {
                    Console.WriteLine($"{b.BlogId}. Posts from {b.Name}");
                }
                
                int choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 0)
                {
                    var blogPost = db.Blogs
                                    .Join(
                                        db.Posts,
                                        blog => blog.BlogId,
                                        post => post.BlogId,
                                        (blog , Post) => new
                                        {
                                            BlogName = blog.Name,
                                            BlogId = blog.BlogId,
                                            BlogPost = Post.Title,
                                            BlogContent = Post.Content
                                        }
                                    ).ToList();
                                    var total = blogPost.Count();
                                    Console.WriteLine($"{total} post(s) returned");
                                    foreach(var blog in blogPost)
                                    {
				                        Console.WriteLine("Blog Name: {0} \n Post Title: {1} \n Post Content: {2} \n", blog.BlogName, blog.BlogPost, blog.BlogContent);
                                    }
                } else if (choice != 0)
                {
                    
                    var blogPost = db.Blogs
                                    .Where(b => b.BlogId == choice)
                                    .Join(
                                        db.Posts,
                                        blog => blog.BlogId,
                                        post => post.BlogId,
                                        (blog , Post) => new
                                        {
                                            BlogName = blog.Name,
                                            BlogId = blog.BlogId,
                                            BlogPost = Post.Title,
                                            BlogContent = Post.Content
                                        }

                                    ).ToList();
                                    var total = blogPost.Count();
                                    Console.WriteLine($"{total} post(s) returned");
                                    foreach(var blog in blogPost)
                                    {
				                        Console.WriteLine("Blog Name: {0} \n Post Title: {1} \n Post Content: {2} \n", blog.BlogName, blog.BlogPost, blog.BlogContent);
                                    }
                }
                
            }
            logger.Info("Program ended");
        }
    }
}
