using Microsoft.EntityFrameworkCore;
using test27180.Models;

namespace test27180.Context;

public class ApplicationContext : DbContext
{
    protected ApplicationContext()
    {
    }
    public ApplicationContext(DbContextOptions options) : base(options) 
    { 
    } 
    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<Character?> Characters { get; set; }
    public DbSet<CharacterTitle> CharacterTitles { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Title> Titles { get; set; }
}