using AutoMapper;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Services.FileUploadService;
using WebShopCore.ViewModel.Product;
using WebShopCore.ViewModel.ProductFeedBack;
using WebShopEndUser.Models;
using WebShopEndUser.Permission;

namespace WebShopEndUser.Controllers
{
    [LoginRequired]
    public class FeedbackController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        public FeedbackController(IUnitOfWork uow, IMapper mapper, IFileUploadService fileUploadService)
        {
            _uow = uow;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
        }

        public IActionResult CreateFeedback(int orderId)
        {
            try
            {
                var order = _uow.OrderRepository.BuildQuery(x => x.OrderId == orderId)
                    .Include(x => x.Feedback)
                    .Include(x => x.User)
                    .FirstOrDefault();
                var product = _uow.OrderItemRepository.BuildQuery(x => x.OrderId == order.OrderId)
                    .Include(x => x.Product)
                        .ThenInclude(x => x.ProductImages)
                        .ThenInclude(x => x.Image)
                    .Select(x => _mapper.Map<ProductViewModel>(x.Product))
                    .ToList();
                if (order != null)
                {
                    if(order.FeedbackId == null)
                    {
                        return View(new FeedbackCrudModel
                        {
                            OrderId = orderId,
                            UserId = order.User.UserId,
                            Products = product
                        });
                    }
                    else
                    {
                        return View("Error", " Đã xảy ra lỗi ");
                    }
                }
                else
                {
                    return View("Error", " Đã xảy ra lỗi ");

                }

            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback(FeedbackCrudModel feedback)
        {
            if (ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    var order = _uow.OrderRepository.BuildQuery(x => x.OrderId == feedback.OrderId)
                                .Include(x => x.Feedback)
                                .Include(x => x.User)
                                .FirstOrDefault();
                    var product = _uow.OrderItemRepository.BuildQuery(x => x.OrderId == order.OrderId)
                                .Include(x => x.Product)
                                .Select(x => x.Product)
                                .ToList();

                    if (order != null)
                    {
                        feedback.IsActive = true;
                        var feedbackNew = _mapper.Map<Feedback>(feedback);
                        _uow.FeedbackRepository.Add(feedbackNew);
                        await _uow.CommitAsync();

                        foreach(var item in product)
                        {
                            var productFeedBack = new ProductFeedBack()
                            {
                                FeedbackId = feedbackNew.FeedbackId,
                                ProductId = item.ProductId
                            };
                            _uow.ProductFeedBackRepository.Add(productFeedBack);
                        }
                        await _uow.CommitAsync();


                        if(feedback.FileImages.Any() && feedback.FileImages.Count()>0)
                        {
                            var urls = new List<string>();
                            foreach (var img in feedback.FileImages)
                            {
                                var url = await _fileUploadService.UpLoadFileAsync(img);
                                urls.Add(url);
                            }
                            foreach (var url in urls)
                            {
                                var model = new Image()
                                {
                                    FeedbackId = feedbackNew.FeedbackId,
                                    Url = url,
                                    IsActive = true
                                };
                                await _uow.ImageRepository.AddAsync(model);
                                await _uow.CommitAsync();

                            }
                        }

                        order.FeedbackId = feedbackNew.FeedbackId;
                        await _uow.CommitAsync();
                        return RedirectToAction("OrderHistory", "Home");
                    }

                }
                catch(Exception ex)
                {
                    return View("Error", ex.Message);
                }
            }
            else if(!ModelState.IsValid)
            {
                var errors = ModelState
        .Where(x => x.Value.Errors.Count > 0)
        .Select(x => new { x.Key, x.Value.Errors })
        .ToArray();
            }
            return View(feedback);
        }
    }
}
