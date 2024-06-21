using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using test27180.Models;

namespace test27180.Models;

[Table("items")]
public class Item
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    public int Weight { get; set; }
    public ICollection<Character> Characters { get; set; } =  new HashSet<Character>();

}