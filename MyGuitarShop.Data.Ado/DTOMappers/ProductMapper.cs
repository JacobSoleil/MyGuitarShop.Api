using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.DTOMappers
{
    public static class ProductMapper
    {
        public static ProductEntity ToEntity(ProductDto dto)
        {
            dto.ProductID ??= 0;
            dto.DateAdded ??= DateTime.UtcNow;

            return new ProductEntity
            {
                ProductID = dto.ProductID.Value,
                CategoryID = dto.CategoryID,
                ProductCode = dto.ProductCode,
                ProductName = dto.ProductName,
                Description = dto.Description,
                ListPrice = dto.ListPrice,
                DiscountPercent = dto.DiscountPercent,
                DateAdded = dto.DateAdded
            };
        }

        public static ProductDto ToDto(ProductEntity entity)
        {
            return new ProductDto
            {
                ProductID = entity.ProductID,
                CategoryID = entity.CategoryID,
                ProductCode = entity.ProductCode,
                ProductName = entity.ProductName,
                Description = entity.Description,
                ListPrice = entity.ListPrice,
                DiscountPercent = entity.DiscountPercent,
                DateAdded = entity.DateAdded
            };
        }
    }
}
