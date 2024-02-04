using JWTAuth.WebApi.Models;
using System.Data.Entity;

namespace MeetingPlannerAPI.DAL
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        readonly DatabaseContext _dbContext = new();

        public Repository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> getAllAsync()
        {
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch
            {
                throw;
            }
        }


        public void Add(T input)
        {
            try
            {
                _dbContext.Set<T>().Add(input);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }

        }

        public void Update(T input)
        {
            try
            {
                _dbContext.Set<T>().Update(input);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}