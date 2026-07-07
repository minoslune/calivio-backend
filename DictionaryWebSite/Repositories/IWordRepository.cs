using DictionaryWebSite.Models_DTOs;

namespace DictionaryWebSite.Repositories
{
    public interface IWordRepository
    {
        //Crud operations
        Task<IEnumerable<Word>> GetAllWordsAsync();
        Task<Word?> GetWordByIdAsync(int id);
        Task AddWordAsync(Word word);
        Task UpdateWordAsync(Word word);
        Task DeleteWordAsync(int id);
    }
}
