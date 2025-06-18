using CareerBuilderX.Data;
using CareerBuilderX.Models;
using CareerBuilderX.Repositories.Interfaces;

namespace CareerBuilderX.Repositories.Repo
{
    public class ServiceRepository : IServiceRepository
    {
        private ApplicationDbContext _context { get; set; }

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Service> GetAllServices()
        {
            return _context.Services.Where(s => s.IsDeleted == false).ToList();
        }

        public Service GetServiceById(int id)
        {
            return _context.Services.SingleOrDefault(x=> x.ServiceId == id);
        }

        public void CreateService(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public void EditService(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
        }

        public void DeleteService(int ServiceId)
        {
            var service = GetServiceById(ServiceId);
            if (service != null)
            {
                service.IsDeleted = true;
            }
            _context.SaveChanges();
        }
    }
}
