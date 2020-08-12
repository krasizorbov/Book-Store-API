using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Contracts;
using BookStore.API.Data;
using BookStore.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    /// <summary>
    /// Interacts with the Books table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;
        private readonly ILoggerService logger;
        private readonly IMapper mapper;
        public BooksController(IBookRepository bookRepository, ILoggerService logger, IMapper mapper)
        {
            this.bookRepository = bookRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>A List of Books</returns>
        [HttpGet]
        [Authorize(Roles = "Administrator, Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Attemted Call");
                var books = await bookRepository.FindAll();
                var response = mapper.Map<IList<BookDTO>>(books);
                logger.LogInfo($"{location}: Successfull");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Gets a book by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A book record</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Attemted Call for ID:{id}");
                var book = await bookRepository.FindById(id);
                if (book == null)
                {
                    logger.LogWarn($"{location}: Failed to retrive record with ID:{id}");
                    return NotFound();
                }
                var response = mapper.Map<BookDTO>(book);
                logger.LogInfo($"{location}: Successfully got record with ID:{id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Create a new Book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns>Book Object</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Create Attemted");
                if (bookDTO == null)
                {
                    logger.LogWarn($"{location}: Empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"{location}: Data was incomplete");
                    return BadRequest(ModelState);
                }
                var book = mapper.Map<Book>(bookDTO);
                var isSuccess = await bookRepository.Create(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Creation Failed");
                }
                logger.LogInfo($"{location}: Creation was successfull");
                logger.LogInfo($"{location}: {book}");
                return Created("Create", new { book });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Updates a Book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Update Attemted on record with ID:{id}");
                if (id < 1 || bookDTO == null || id != bookDTO.Id)
                {
                    logger.LogWarn($"{location}: Update failed with bad data with ID:{id}");
                    return BadRequest();
                }
                var exists = await bookRepository.Exists(id);
                if (!exists)
                {
                    logger.LogWarn($"{location}: Failed to retrieve record with ID:{id}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"{location}: Data was incomplete");
                    return BadRequest(ModelState);
                }
                var book = mapper.Map<Book>(bookDTO);
                var isSuccess = await bookRepository.Update(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Update Failed");
                }
                logger.LogInfo($"{location}: Record with ID:{id} successfully updated");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Removes a Book by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: Book with ID:{id} delete attempted");
                if (id < 1)
                {
                    logger.LogWarn($"{location}: Book delete failed with bad data");
                    return BadRequest();
                }
                var exists = await bookRepository.Exists(id);
                if (!exists)
                {
                    logger.LogWarn($"{location}: Book with ID:{id} was not found");
                    return NotFound();
                }
                var book = await bookRepository.FindById(id);
                var isSuccess = await bookRepository.Delete(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Book delete failed");
                }
                logger.LogWarn($"{location}: Book with ID:{id} successfully deleted");
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
