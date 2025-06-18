using CareerBuilderX.Models;
using CareerBuilderX.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CareerBuilderX.Controllers
{
    public class ServiceController : Controller
    {
        private IServiceRepository _serviceRepository { get; set; }

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        // GET: /Service
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var services = _serviceRepository.GetAllServices()
                             .Where(s => !s.IsDeleted)
                             .ToList();
            return View(services);
        }

        // GET: /Service/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Service/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Service service, IFormFile? serviceImage)
        {
            
                if (serviceImage?.Length > 0)
                {
                    using var br = new BinaryReader(serviceImage.OpenReadStream());
                    service.ServiceImg = br.ReadBytes((int)serviceImage.Length);
                    service.ServiceImgName = serviceImage.FileName;
                    service.ServiceImgType = serviceImage.ContentType;
                }

                _serviceRepository.CreateService(service);
                return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var service = _serviceRepository.GetServiceById(id);
            if (service == null || service.IsDeleted) return NotFound();

            return View(service);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Service service, IFormFile? serviceImage)
        {
            var existingService = _serviceRepository.GetServiceById(service.ServiceId);
            if (existingService == null || existingService.IsDeleted)
                return NotFound();

            // تحديث البيانات الأساسية
            existingService.ServiceName = service.ServiceName;
            existingService.ServiceDescription = service.ServiceDescription;

            // تحديث الصورة فقط إذا تم رفع واحدة جديدة
            if (serviceImage?.Length > 0)
            {
                using var br = new BinaryReader(serviceImage.OpenReadStream());
                existingService.ServiceImg = br.ReadBytes((int)serviceImage.Length);
                existingService.ServiceImgName = serviceImage.FileName;
                existingService.ServiceImgType = serviceImage.ContentType;
            }

            _serviceRepository.EditService(existingService);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var service = _serviceRepository.GetServiceById(id);
            if (service != null)
            {
                _serviceRepository.DeleteService(service.ServiceId);
                TempData["Deleted"] = "true"; // لتفعيل رسالة التأكيد بعد الحذف
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
