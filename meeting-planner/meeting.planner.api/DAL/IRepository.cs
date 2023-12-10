using JWTAuth.WebApi.Models;
using MeetingPlannerAPI.Model;


namespace MeetingPlannerAPI.DAL
{
    public interface IRepository<T>
    where T : class
    {
        public IQueryable<T> getAll();
        public void Add(T input);
        public void Update(T input);  
    }
}