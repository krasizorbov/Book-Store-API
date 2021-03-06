﻿namespace BookStore.API.Controllers
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
    /// Endpoints use to interact with the Authors in the Book Store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        /// Gets all Authors
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
                logger.LogInfo($"{location}: {GlobalConstants.TryGetAuthors}");
                var authors = await authorRepository.FindAll();
                var response = mapper.Map<IList<AuthorDTO>>(authors);
                logger.LogInfo($"{location}: {GlobalConstants.GetAuthors}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
            
        }

        /// <summary>
        /// Gets an Author by ID
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
                logger.LogInfo($"{location}: {GlobalConstants.TryGetAuthorById}");
                var author = await authorRepository.FindById(id);
                if (author == null)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.AuthorNotFound}");
                    return NotFound();
                }
                var response = mapper.Map<AuthorDTO>(author);
                logger.LogInfo($"{location}: {GlobalConstants.AuthorFoundById}");
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
        /// <param name="authorCreateDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorCreateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: {GlobalConstants.TryCreateAuthor}");
                if (authorCreateDTO == null)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.AuthorBadRequest}");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.AuthorDataIncomplete}");
                    return BadRequest(ModelState);
                }
                var author = mapper.Map<Author>(authorCreateDTO);
                var isSuccess = await authorRepository.Create(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: {GlobalConstants.AuthorCreationFailed}");
                }
                logger.LogInfo($"{location}: {GlobalConstants.AuthorCreated}");
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
        /// <param name="authorUpdateDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorUpdateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: {GlobalConstants.TryAuthorUpdate}");
                if (id < 1 || authorUpdateDTO == null || id != authorUpdateDTO.Id)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.AuthorUpdateBadData}");
                    return BadRequest();
                }
                var exists = await authorRepository.Exists(id);
                if (!exists)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.AuthorUpdateNotFound}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.AuthorInvalidModelState}");
                    return BadRequest(ModelState);
                }
                var author = mapper.Map<Author>(authorUpdateDTO);
                var isSuccess = await authorRepository.Update(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: {GlobalConstants.AuthorUpdateFailed}");
                }
                logger.LogWarn($"{location}: {GlobalConstants.AuthorUpdateSuccessfull}");
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
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                logger.LogInfo($"{location}: {GlobalConstants.TryDeleteAuthor}");
                if (id < 1)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.DeleteAuthorBadData}");
                    return BadRequest();
                }
                var exists = await authorRepository.Exists(id);
                if (!exists)
                {
                    logger.LogWarn($"{location}: {GlobalConstants.DeleteAuthorByIdNotFound}");
                    return NotFound();
                }
                var author = await authorRepository.FindById(id);
                var isSuccess = await authorRepository.Delete(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: {GlobalConstants.DeleteAuthorFailed}");
                }
                logger.LogWarn($"{location}: {GlobalConstants.AuthorDeletionSuccessfull}");
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
