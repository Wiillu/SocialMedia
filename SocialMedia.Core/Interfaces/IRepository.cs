using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        //debe devolver la entidad en la que se realiza

        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);

        Task Add(T entity);

        Task Update(T entity);

        Task Delete(int id);

        
    }
}
