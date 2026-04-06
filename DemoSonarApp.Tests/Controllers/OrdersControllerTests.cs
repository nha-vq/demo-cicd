using DemoSonarApp.Controllers;
using DemoSonarApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoSonarApp.Tests.Controllers;

[TestClass]
public class OrdersControllerTests
{
    private OrdersController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _controller = new OrdersController();
    }

    // ??????????????????????????????????????????????
    // HTTP 200 / result shape
    // ??????????????????????????????????????????????

    [TestMethod]
    public void Calculate_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new OrderRequest
        {
            Prices = new List<decimal> { 100m },
            CustomerType = "REGULAR",
            Discount = 0m
        };

        // Act
        var result = _controller.Calculate(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Calculate_WithValidRequest_ReturnsTotalInBody()
    {
        // Arrange
        var request = new OrderRequest
        {
            Prices = new List<decimal> { 100m },
            CustomerType = "REGULAR",
            Discount = 0m
        };

        // Act
        var okResult = _controller.Calculate(request) as OkObjectResult;

        // Assert
        Assert.IsNotNull(okResult);
        var total = okResult.Value?.GetType().GetProperty("total")?.GetValue(okResult.Value);
        Assert.AreEqual(100m, total);
    }

    // ??????????????????????????????????????????????
    // VIP customer via controller
    // ??????????????????????????????????????????????

    [TestMethod]
    public void Calculate_WithVipCustomer_Returns20PercentDiscountedTotal()
    {
        // Arrange
        var request = new OrderRequest
        {
            Prices = new List<decimal> { 100m },
            CustomerType = "VIP",
            Discount = 0m
        };

        // Act
        var okResult = _controller.Calculate(request) as OkObjectResult;

        // Assert
        Assert.IsNotNull(okResult);
        var total = okResult.Value?.GetType().GetProperty("total")?.GetValue(okResult.Value);
        Assert.AreEqual(80m, total);
    }

    // ??????????????????????????????????????????????
    // Flat discount via controller
    // ??????????????????????????????????????????????

    [TestMethod]
    public void Calculate_WithDiscount_ReturnsTotalMinusDiscount()
    {
        // Arrange
        var request = new OrderRequest
        {
            Prices = new List<decimal> { 200m },
            CustomerType = "REGULAR",
            Discount = 50m
        };

        // Act
        var okResult = _controller.Calculate(request) as OkObjectResult;

        // Assert
        Assert.IsNotNull(okResult);
        var total = okResult.Value?.GetType().GetProperty("total")?.GetValue(okResult.Value);
        Assert.AreEqual(150m, total);
    }

    // ??????????????????????????????????????????????
    // INTERNAL customer via controller
    // ??????????????????????????????????????????????

    [TestMethod]
    public void Calculate_WithInternalCustomer_ReturnsCorrectTotal()
    {
        // Arrange
        var request = new OrderRequest
        {
            Prices = new List<decimal> { 100m, 50m },
            CustomerType = "INTERNAL",
            Discount = 0m
        };

        // Act
        var okResult = _controller.Calculate(request) as OkObjectResult;

        // Assert
        Assert.IsNotNull(okResult);
        var total = okResult.Value?.GetType().GetProperty("total")?.GetValue(okResult.Value);
        Assert.AreEqual(150m, total);
    }

    // ??????????????????????????????????????????????
    // Edge cases via controller
    // ??????????????????????????????????????????????

    [TestMethod]
    public void Calculate_WithEmptyPriceList_ReturnsTotalOfZero()
    {
        // Arrange
        var request = new OrderRequest
        {
            Prices = new List<decimal>(),
            CustomerType = "REGULAR",
            Discount = 0m
        };

        // Act
        var okResult = _controller.Calculate(request) as OkObjectResult;

        // Assert
        Assert.IsNotNull(okResult);
        var total = okResult.Value?.GetType().GetProperty("total")?.GetValue(okResult.Value);
        Assert.AreEqual(0m, total);
    }

    [TestMethod]
    public void Calculate_WithVipAndDiscount_AppliesVipDiscountThenFlat()
    {
        // Arrange – sum 200, VIP -> 160, discount 10 -> 150
        var request = new OrderRequest
        {
            Prices = new List<decimal> { 200m },
            CustomerType = "VIP",
            Discount = 10m
        };

        // Act
        var okResult = _controller.Calculate(request) as OkObjectResult;

        // Assert
        Assert.IsNotNull(okResult);
        var total = okResult.Value?.GetType().GetProperty("total")?.GetValue(okResult.Value);
        Assert.AreEqual(150m, total);
    }
}
