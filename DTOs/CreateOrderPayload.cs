namespace HHPW_Serverside.DTOs
{
    public class CreateOrderPayload
    {
        public int id { get; set; } 
        public int orderStatusId { get; set; }
        public int orderTypeId { get; set; }
        public int paymentTypeId { get; set; }
        public int userId { get; set; }
        public List<int> itemIds { get; set; }
    }
}
