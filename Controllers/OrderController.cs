using CafeKDSApi.Data;
using CafeKDSApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using static System.Net.Mime.MediaTypeNames;

namespace CafeKDSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get latest 20 orders that have either been made or not made
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromBody]bool isMade)
        {
            var groupedItems = await _dbContext.Item
                .Where(o => o.IsMade == isMade)
                .GroupBy(o => o.OrderId)
                .Select(group => new
                {
                    OrderId = group.Key,
                    Items = group.Take(20).ToList()
                })
                .Take(20)
                .ToListAsync();

            if (groupedItems == null || !groupedItems.Any())
            {
                return NotFound("No orders found for the provided OrderIds.");
            }

            var ordersWithItems = groupedItems.Select(g => new Order
            {
                OrderId = g.OrderId,
                Items = g.Items
            }).ToList();

            return Ok(ordersWithItems);
        }



        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _dbContext.Order.FindAsync(id);

            return order;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Order>>> PostOrder(Order order)
        {
            if (order == null || !order.Items.Any())
            {
                return BadRequest("Orders cannot be null or empty.");
            }

            // Get a new ItemId
            var newOrderId = await GetNewItemIdAsync();
            _dbContext.Order.Add(order);
            await _dbContext.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetOrder), new { id = newOrderId }, order);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateIsMade(int orderId, [FromBody] bool isMade)
        {
            var items = await _dbContext.Item
                .Where(o => o.OrderId == orderId)
                .ToListAsync();

            if (items == null || !items.Any())
            {
                return NotFound("No items found for the provided OrderId.");
            }

            items.ForEach(item => item.IsMade = isMade);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the items.");
            }

            return NoContent();
        }

        // Helper method to get a new ItemId from the database
        private async Task<int> GetNewItemIdAsync()
        {
            var lastOrder = await _dbContext.Order.OrderByDescending(o => o.OrderId).FirstOrDefaultAsync();
            if (lastOrder != null)
            {
                return lastOrder.OrderId + 1;
            }
            else
            {
                // If there are no orders in the database yet, start from 1
                return 1;
            }
        }

        
    }
}
