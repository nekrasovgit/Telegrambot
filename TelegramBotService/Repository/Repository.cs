using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotService.Models;

namespace TelegramBotService.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TelegramContext _telegramContext;
        private readonly DbSet<T> _dbSet;

        public Repository(TelegramContext telegramContext)
        {
            _telegramContext = telegramContext;
            _dbSet = _telegramContext.Set<T>();
        }

        public async Task AddAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public async Task<T> FindAsync(string discription)
        {
            return await _dbSet.FindAsync(discription);
        }

        public T FindById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> FindAsync(int number)
        {
            return await _dbSet.FindAsync(number);
        }

        public IEnumerable<T> GetAllAsync(/*Expression<Func<T, bool>> predicate = null*/)
        {
            var result = _dbSet.ToList();
            return result;
            //return predicate != null ? result.Where(predicate) : result;
        }

        public async Task Edit(T obj)
        {
            _dbSet.Update(obj);
            await SaveChangeAsync();
        }

        private async Task SaveChangeAsync()
        {
            await _telegramContext.SaveChangesAsync();
        }
    }
}
