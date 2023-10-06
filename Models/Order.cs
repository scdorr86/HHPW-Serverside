namespace HHPW_Serverside.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int orderStatusId { get; set; }
        public OrderStatus status { get; set; }
        public int orderTypeId { get; set; }
        public OrderType orderType { get; set; }
        public int paymentTypeId { get; set; }
        public PaymentType paymentType { get; set; }
        public List <Item> items { get; set; }
        public decimal orderTotal => items.Sum(i => i.price);
        public string uid { get; set; }
        public User user { get; set; }
        public List<Review> reviews { get; set; }
    }
}
