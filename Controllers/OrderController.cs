using CafeKDSApi.Data;
using CafeKDSApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            // Retrieve orderIds asynchronously
            var orderIds = await _dbContext.Order
                .Select(o => o.OrderId)
                .ToListAsync();

            // Query orders based on the list of OrderIds
            var groupedItems = await _dbContext.Item
                .Where(o => orderIds.Contains(o.OrderId))
                .GroupBy(o => o.OrderId).ToListAsync();
                

            if (groupedItems == null || !groupedItems.Any())
            {
                return NotFound("No orders found for the provided OrderIds.");
            }
            var ordersWithItems = groupedItems.Select(group => new Order
            {
                OrderId = group.Key,
                Items = group.ToList()
            }).ToList();

            return ordersWithItems;
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
