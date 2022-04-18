using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotService.Repository
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T obj);
        Task<T> FindAsync(string discription);
        Task<T> FindAsync(int discription);
        IEnumerable<T> GetAllAsync(/*Expression<Func<T, bool>> predicate = null*/);
        T FindById(Guid id);
        Task Edit(T obj);
    }
}
