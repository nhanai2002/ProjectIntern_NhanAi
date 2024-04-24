using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WebShop.Permission;
using WebShopCore.Const;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.ViewModel.Home;

namespace WebShop.Controllers
{
    [Display(Name = "Quản lý thống kê")]
    [LoginRequired]
    public class ReportController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ReportController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [Display(Name = "Xem thống kê")]
        public IActionResult Index(string keyword, DateTime? startTime, DateTime? endTime)
        {
            // tháng hiện tại
            var orders = _uow.OrderRepository.BuildQuery(x => !x.IsDeleted
                && x.ShippingStatus == (int)SysEnum.ShippingStatus.Completed
                && x.OrderStatus == (int)SysEnum.OrderStatus.Completed
                && x.PaymentStatus == (int)SysEnum.PaymentStatus.Completed
                && x.UpdatedAt.Month == DateTime.Now.Month)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ToList();

            if (startTime.HasValue && startTime != null && endTime.HasValue && endTime != null)
            {
                orders = orders.Where(x => x.UpdatedAt >= startTime && x.UpdatedAt <= endTime).ToList();
            }
            var models = orders.SelectMany(x => x.OrderItems)
                            .GroupBy(x => x.Product)
                            .Select(x => new ProductDashboardViewModel
                            {
                                ProductId = x.Key.ProductId,
                                Code = x.Key.Code,
                                Name = x.Key.Name,
                                TotalSold = x.Sum(p => p.Quantity),
                                ProductRevenue = x.Sum(p => p.PriceTotal),
                            })
                            .ToList();
            if (keyword != null && keyword != "")
            {
                models = models.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") 
                    || EF.Functions.Like(x.Code, $"%{keyword}%"))
                    .ToList();
            }

            return View(models);
        }


        [Display(Name = "Xuất file excel")]
        public async Task<IActionResult> ExportExcel(DateTime? startTime, DateTime? endTime)
        {
             var orders = _uow.OrderRepository.BuildQuery(x => !x.IsDeleted
                && x.ShippingStatus == (int)SysEnum.ShippingStatus.Completed
                && x.OrderStatus == (int)SysEnum.OrderStatus.Completed
                && x.PaymentStatus == (int)SysEnum.PaymentStatus.Completed
                && x.UpdatedAt.Month == DateTime.Now.Month)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ToList();

            if (startTime.HasValue && startTime != null && endTime.HasValue && endTime != null)
            {
                orders = orders.Where(x => x.UpdatedAt >= startTime && x.UpdatedAt <= endTime).ToList();
            }
            var models = orders.SelectMany(x => x.OrderItems)
                            .GroupBy(x => x.Product)
                            .Select(x => new ProductDashboardViewModel
                            {
                                ProductId = x.Key.ProductId,
                                Code = x.Key.Code,
                                Name = x.Key.Name,
                                TotalSold = x.Sum(p => p.Quantity),
                                ProductRevenue = x.Sum(p => p.PriceTotal),
                            })
                            .ToList();

            var result = GenerateExcel(models);
            if(result == null)
            {
                return NotFound("Lỗi tải xuống");
            }
            return result;
        }

        private FileResult GenerateExcel(List<ProductDashboardViewModel> models)
        {
            try
            {
                string filename = "ThongKe_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                DataTable dataTable = new DataTable("Thống kê sản phẩm");
                dataTable.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("Mã sản phẩm"),
                    new DataColumn("Tên sản phẩm"),
                    new DataColumn("Lượt mua"),
                    new DataColumn("Doanh thu")
                });
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                foreach (var item in models)
                {
                    dataTable.Rows.Add(item.Code, item.Name, item.TotalSold, String.Format(info, "{0:c}", item.ProductRevenue));
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dataTable);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            filename);
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
