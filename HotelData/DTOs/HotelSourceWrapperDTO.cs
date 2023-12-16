namespace HotelData.DTOs
{
    public class HotelSourceWrapperDTO
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public IList<HotelSourceDTO> Hotels { get; set; }
    }
}
