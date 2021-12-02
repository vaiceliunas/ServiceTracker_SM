using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinlySmartMonitoringAssignment.Models;
using KinlySmartMonitoringAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace KinlySmartMonitoringAssignment.Services
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly FoobarServicesContext _context;

        public ServiceRepository(FoobarServicesContext _ctx)
        {
            _context = _ctx;
        }

        public Service InsertService(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
            return service;
        }

        public bool ServiceNameExists(string serviceName)
        {
            var res = _context.Services.Any(t => t.Name == serviceName);
            return res;
        }

        public void DeleteService(string serviceName)
        {
            var obj = _context.Services.FirstOrDefault(t => t.Name == serviceName);
            if (obj != null) _context.Services.Remove(obj);
            _context.SaveChanges();
        }

        public object PatchService(string serviceName, Dictionary<string, object> attributes)
        {
            var newObj = _context.Services.Include(t=> t.Labels).First(t => t.Name == serviceName);
            var oldObj = new Service()
            {
                Id = newObj.Id,
                Port = newObj.Port,
                MaintainerEmail = newObj.MaintainerEmail,
                Name = newObj.Name,
                Labels = newObj.Labels
            };
            if (attributes.Any(t => t.Key == "port"))
            {
                newObj.Port = int.Parse(attributes.First(t => t.Key == "port").Value.ToString() ?? string.Empty);
            }
            if (attributes.Any(t => t.Key == "maintainerEmail"))
            {
                newObj.MaintainerEmail = attributes.First(t => t.Key == "maintainerEmail").Value.ToString();
            }

            _context.Attach(newObj);
            if(newObj.Port > 0)
                _context.Entry(newObj).Property(t => t.Port).IsModified = true;
            if(!string.IsNullOrEmpty(newObj.MaintainerEmail))
                _context.Entry(newObj).Property(t => t.MaintainerEmail).IsModified = true;
            _context.SaveChanges();

            return new
            {
                oldObj = oldObj,
                newObj = newObj
            };
        }

        public Service UpdateService(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
            return service;
        }

        public IEnumerable<Service> GetServices()
        {
            var result = _context.Services.Include(t=> t.Labels).ToList();
            return result;
        }

        public Service GetService(string serviceName)
        {
            var result = _context.Services.Include(t => t.Labels).FirstOrDefault(t => t.Name == serviceName);
            return result;
        }

        public IEnumerable<Service> GetServicesByLabel(Label label)
        {
            var labels = _context.Labels
                .Where(t => t.LabelKey == label.LabelKey && t.LabelValue == label.LabelValue).ToList();
            var serviceIds = labels.Select(t => t.ServiceId).Distinct().ToList();
            var result = _context.Services.Where(t => serviceIds.Contains(t.Id)).ToList();
            return result;
        }
    }
}
