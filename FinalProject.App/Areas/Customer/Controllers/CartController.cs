using FinalProject.Core.Feature.Apponitments.Command.Models;
using FinalProject.Core.Feature.Apponitments.Query.Models;
using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.PaymentModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IDoctorServices _doctorServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly IAppointmentServices _appointmentServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientServices _patientServices;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IMediator mediator, UserManager<ApplicationUser> userManager,
            IDoctorServices doctorServices, IDepartmentServices departmentServices,
            IAppointmentServices appointmentServices, IPatientServices patientServices ,IUnitOfWork unitOfWork)
        {
            this._mediator = mediator;
            this._doctorServices = doctorServices;
            this._departmentServices = departmentServices;
            this._appointmentServices = appointmentServices;
            this._userManager = userManager;
            this._patientServices = patientServices;
            this._unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? query, int page = 1)
        {

            var response = await _mediator.Send(new GetAllApponintmentsQuery
            {
                Query = query,
                Page = page,
                PageSize = 10
            });

            ViewBag.CurrentQuery = query;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(response.TotalCount / 10.0);

            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int doctorId, int patientId)
        {
            var Dr = await _doctorServices.GetById(doctorId);
            ViewBag.Dr = Dr;
            var patient = await _patientServices.GetById(patientId);
            ViewBag.Patient = patient;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddNewAppointmentCommand command)
        {
            var userId = _userManager.GetUserId(User);
            var id = await _appointmentServices.GetPatientIdFromUserAsync(userId);
            if (id == null)
            {
                ModelState.AddModelError("", "يرجى إكمال بيانات المريض أولاً.");
                return View(command);
            }
            command.PatientId = (int)id;

            var DeptId = await _departmentServices.findDeptByDocId(command.DoctorId);

            command.DepartmentId = DeptId;
            var response = await _mediator.Send(command);
            if (response == 0)
                return RedirectToAction("Create", "Cart", new { area = "Customer", doctorId = command.DoctorId, patientId = command.PatientId });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Pay()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var appointments = _appointmentServices.GetAll().ToList();
                if (!appointments.Any())
                {
                    return BadRequest("لا توجد مواعيد متاحة للدفع");
                }

                // حساب المبلغ الإجمالي بناءً على نوع الموعد
                var orderTotal = (double)appointments.Sum(e =>
                    e.Status == Status.Initial ? e.Doctor.IntialPrice : e.Doctor.FollowUpPrice);

                var order = new Order
                {
                    ApplicationUserId = userId,
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.Pending,
                    OrderTotal = orderTotal
                };

                _unitOfWork.OrderRepository.Create(order);
                await _unitOfWork.CompleteAsync();

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/checkout/Success?orderId={order.Id}",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
                };

                foreach (var item in appointments)
                {
                    if (item.Doctor == null) continue;

                    decimal? price = item.Status == Status.Initial
                        ? item.Doctor.IntialPrice
                        : item.Doctor.FollowUpPrice;

                    if (!price.HasValue) continue;

                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "EGP",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Doctor.Name,
                                Description = item.Doctor.Details
                            },
                            UnitAmount = (long)(price.Value * 100)
                        },
                        Quantity = 1
                    });
                }

                var service = new SessionService();
                var session = service.Create(options);
                order.SessionId = session.Id;
                await _unitOfWork.CompleteAsync();

                // إنشاء عناصر الطلب باستخدام السعر الصحيح
                List<OrderItem> orderItems = new();
                foreach (var item in appointments)
                {
                    if (item.Doctor == null) continue;

                    decimal? price = item.Status == Status.Initial
                        ? item.Doctor.IntialPrice
                        : item.Doctor.FollowUpPrice;

                    if (!price.HasValue) continue;

                    var orderItem = new OrderItem()
                    {
                        OrderId = order.Id,
                        Price = (double)price.Value, // استخدام السعر الصحيح هنا
                        DoctorId = item.Doctor.Id
                    };
                    orderItems.Add(orderItem);
                }

                _unitOfWork.OrderItemRepository.CreateRange(orderItems);
                await _unitOfWork.CompleteAsync();

                return Redirect(session.Url);
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                return StatusCode(500, "حدث خطأ أثناء معالجة الدفع");
            }
        }
        //public async Task<IActionResult> Pay()
        //{
        //    var userId = _userManager.GetUserId(User);
        //    var appointment = _appointmentServices.GetAll();

        //    var order = new Order();
        //    order.ApplicationUserId = userId;
        //    order.OrderDate = DateTime.Now;
        //    order.Status = OrderStatus.Pending;
        //    order.OrderTotal =(double) appointment.Sum(e =>
        //          (e.Status == Status.Initial ? e.Doctor.IntialPrice : e.Doctor.FollowUpPrice) );


        //    _unitOfWork.OrderRepository.Create(order);

        //    await _unitOfWork.CompleteAsync();

        //    var options = new SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string> { "card" },
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/checkout/Success?orderId={order.Id}",
        //        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
        //    };

        //    foreach (var item in appointment)
        //    {
        //        if (item.Doctor == null) continue;

        //        decimal? price = item.Status == Status.Initial
        //            ? item.Doctor.IntialPrice
        //            : item.Doctor.FollowUpPrice;

        //        if (!price.HasValue) continue;

        //        options.LineItems.Add(new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                Currency = "EGP",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = item.Doctor.Name,
        //                    Description = item.Doctor.Details
        //                },
        //                UnitAmount = (long)(price.Value * 100)
        //            },
        //            Quantity = 1
        //        });
        //    }

        //    var service = new SessionService();
        //    var session = service.Create(options);
        //    order.SessionId = session.Id;
        //    await _unitOfWork.CompleteAsync();

        //    List<OrderItem> orderItems = [];
        //    foreach (var item in appointment)
        //    {
        //        var orderItem = new OrderItem()
        //        {
        //            OrderId = order.Id,
        //            Price = (double)item.Doctor.IntialPrice,
        //            DoctorId = item.Doctor.Id
        //        };
        //        orderItems.Add(orderItem);
        //    }
        //    _unitOfWork.OrderItemRepository.CreateRange(orderItems);
        //    await _unitOfWork.CompleteAsync();
        //    return Redirect(session.Url);




        //}



    }
    }
