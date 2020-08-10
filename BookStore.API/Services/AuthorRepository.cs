using BookStore.API.Contracts;
using BookStore.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext db;
        public AuthorRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public Task<bool> Create(Author entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Author entity)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Author>> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task<Author> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Author entity)
        {
            throw new NotImplementedException();
        }
    }
}
