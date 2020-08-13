namespace BookStore.API.Services
{
    using BookStore.API.Contracts;
    using BookStore.API.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext db;
        public BookRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<bool> Create(Book entity)
        {
            await db.Books.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            db.Books.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            var exists = await db.Books.AnyAsync(b => b.Id == id);
            return exists;
        }

        public async Task<IList<Book>> FindAll()
        {
            var books = await db.Books.ToListAsync();
            return books;
        }

        public async Task<Book> FindById(int id)
        {
            var book = await db.Books.FindAsync(id);
            return book;
        }

        public async Task<bool> Save()
        {
            var changes = await db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Book entity)
        {
            db.Books.Update(entity);
            return await Save();
        }
    }
}
