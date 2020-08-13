namespace BookStore.API.Controllers
{
    using System.Collections.Generic;
    using BookStore.API.Contracts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// This is a test API controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILoggerService logger;
        public HomeController(ILoggerService logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Gets values
        /// </summary>
        /// <returns></returns>
        // GET: api/<HomeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            logger.LogInfo("Accessed Home Controler");
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HomeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
