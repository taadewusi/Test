using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Test.DataTiers.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> table;
        private readonly IHttpContextAccessor _accessor;
        public Repository(ApplicationDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            table = _context.Set<T>();
            _accessor = accessor;
        }
        public void Add(T t)
        {
            table.Add(t);
        }

        public void Delete(Guid id)
        {
            T exists = table.Find(id);
            table.Remove(exists);
        }

        public List<T> GetAll()
        {
            return table.ToList();
        }

        //public PagedList<T> GetAll(dynamic parameters)
        //{
        //    return PagedList<T>.ToPagedList(table.ToList(), parameters);
        //}

        //public List<T> GetAll(dynamic parameters)
        //{
        //    return table.ToList()
        //}

        public T GetById(Guid id)
        {
            return table.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T t)
        {
            table.Attach(t);
            _context.Entry(t).State = EntityState.Modified;
        }

    }
}
