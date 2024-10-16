using Microsoft.EntityFrameworkCore;
using SWD392.Snapper.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Snapper.Repository
{
    public class GenericRepository<T> where T : class
    {
        protected readonly SNAPPETContext _context;

        public GenericRepository(SNAPPETContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Return IQueryable to support LINQ operations
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<int> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _context.Attach(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);  // Attach the entity to the DbContext
        }

        #region Prepare Operations for Batch Actions

        public void PrepareCreate(T entity)
        {
            _context.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            _context.Attach(entity).State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle or log error
                throw new Exception("Error while saving data.", ex);
            }
        }

        #endregion
    }

}
