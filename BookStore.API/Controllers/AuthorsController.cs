using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Contracts;
using BookStore.API.Data;
using BookStore.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    /// <summary>
    /// Endpoints use to interact with the Authors in the Book Store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository authorRepository;
        private readonly ILoggerService logger;
        private readonly IMapper mapper;
        public AuthorsController(IAuthorRepository authorRepository, ILoggerService logger, IMapper mapper)
        {
            this.authorRepository = authorRepository;
            this.logger = logger;
            this.mapper = mapper;
        }
        /// <summary>
        /// Get all Authors
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Attempted Get All Authors");
                var authors = await authorRepository.FindAll();
                var response = mapper.Map<IList<AuthorDTO>>(authors);
                logger.LogInfo($"{location}: Success got all Authors");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
            
        }

        /// <summary>
        /// Get an Author by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Authors record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Attempted Get Author with an ID:{id}");
                var author = await authorRepository.FindById(id);
                if (author == null)
                {
                    logger.LogWarn($"{location}: Author with ID:{id} not found");
                    return NotFound();
                }
                var response = mapper.Map<AuthorDTO>(author);
                logger.LogInfo($"{location}: Success got Author with ID:{id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Creates an Author
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Author submission attempted");
                if (authorDTO == null)
                {
                    logger.LogWarn($"{location}: Empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"{location}: Author data was incomplete");
                    return BadRequest(ModelState);
                }
                var author = mapper.Map<Author>(authorDTO);
                var isSuccess = await authorRepository.Create(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Author creation failed");
                }
                logger.LogInfo($"{location}: Author created");
                logger.LogInfo($"{location}: {author}");
                return Created("Create", new { author });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Updates an Author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Author update attempted - ID:{id}");
                if (id < 1 || authorDTO == null || id != authorDTO.Id)
                {
                    logger.LogWarn($"{location}: Author update failed with bad data");
                    return BadRequest();
                }
                var exists = await authorRepository.Exists(id);
                if (!exists)
                {
                    logger.LogWarn($"{location}: Author with ID:{id} was not found");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"{location}: Author data was incomplete");
                    return BadRequest(ModelState);
                }
                var author = mapper.Map<Author>(authorDTO);
                var isSuccess = await authorRepository.Update(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Update operation failed");
                }
                logger.LogWarn($"{location}: Author with ID:{id} successfully updated");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Removes an Author by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Author with ID:{id} delete attempted");
                if (id < 1)
                {
                    logger.LogWarn($"{location}: Author delete failed with bad data");
                    return BadRequest();
                }
                var exists = await authorRepository.Exists(id);
                if (!exists)
                {
                    logger.LogWarn($"{location}: Author with ID:{id} was not found");
                    return NotFound();
                }
                var author = await authorRepository.FindById(id);
                var isSuccess = await authorRepository.Delete(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Author delete failed");
                }
                logger.LogWarn($"{location}: Author with ID:{id} successfully deleted");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }
        private ObjectResult InternalError(string message)
        {
            logger.LogError(message);
            return StatusCode(500, "Something went wrong, please contact the Admin");
        }
    }
}
