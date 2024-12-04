using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeZone_Task.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [ForeignKey("Stores")]
        public int Store_Id { get; set; }
        public virtual Store? Stores { get; set; }

        [ForeignKey("Items")]
        public int Item_Id { get; set; }
        public virtual Item? Items { get; set; }

        public int Stock { get; set; } = 0;

        [NotMapped]
        public int New_Stock { get; set; } = 0;
    }
}
