using CafeKDSApi.Data;
using CafeKDSApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeKDSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController: Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Get api order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrders()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Orders>> PostOrder(Orders order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderID }, order);
        }

    }
}
