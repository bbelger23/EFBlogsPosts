using System.Collections.Generic;
namespace EFBlogsPosts
{
        public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        public List<Post> Posts { get; set; }
    }
}