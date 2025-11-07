using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.DTOMappers
{
    public class AddressMapper
    {
        public static AddressEntity ToEntity(AddressDto dto)
        {
            dto.AddressID ??= 0;

            return new AddressEntity
            {
                AddressID = dto.AddressID.Value,
                CustomerID = dto.CustomerID,
                Line1 = dto.Line1,
                Line2 = dto.Line2,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                Phone = dto.Phone,
                Disabled = dto.Disabled
            };
        }

        public static AddressDto ToDto(AddressEntity entity)
        {
            return new AddressDto
            {
                AddressID = entity.AddressID,
                CustomerID = entity.CustomerID,
                Line1 = entity.Line1,
                Line2 = entity.Line2,
                City = entity.City,
                State = entity.State,
                ZipCode = entity.ZipCode,
                Phone = entity.Phone,
                Disabled = entity.Disabled
            };
        }
    }
}
