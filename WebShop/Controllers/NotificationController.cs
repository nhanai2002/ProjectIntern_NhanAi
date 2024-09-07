using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebShop.Models;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Services.Hubs;
using WebShopCore.ViewModel.Notification;
using WebShopCore.ViewModel.User;

namespace WebShop.Controllers
{
    [Display(Name = "Quản lý thông báo")]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly INotificationHub _hub;
        public NotificationController(IUnitOfWork uow, IMapper mapper, INotificationHub hub)
        {
            _uow = uow;
            _mapper = mapper;
            _hub = hub;
        }

        [Display(Name = "Xem thông báo")]
        public IActionResult Index(string keyword, int? selectType, DateTime? startTime, DateTime? endTime)
        {
            var data = _uow.NotificationRepository.BuildQuery(x => !x.IsDeleted);
            if(keyword != null && keyword != "")
            {
                data = data.Where(x => EF.Functions.Like(x.Message, $"%{keyword}%"));
            }
            switch (selectType)
            {
                case (int)SysEnum.NotificationType.All:
                    data = data.Where(x => x.MessageType == (int)SysEnum.NotificationType.All);
                    break;
                case (int)SysEnum.NotificationType.Personal:
                    data = data.Where(x => x.MessageType == (int)SysEnum.NotificationType.Personal);
                    break;
                 case (int)SysEnum.NotificationType.Group:
                    data = data.Where(x => x.MessageType == (int)SysEnum.NotificationType.Group);
                    break;
                default:
                    break;
            }

            if (startTime.HasValue && startTime != null && endTime.HasValue && endTime != null)
            {
                data = data.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime);
            }

            data = data.OrderByDescending(x => x.CreatedAt);
            var result = data.Select(x => _mapper.Map<NotificationViewModel>(x)).ToList();

            return View(result);
        }

        [Display(Name = "Tạo thông báo")]
        public IActionResult Create()
        {
            var users = _uow.UserRepository.BuildQuery(x => !x.IsDeleted).Select(x => _mapper.Map<UserViewModel>(x)).ToList();
            var model = new NotificationCrudModel()
            {
                ListUserVM = users
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NotificationCrudModel model)
        {
            if (ModelState.IsValid)
            {
                var error = new ErrorViewModel();
                try
                {
                    if (model.MessageType == (int)SysEnum.NotificationType.All)
                    {
                        //await _hub.SendNotificationToAll(model.Message);
                        var notify = new Notification()
                        {
                            CreatedAt = DateTime.Now,
                            Title = model.Title,
                            Message = model.Message,
                            MessageType = model.MessageType
                        };
                        await _uow.NotificationRepository.AddAsync(notify);
                        await _uow.CommitAsync();

                        var getAllUsers = _uow.UserRepository.BuildQuery(x => !x.IsDeleted).ToList();
                        foreach (var user in getAllUsers)
                        {
                            var UserNoti = new UserNoti()
                            {
                                NotiId = notify.NotiId,
                                UserId = user.UserId
                            };
                            await _uow.UserNotiRepository.AddAsync(UserNoti);
                        }
                        await _uow.CommitAsync();
                        return RedirectToAction("Index");
                    }

                    else if (model.MessageType == (int)SysEnum.NotificationType.Personal)
                    {
                        var user = _uow.UserRepository.BuildQuery(x => !x.IsDeleted && x.UserId == model.UserId)
                            .Select(x => _mapper.Map<UserViewModel>(x))
                            .FirstOrDefault();
                        if (user != null)
                        {
                            //await _hub.SendNotificationToClient(model.Message, user.UserId);
                            var notify = new Notification()
                            {
                                CreatedAt = DateTime.Now,
                                Title = model.Title,
                                Message = model.Message,
                                MessageType = model.MessageType
                            };
                            await _uow.NotificationRepository.AddAsync(notify);
                            await _uow.CommitAsync();
                            var UserNoti = new UserNoti()
                            {
                                NotiId = notify.NotiId,
                                UserId = user.UserId
                            };
                            await _uow.UserNotiRepository.AddAsync(UserNoti);
                            await _uow.CommitAsync();
                            return RedirectToAction("Index");
                        }
                    }

                    return View(model);
                }
                catch (Exception ex)
                {
                    error.ErrorMessage = ex.Message;
                    return View("Error", error);
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SendNotification(int id, int type)
        {
            try
            {
                var getNoti = _uow.NotificationRepository
                    .BuildQuery(x => x.NotiId == id && !x.IsDeleted).FirstOrDefault();                
                getNoti.SendAt = DateTime.Now;
                await _uow.CommitAsync();


                var result = _uow.NotificationRepository
                    .BuildQuery(x => x.NotiId == id && !x.IsDeleted)
                    .Select(x => _mapper.Map<NotificationViewModel>(x))
                    .FirstOrDefault();
                if (type == (int)SysEnum.NotificationType.All)
                {
                    await _hub.SendNotificationToAll(result);
                }
                else if (type == (int)SysEnum.NotificationType.Personal)
                {
                    var userId = _uow.UserNotiRepository.BuildQuery(x => x.NotiId == id).Select(x => x.UserId).FirstOrDefault();

                    if (userId != null)
                    {
                        await _hub.SendNotificationToClient(result, userId.Value);
                    }
                }
                return Json(new { success = true });
            }
            catch 
            {
                return Json(new { success = false });
            }

        }
    }
}
