namespace HotelData.Data.Repository
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly HotelDataDBContext _dbContext;

        public HotelRepository(HotelDataDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
