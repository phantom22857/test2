namespace test27180.DTOs.CharacterDto;

public class GetCharacterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int CurrentWeight { get; set; }
    public int MaxWeight { get; set; }
    public ICollection<GetBackpackItemsDto> BackpackItems { get; set; } 
    public ICollection<GetTitleDto> Titles { get; set; } 
}

public class GetBackpackItemsDto
{
    public string ItemName { get; set; }
    public int ItemWeight { get; set; }
    public int Amount { get; set; }
}

public class GetTitleDto
{
    public string Title { get; set; }
    public DateTime AcquiredAt { get; set; }

}