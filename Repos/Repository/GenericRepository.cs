using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using Core.IRepository;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> entities;
        public GenericRepository(AppDbContext appDbContext)
        {
                _appDbContext = appDbContext;
                 entities = _appDbContext.Set<T>();
        }
        public async Task Create(T model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("Model");
                }

                await entities.AddAsync(model);
                await _appDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var model = await entities.FindAsync(id);
            if (model != null)
            {
                entities.Remove(model);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await entities.Include(x => entities).ToListAsync();
        }


        public async Task UpdateAsync(T Model)
        {
            if (Model == null)
            {
                throw new ArgumentNullException("entity");
            }
             entities.Update(Model);
            await _appDbContext.SaveChangesAsync();
        }




        public async Task<T> GetByIdAsync(Guid id)
        {
            return await entities.FindAsync(id);
        }
    }
}
