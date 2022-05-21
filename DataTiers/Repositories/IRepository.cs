using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DataTiers.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(Guid id);
        void Add(T t);
        void Update(T t);
        void Delete(Guid id);
        void Save();
       // PagedList<T> GetAll(dynamic parameters);
        //string GetLoggedInUser();
        //string GetLoggedInEmail();
        //List<T> GetAll(dynamic parameters);
    }
}
