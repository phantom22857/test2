using Microsoft.AspNetCore.Mvc;
using test27180.DTOs.CharacterDto;
using test27180.Services;

using Microsoft.AspNetCore.Mvc;
using test27180.DTOs.CharacterDto;
using test27180.Services;
using test27180.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test27180.Controller
{
    [Route("api/characters")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly IDbService _dbService;
        public CharactersController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{characterId:int}")]
        public async Task<IActionResult> GetCharacter(int characterId)
        {
            if (!await _dbService.DoesCharacterExist(characterId))
            {
                return NotFound($"Character with ID {characterId} not found");
            }

            var character = await _dbService.GetCharacter(characterId);
            var backpackItems = await _dbService.GetItemsForCharacter(characterId);
            var titles = await _dbService.GetTitlesForCharacter(characterId);

            var characterDto = new GetCharacterDto
            {
                FirstName = character.FirstName,
                LastName = character.LastName,
                CurrentWeight = character.CurrentWeight,
                MaxWeight = character.MaxWeight,
                BackpackItems = backpackItems,
                Titles = titles
            };
            return Ok(characterDto);
        }

        [HttpPost("{characterId:int}/backpacks")]
        public async Task<IActionResult> AddItems(int characterId, [FromBody] List<AddItemDto> items)
        {
            if (!await _dbService.DoesCharacterExist(characterId))
            {
                return NotFound($"Character with ID {characterId} not found");
            }

            var itemIdList = items.Select(i => i.ItemId).ToList();
            if (!await _dbService.DoItemsExist(itemIdList))
            {
                return BadRequest("One or more items do not exist in the database");
            }

            var character = await _dbService.GetCharacter(characterId);
            var totalWeight = await _dbService.GetTotalWeight(itemIdList);
            if (character.CurrentWeight + totalWeight > character.MaxWeight)
            {
                return BadRequest("Character cannot carry the added weight");
            }

            await _dbService.AddItemsToCharacter(characterId, items);

            return Ok("Items added to character's backpack");
        }

    }
}
