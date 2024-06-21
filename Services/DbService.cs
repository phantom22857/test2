using Microsoft.EntityFrameworkCore;
using test27180.Context;
using test27180.DTOs.CharacterDto;
using test27180.Models;

namespace test27180.Services
{
    public class DbService : IDbService
    {
        private readonly ApplicationContext _context;
        public DbService(ApplicationContext context)
        {
            _context = context;
            Init().Wait();
        }

        public async Task<bool> DoesCharacterExist(int characterId)
        {
            return await _context.Characters.AnyAsync(c => c.Id == characterId);
        }

        public async Task<Character?> GetCharacter(int characterId)
        {
            return await _context.Characters.FindAsync(characterId);
        }
        public async Task AddItemsToCharacter(int characterId, List<AddItemDto> items)
        {
            var character = await _context.Characters.FindAsync(characterId);
            if (character == null) throw new ArgumentException("Character not found");

            foreach (var itemDto in items)
            {
                var backpackItem = await _context.Backpacks
                    .FirstOrDefaultAsync(b => b.CharacterId == characterId && b.ItemId == itemDto.ItemId);

                if (backpackItem != null)
                {
                    backpackItem.Amount += itemDto.Amount;
                }
                else
                {
                    _context.Backpacks.Add(new Backpack
                    {
                        CharacterId = characterId,
                        ItemId = itemDto.ItemId,
                        Amount = itemDto.Amount
                    });
                }
            }

            var totalWeight = await GetTotalWeight(items.Select(i => i.ItemId).ToList());
            character.CurrentWeight += totalWeight;

            await _context.SaveChangesAsync();
        }


        public async Task<List<GetBackpackItemsDto>> GetItemsForCharacter(int characterId)
        {
            var items = await _context.Backpacks
                .Where(b => b.CharacterId == characterId)
                .Join(
                    _context.Items,
                    b => b.ItemId,
                    i => i.Id,
                    (b, i) => new GetBackpackItemsDto
                    {
                        ItemName = i.Name,
                        ItemWeight = i.Weight,
                        Amount = b.Amount
                    }
                ).ToListAsync();

            return items;
        }

        public async Task<List<GetTitleDto>> GetTitlesForCharacter(int characterId)
        {
            var titles = await _context.CharacterTitles
                .Where(ct => ct.CharacterId == characterId)
                .Join(
                    _context.Titles,
                    ct => ct.TitleId,
                    t => t.Id,
                    (ct, t) => new GetTitleDto
                    {
                        Title = t.Name,
                        AcquiredAt = ct.AcquiredAt
                    }
                ).ToListAsync();

            return titles;
        }

        public async Task<bool> DoItemsExist(List<int> itemIdList)
        {
            return await _context.Items.CountAsync(i => itemIdList.Contains(i.Id)) == itemIdList.Count;
        }

        public async Task<int> GetTotalWeight(List<int> itemIdList)
        {
            return await _context.Items
                .Where(i => itemIdList.Contains(i.Id))
                .SumAsync(i => i.Weight);
        }
        


        
        public async Task Init()
        {
            var character = new Character
            {
                FirstName = "Yakuza",
                LastName = "Cena",
                CurrentWeight = 60,
                MaxWeight = 100
            };
            _context.Characters.Add(character);

            var item = new Item
            {
                Name = "phone",
                Weight = 1
            };
            _context.Items.Add(item);

            var title = new Title
            {
                Name = "Romeo and Juliet"
            };
            _context.Titles.Add(title);

            var characterTitle = new CharacterTitle
            {
                Character = character,
                Title = title,
                AcquiredAt = DateTime.Now
            };
            _context.CharacterTitles.Add(characterTitle);

            var backpack = new Backpack
            {
                CharacterId = character.Id,
                ItemId = item.Id,
                Amount = 1
            };
            _context.Backpacks.Add(backpack);

            await _context.SaveChangesAsync();
        }
    }
}
