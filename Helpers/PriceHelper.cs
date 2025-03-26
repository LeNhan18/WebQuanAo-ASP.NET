public static class PriceHelper
{
    public static string FormatPrice(decimal price)
    {
        // Bỏ phần nhân với 1000
        return price.ToString("N0") + " đ";
    }
} 