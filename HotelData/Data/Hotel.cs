namespace HotelData.Data
{
    public class Hotel
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
