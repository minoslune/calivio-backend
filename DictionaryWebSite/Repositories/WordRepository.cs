using DictionaryWebSite.Data;
using DictionaryWebSite.Models_DTOs;
using Microsoft.EntityFrameworkCore;

namespace DictionaryWebSite.Repositories
{
    public class WordRepository : IWordRepository
    {

        //Dependency injection beginning
            private readonly AppDbContext _context;

        public WordRepository(AppDbContext context)
            {
            //This variable will contain the instance of the AppDbContext that is created by the dependency injection in the Program.cs
            //file, this allows us to use the database context in this repository class.
            _context = context;
        }
        //Dependency injection end

        public async Task AddWordAsync(Word word)
        {
            await _context.Words.AddAsync(word);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWordAsync(int id)
        {
            //This will try to assign the employee data with the given ID to the nullable var below
            var wordInDb = await _context.Words.FindAsync(id);

            //If the employee ID is not found in the DB, this if will throw an exception, halting the code.
            if (wordInDb == null)
            {
                throw new KeyNotFoundException($"Employee with id {id} was not found");
            }
            //if the employee data exists, then we will proceed to remove it.
            _context.Words.Remove(wordInDb);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Word>> GetAllWordsAsync()
        {
            return await _context.Words.ToListAsync();
        }

        public async Task<Word?> GetWordByIdAsync(int id)
        {
            return await _context.Words.FindAsync(id);  
        }

        public async Task UpdateWordAsync(Word word)
        {
            _context.Words.Update(word);
            await _context.SaveChangesAsync();
        }
    }
}
