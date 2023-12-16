namespace HotelData.DTOs
{
    public class HotelSourceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressSourceDTO Address { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
