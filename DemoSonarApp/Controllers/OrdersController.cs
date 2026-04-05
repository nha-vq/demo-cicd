using DemoSonarApp.Models;
using DemoSonarApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoSonarApp.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService = new OrderService();

    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] OrderRequest request)
    {
        var total = _orderService.CalculateTotal(request.Prices, request.CustomerType, request.Discount);
        return Ok(new { total });
    }
}