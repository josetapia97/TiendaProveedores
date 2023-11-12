using API.FurnitoreStore.Data;
using API.FurnitoreStore.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FurnitoreStore.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly APIFurnitureStoreContext _context;
        public OrdersController(APIFurnitureStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            //En este caso no es solo imprimir ordenes, tambien hay que incluir el detalle (JOIN)

            return await _context.Orders.Include(o=>o.OrderDetails).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o=>o.Id == id);
            if (order == null) return NotFound();
            return Ok(order);

        }

        [HttpPost]
        public async Task<IActionResult> Post(Order order)
        {
            if (order == null) return NotFound();
            if (order.OrderDetails == null) return BadRequest();
            //inserto orden
            await _context.Orders.AddAsync(order);
            //antes de saveChanges, inserto los detalles como lista
            await _context.OrderDetails.AddRangeAsync(order.OrderDetails);

            await _context.SaveChangesAsync();
            return CreatedAtAction("Post", order.Id, order);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Order order)
        {
            if(order == null) return NotFound();
            if(order.Id <= 0) return NotFound();

            //traer orden de la bd
            var existingOrder = await _context.Orders.Include(o=>o.OrderDetails).FirstOrDefaultAsync(o=>o.Id == order.Id);
            if (existingOrder == null) return NotFound();

            //se edita el maestro, Order
            existingOrder.OrderNumber = order.OrderNumber;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.DeliveryDate = order.DeliveryDate;
            existingOrder.ClientId = order.ClientId;

            //se eliminan y se crean OrderDetails
            _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);
            _context.Orders.Update(existingOrder);
            _context.OrderDetails.AddRange(order.OrderDetails);

            //guardar cambios
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Order order)
        {
            if(order == null) return NotFound();

            //traigo la order desde bd
            var existingOrder = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == order.Id);
            if (existingOrder == null) return NotFound();

            //borrar la orden junto a sus detalles
            _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);
            _context.Remove(existingOrder);

            //guardar y devolver
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
