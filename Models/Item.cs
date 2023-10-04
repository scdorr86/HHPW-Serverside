using System.ComponentModel;

namespace HHPW_Serverside.Models
{
    public class Item
    {
        public int Id { get; set; }
        public List<Order> Orders { get; set; }
        public int itemTypeId { get; set; }
        public ItemType itemType { get; set; }
        public string itemName { get; set; }
        public decimal price { get; set; }
    }
}
