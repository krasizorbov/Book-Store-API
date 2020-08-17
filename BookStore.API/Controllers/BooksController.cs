namespace BookStore.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using BookStore.API.Common;
    using BookStore.API.Contracts;
    using BookStore.API.Data;
    using BookStore.API.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    /// <summary>
    /// Interacts with the Books table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
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
        /// Gets all Books
        /// </summary>
        /// <returns>A List of Books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: {GlobalConstants.TryGetBooks}");
                var books = await bookRepository.FindAll();
                var response = mapper.Map<IList<BookDTO>>(books);
                logger.LogInfo($"{location}: {GlobalConstants.GetBooks}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Gets a Book by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A book record</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: {GlobalConstants.TryGetBookById}");
                var book = await bookRepository.FindById(id);
                if (book == null)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.BookNotFound}");
                    return NotFound();
                }
                var response = mapper.Map<BookDTO>(book);
                logger.LogInfo($"{location}: {GlobalConstants.BookFoundById}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Creates a new Book
        /// </summary>
        /// <param name="bookCreateDTO"></param>
        /// <returns>Book Object</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookCreateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: {GlobalConstants.TryCreateBook}");
                if (bookCreateDTO == null)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.BookBadRequest}");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.BookDataIncomplete}");
                    return BadRequest(ModelState);
                }
                var book = mapper.Map<Book>(bookCreateDTO);
                var isSuccess = await bookRepository.Create(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: {GlobalConstants.BookCreationFailed}");
                }
                logger.LogInfo($"{location}: {GlobalConstants.BookCreated}");
                logger.LogInfo($"{location}: {book}");
                return Created("Create", new { book });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Updates a Book by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookUpdateDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookUpdateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: {GlobalConstants.TryBookUpdate}");
                if (id < 1 || bookUpdateDTO == null || id != bookUpdateDTO.Id)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.BookUpdateBadData}");
                    return BadRequest();
                }
                var exists = await bookRepository.Exists(id);
                if (!exists)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.BookUpdateNotFound}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.BookInvalidModelState}");
                    return BadRequest(ModelState);
                }
                var book = mapper.Map<Book>(bookUpdateDTO);
                var isSuccess = await bookRepository.Update(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: {GlobalConstants.BookUpdateFailed}");
                }
                logger.LogInfo($"{location}: {GlobalConstants.BookUpdateSuccessfull}");
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
                logger.LogInfo($"{location}: {GlobalConstants.TryDeleteBook}");
                if (id < 1)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.DeleteBookBadData}");
                    return BadRequest();
                }
                var exists = await bookRepository.Exists(id);
                if (!exists)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.DeleteBookByIdNotFound}");
                    return NotFound();
                }
                var book = await bookRepository.FindById(id);
                var isSuccess = await bookRepository.Delete(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: {GlobalConstants.DeleteBookFailed}");
                }
                logger.LogWarn($"{location}: {GlobalConstants.BookDeletionSuccessfull}");
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
            return StatusCode(500, $"{GlobalConstants.Error500}");
        }
    }
}
