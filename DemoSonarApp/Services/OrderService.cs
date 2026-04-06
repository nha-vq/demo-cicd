namespace DemoSonarApp.Services;

public class OrderService
{
    public decimal CalculateTotal(List<decimal> prices, string customerType, decimal discount)
    {
        decimal total = 0;

        foreach (var price in prices)
        {
            total += price;
        }

        if (customerType == "VIP")
        {
            total -= total * 0.2m;
        }

        if (discount > 0)
        {
            total -= discount;
        }

        if (customerType == "INTERNAL")
        {
            Console.WriteLine("Internal customer discount applied");
        }

        return total;
    }
}