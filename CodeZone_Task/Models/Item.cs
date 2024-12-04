using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace CodeZone_Task.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = "";

        public virtual List<Purchase>? Purchases { get; set; }
    }
}
