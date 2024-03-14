using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Services.VnPayService
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        public VnPayService(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymenUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            pay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());

            pay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

            pay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.OrderId);
            pay.AddRequestData("vnp_OrderType", "order"); //default value: other
            pay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);

            // Mã tham chiếu của giao dịch hệ thống của merchant.
            // Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.
            // Không được trùng lặp trong ngày
            pay.AddRequestData("vnp_TxnRef", tick); 

            var paymentUrl = pay .CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();
            foreach(var (key,value) in collection)
            {
                if(!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            long vnp_orderId = 0;
            if (vnpay.GetResponseData("vnp_TxnRef") != null && vnpay.GetResponseData("vnp_TxnRef") != "")
            {
                  vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            }
            long vnp_TransactionId = 0;
            if (vnpay.GetResponseData("vnp_TransactionNo") != null && vnpay.GetResponseData("vnp_TransactionNo") != "")
            {
                vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            }

            var vnp_SecureHash = collection.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false
                };
            }

            return new VnPaymentResponseModel {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}
