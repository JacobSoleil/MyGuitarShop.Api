using MyGuitarShop.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.Interfaces
{
    public interface IUniqueRepository<TDto> : IRepository<TDto, int>
    {
        Task<TDto?> FindByUniqueAsync(string ident);
    }
}
