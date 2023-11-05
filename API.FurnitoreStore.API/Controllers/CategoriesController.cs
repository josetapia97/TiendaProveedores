using API.FurnitoreStore.Data;
using API.FurnitoreStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FurnitoreStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //DB context comunication
        private readonly APIFurnitureStoreContext _context;

        //inyeccion de dependencia en el ctor
        public CategoriesController(APIFurnitureStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductCategory>> Get()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var categories = await _context.ProductCategories.FirstOrDefaultAsync(c => c.Id == id);
            if(categories == null) return NotFound();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductCategory category)
        {
            await _context.ProductCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Post",category.Id,category);
        }


        [HttpPut]
        public async Task<IActionResult> Put(ProductCategory category)
        {
            _context.ProductCategories.Update(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ProductCategory category)
        {
            if (category == null) return NotFound();
            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
