﻿using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    //se crea una base para repositorio para los movimientos basicos tipo CRUD
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        //da acceso a la variable
        private readonly SocialMediaContext _context;
        protected DbSet<T> _entities;
        public BaseRepository(SocialMediaContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task Add(T entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            //variable tipo T que trae el ID
            T entity = await GetById(id);
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()
        {
            //return await _entities.ToListAsync();
            return _entities.AsEnumerable();
        }
        
        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }
        //actualiza toda la entidad
        public void Update(T entity)
        {
            _entities.Update(entity);
            //await _context.SaveChangesAsync();
        }
    }
}
