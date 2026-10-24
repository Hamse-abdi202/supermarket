
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Modules.Models;
using Hospital.Modules.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Modules.controlers
{
    [ApiController]
    [Route("api/[controller]")]
    public class productApi : ControllerBase
    {
        private readonly Productrepo _Iproduct;

        public productApi(Productrepo Iproduc)
        {
            _Iproduct = Iproduc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<product>>> GetProducts()
        {
            var products = await _Iproduct.GetProducts();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<product>> GetproductsbyID(int id)
        {
            var productItem = await _Iproduct.GetProductbyId(id);

            if (productItem == null)
            {
                
                return NotFound();
            }

            return Ok(productItem);
        }

        [HttpPost]
        public async Task<ActionResult<product>> Create([FromBody] product product)
        {
            var newproduct = await _Iproduct.Create(product);

            return CreatedAtAction(
                nameof(GetproductsbyID),
                new { id = newproduct.id },
                newproduct
            );
        }
    }
}
