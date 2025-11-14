using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Api.Abstract;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.EFCore.Entities;
using MyGuitarShop.Data.EFCore.Repositories;

namespace MyGuitarShop.Api.EFCoreControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersEFCoreController(
        CustomerRepository repository,
        ILogger<CustomersEFCoreController> logger)
        : BaseController<CustomerDto, Customer>(repository, logger)
    {
    }
}