using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.OrderDtos;
using Assist.Lunch._4.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Assist.Lunch._4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get")]
        [SwaggerOperation(Summary = "Get a list of all orders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDto<ResponseOrderDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> Get()
        {
            var response = await orderService.GetAll();

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-by-user/{userId}")]
        [SwaggerOperation(Summary = "Get a list of orders by user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDto<ResponseOrderDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var response = await orderService.GetByUser(userId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-by-order/{orderId}")]
        [SwaggerOperation(Summary = "Get an order")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseOrderDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> GetBy(Guid orderId)
        {
            var response = await orderService.GetById(orderId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add a new order")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseOrderDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Order already exists")]
        public async Task<IActionResult> Add(BaseOrderDto baseOrderDto)
        {
            var response = await orderService.Add(baseOrderDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update order information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseOrderDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        public async Task<IActionResult> Update(UpdateOrderDto updatedOrderDto)
        {
            var response = await orderService.Update(updatedOrderDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("delete")]
        [SwaggerOperation(Summary = "Delete today's user order")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(MessageDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var response = await orderService.Delete(userId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-history")]
        [SwaggerOperation(Summary = "Get user's order history")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseOrderHistoryDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> GetOrderHistory([FromQuery] RequestOrderHistoryDto requestOrderHistoryDto)
        {
            var response = await orderService.GetHistoryByUser(requestOrderHistoryDto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-todays-orders")]
        [SwaggerOperation(Summary = "Get a list of today's orders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseDto<ResponseOrderDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> GetTodaysOrders()
        {
            var response = await orderService.GetTodaysOrders();

            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-todays-orders/{userId}")]
        [SwaggerOperation(Summary = "Get a list of today's order by user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request successful", Type = typeof(ResponseOrderDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No entity found")]
        public async Task<IActionResult> GetTodaysOrder(Guid userId)
        {
            var response = await orderService.GetTodaysOrderByUser(userId);

            return Ok(response);
        }
    }
}
