using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using Productt.Models;

public class PaymentService
{
    private readonly IConfiguration _configuration;
    private readonly VNPayConfig _vnPayConfig;

    public PaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        _vnPayConfig = configuration.GetSection(VNPayConfig.ConfigName).Get<VNPayConfig>();
    }

    public string CreatePaymentUrl(Order order, HttpContext context)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();
        var pay = new VnPayLibrary();

        var urlCallBack = _vnPayConfig.ReturnUrl;

        pay.AddRequestData("vnp_Version", _vnPayConfig.Version);
        pay.AddRequestData("vnp_Command", "pay");
        pay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)order.TotalAmount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", "VND");
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", "vn");
        pay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang #{order.OrderId}");
        pay.AddRequestData("vnp_OrderType", "fashion");
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", tick);

        var paymentUrl = pay.CreateRequestUrl(_vnPayConfig.PaymentUrl, _vnPayConfig.HashSecret);

        return paymentUrl;
    }
} 