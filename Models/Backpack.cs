


using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace test27180.Models
{
    [Table("backpacks")]
    [PrimaryKey(nameof(CharacterId),nameof(ItemId))]

    public class Backpack
    {
        
        public int CharacterId { get; set; }
        
        public int ItemId { get; set; }
        
        public int Amount { get; set; }
    }
}
