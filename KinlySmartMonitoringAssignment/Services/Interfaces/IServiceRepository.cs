using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinlySmartMonitoringAssignment.Models;

namespace KinlySmartMonitoringAssignment.Services.Interfaces
{
    public interface IServiceRepository
    {
        public Service InsertService(Service service);
        public bool ServiceNameExists(string serviceName);
        public void DeleteService(string serviceName);
        public object PatchService(string serviceName, Dictionary<string, object> attributes);
        public Service UpdateService(Service service);
        public IEnumerable<Service> GetServices();
        public Service GetService(string serviceName);
        public IEnumerable<Service> GetServicesByLabel(Label label);
    }
}
