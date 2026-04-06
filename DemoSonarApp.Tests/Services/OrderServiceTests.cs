using DemoSonarApp.Services;

namespace DemoSonarApp.Tests.Services;

[TestClass]
public class OrderServiceTests
{
    private OrderService _orderService = null!;

    [TestInitialize]
    public void Setup()
    {
        _orderService = new OrderService();
    }

    // ??????????????????????????????????????????????
    // Basic sum
    // ??????????????????????????????????????????????

    [TestMethod]
    public void CalculateTotal_WithEmptyPriceList_ReturnsZero()
    {
        // Arrange
        var prices = new List<decimal>();

        // Act
        var result = _orderService.CalculateTotal(prices, "REGULAR", 0);

        // Assert
        Assert.AreEqual(0m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithSinglePrice_ReturnsThatPrice()
    {
        // Arrange
        var prices = new List<decimal> { 100m };

        // Act
        var result = _orderService.CalculateTotal(prices, "REGULAR", 0);

        // Assert
        Assert.AreEqual(100m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithMultiplePrices_ReturnsSumOfPrices()
    {
        // Arrange
        var prices = new List<decimal> { 10m, 20m, 30m };

        // Act
        var result = _orderService.CalculateTotal(prices, "REGULAR", 0);

        // Assert
        Assert.AreEqual(60m, result);
    }

    // ??????????????????????????????????????????????
    // VIP discount (20 %)
    // ??????????????????????????????????????????????

    [TestMethod]
    public void CalculateTotal_WithVipCustomer_Applies20PercentDiscount()
    {
        // Arrange
        var prices = new List<decimal> { 100m };

        // Act
        var result = _orderService.CalculateTotal(prices, "VIP", 0);

        // Assert
        Assert.AreEqual(80m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithVipCustomerAndMultiplePrices_Applies20PercentOnTotal()
    {
        // Arrange
        var prices = new List<decimal> { 50m, 50m, 100m }; // sum = 200

        // Act
        var result = _orderService.CalculateTotal(prices, "VIP", 0);

        // Assert
        Assert.AreEqual(160m, result); // 200 - 40
    }

    [TestMethod]
    public void CalculateTotal_WithVipCustomerAndZeroPrices_ReturnsZero()
    {
        // Arrange
        var prices = new List<decimal> { 0m, 0m };

        // Act
        var result = _orderService.CalculateTotal(prices, "VIP", 0);

        // Assert
        Assert.AreEqual(0m, result);
    }

    // ??????????????????????????????????????????????
    // Flat discount
    // ??????????????????????????????????????????????

    [TestMethod]
    public void CalculateTotal_WithPositiveDiscount_SubtractsDiscountFromTotal()
    {
        // Arrange
        var prices = new List<decimal> { 100m };

        // Act
        var result = _orderService.CalculateTotal(prices, "REGULAR", 15m);

        // Assert
        Assert.AreEqual(85m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithZeroDiscount_DoesNotSubtractAnything()
    {
        // Arrange
        var prices = new List<decimal> { 100m };

        // Act
        var result = _orderService.CalculateTotal(prices, "REGULAR", 0m);

        // Assert
        Assert.AreEqual(100m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithNegativeDiscount_DoesNotSubtractAnything()
    {
        // Arrange
        var prices = new List<decimal> { 100m };

        // Act
        var result = _orderService.CalculateTotal(prices, "REGULAR", -10m);

        // Assert
        Assert.AreEqual(100m, result); // negative discount is ignored (discount > 0 is false)
    }

    // ??????????????????????????????????????????????
    // VIP + flat discount combined
    // ??????????????????????????????????????????????

    [TestMethod]
    public void CalculateTotal_WithVipCustomerAndPositiveDiscount_AppliesVipDiscountThenFlatDiscount()
    {
        // Arrange
        var prices = new List<decimal> { 200m }; // sum = 200, VIP -> 160, discount 10 -> 150

        // Act
        var result = _orderService.CalculateTotal(prices, "VIP", 10m);

        // Assert
        Assert.AreEqual(150m, result);
    }

    // ??????????????????????????????????????????????
    // INTERNAL customer (logging branch)
    // ??????????????????????????????????????????????

    [TestMethod]
    public void CalculateTotal_WithInternalCustomer_ReturnsCorrectTotal()
    {
        // Arrange
        var prices = new List<decimal> { 100m };

        // Act
        var result = _orderService.CalculateTotal(prices, "INTERNAL", 0m);

        // Assert
        Assert.AreEqual(100m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithInternalCustomerAndDiscount_AppliesDiscount()
    {
        // Arrange
        var prices = new List<decimal> { 100m };

        // Act
        var result = _orderService.CalculateTotal(prices, "INTERNAL", 20m);

        // Assert
        Assert.AreEqual(80m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithInternalCustomerAndMultiplePrices_ReturnsSumMinusDiscount()
    {
        // Arrange
        var prices = new List<decimal> { 40m, 60m }; // sum = 100

        // Act
        var result = _orderService.CalculateTotal(prices, "INTERNAL", 10m);

        // Assert
        Assert.AreEqual(90m, result);
    }

    // ??????????????????????????????????????????????
    // Unknown / arbitrary customer type
    // ??????????????????????????????????????????????

    [TestMethod]
    public void CalculateTotal_WithUnknownCustomerType_ReturnsRawSum()
    {
        // Arrange
        var prices = new List<decimal> { 30m, 70m };

        // Act
        var result = _orderService.CalculateTotal(prices, "GUEST", 0m);

        // Assert
        Assert.AreEqual(100m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithEmptyCustomerType_ReturnsRawSum()
    {
        // Arrange
        var prices = new List<decimal> { 50m };

        // Act
        var result = _orderService.CalculateTotal(prices, string.Empty, 0m);

        // Assert
        Assert.AreEqual(50m, result);
    }

    // ??????????????????????????????????????????????
    // Decimal precision
    // ??????????????????????????????????????????????

    [TestMethod]
    public void CalculateTotal_WithDecimalPrices_ReturnsCorrectDecimalTotal()
    {
        // Arrange
        var prices = new List<decimal> { 9.99m, 4.99m, 0.02m }; // sum = 15.00

        // Act
        var result = _orderService.CalculateTotal(prices, "REGULAR", 0m);

        // Assert
        Assert.AreEqual(15.00m, result);
    }

    [TestMethod]
    public void CalculateTotal_WithVipAndDecimalPrices_ReturnsCorrectDiscountedTotal()
    {
        // Arrange
        var prices = new List<decimal> { 50m }; // VIP 20% -> 40

        // Act
        var result = _orderService.CalculateTotal(prices, "VIP", 5m);

        // Assert
        Assert.AreEqual(35m, result); // 40 - 5
    }
}
