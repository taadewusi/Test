using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Models.Books;
using Test.Models.Category;

namespace Test.DataTiers
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }


        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory>  Categories { get; set; }
      
    }
}
