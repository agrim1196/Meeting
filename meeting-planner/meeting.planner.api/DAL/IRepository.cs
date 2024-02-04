namespace MeetingPlannerAPI.DAL
{
    public interface IRepository<T>
    where T : class
    {
        public void Add(T input);
        public void Update(T input);
        public Task<List<T>> getAllAsync();
    }
}