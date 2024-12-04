using Microsoft.EntityFrameworkCore;

namespace CodeZone_Task.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; } = "";


        public virtual List<Item>? Items { get; set; }
        public virtual List<Purchase>? Purchases { get; set; }
    }
}
