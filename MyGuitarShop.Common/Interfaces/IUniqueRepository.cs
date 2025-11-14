using MyGuitarShop.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Common.Interfaces
{
    public interface IUniqueRepository<TDto> : IRepository<TDto>
    {
        Task<TDto?> FindByUniqueAsync(string ident);
    }
}
