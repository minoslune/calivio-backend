using DictionaryWebSite.Models_DTOs;
using DictionaryWebSite.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryWebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        //Dependency injection beginning
        private readonly IWordRepository _wordRepository;

        //constructor that takes the IWordRepository as a parameter, this will be used by the
        //dependency injection to inject the repository into this controller
        //wordRepository will be passed down from the Program.cs file where we added the dependency injection for the IWordRepository,
        //this will allow us to use the methods defined in the IWordRepository interface in this controller.
        public WordsController(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }
        //Dependency injection end

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<Word>> CreateWord(Word word)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            await _wordRepository.AddWordAsync(word);
            return CreatedAtRoute("GetWordById", new { id = word.Id }, word);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Word>>> GetAllWordsAsync()
        {
            return Ok(await _wordRepository.GetAllWordsAsync());
        }

        //This means the route will be for this specific case: api/repository/id the user wants to find
        [HttpGet("{id}", Name = "GetWordById")]
        public async Task<ActionResult<Word>> GetWordByIdAsync(int id)
        {
            var word = await _wordRepository.GetWordByIdAsync(id);
            if (word == null)
            {
                return NotFound();
            }
            return Ok(word);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> DeleteWordById(int id)
        {
            await _wordRepository.DeleteWordAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Moderator")]
        //The reason why action result has word is because we want to return the word that has been updated.
        public async Task<ActionResult<Word>> UpdateWordById(int id, Word word)
        {
            if (id != word.Id)
            {
                return BadRequest();
            }
            //This if checks if our anotations and validations made on the model of the controller pass.
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            //This method is basically out the box from the framework
            await _wordRepository.UpdateWordAsync(word);

            return CreatedAtRoute("GetWordById", new { id = word.Id }, word);
        }
    }
}
