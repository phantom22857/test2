using test27180.DTOs.CharacterDto;
using test27180.Models;

namespace test27180.Services;

public interface IDbService
{
    Task<bool> DoesCharacterExist(int characterId);
    Task<Character?> GetCharacter(int characterId);
    Task<List<GetBackpackItemsDto>> GetItemsForCharacter(int characterId);
    Task<List<GetTitleDto>> GetTitlesForCharacter(int characterId);
    Task<bool> DoItemsExist(List<int> itemIdList);
    Task<int> GetTotalWeight(List<int> itemIdList);
    Task AddItemsToCharacter(int characterId, List<AddItemDto> itemIdList);
}