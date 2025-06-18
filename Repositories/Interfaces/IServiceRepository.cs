using CareerBuilderX.Models;

namespace CareerBuilderX.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        public Service GetServiceById(int id);

        public List<Service> GetAllServices();

        public void CreateService(Service service);

        public void EditService(Service service);

        public void DeleteService(int ServiceId);
    }
}
