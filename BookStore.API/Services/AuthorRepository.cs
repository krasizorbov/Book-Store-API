namespace BookStore.API.Services
{
    using BookStore.API.Contracts;
    using BookStore.API.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext db;
        public AuthorRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<bool> Create(Author entity)
        {
            await db.Authors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            db.Authors.Remove(entity);
            return await Save();
        }

        public Task<bool> Exists(int id)
        {
            return db.Authors.AnyAsync(a => a.Id == id);
        }

        public async Task<IList<Author>> FindAll()
        {
            var authors = await db.Authors.Include(b => b.Books).ToListAsync();
            return authors;
        }

        public async Task<Author> FindById(int id)
        {
            var author = await db.Authors.Include(b => b.Books).FirstOrDefaultAsync(b => b.Id == id);
            return author;
        }

        public async Task<bool> Save()
        {
            var changes = await db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Author entity)
        {
            db.Authors.Update(entity);
            return await Save();
        }
    }
}
