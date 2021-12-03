using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinlySmartMonitoringAssignment.Models;
using KinlySmartMonitoringAssignment.Models.Validators;
using KinlySmartMonitoringAssignment.Services.Interfaces;
using RestApi.Models;

namespace KinlySmartMonitoringAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _serviceRep;
        private readonly IServiceValidator _serviceVal;

        public ServicesController(IServiceRepository serviceRep, IServiceValidator serviceVal)
        {
            _serviceRep = serviceRep;
            _serviceVal = serviceVal;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Service>> Get()
        {
            var result  = _serviceRep.GetServices();
            return Ok(result);
        }

        [HttpGet("{serviceName}")]
        public ActionResult<Service> Get(string serviceName)
        {
            _serviceVal.IsNameValid(serviceName, true, ModelState);

            if (ModelState.ErrorCount > 0)
                return BadRequest(ModelState);

            var result = _serviceRep.GetService(serviceName);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("byLabel")]
        public ActionResult<Service> GetWithLabels(string key, string value)
        {
            _serviceVal.IsKeyValuePairValid(key, value, ModelState);
            if (ModelState.ErrorCount > 0)
                return BadRequest(ModelState);

            var labelParam = new Label()
            {
                LabelKey = key,
                LabelValue = value
            };

            var result = _serviceRep.GetServicesByLabel(labelParam);
            if(!result.Any())
                return NotFound();

            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] Service service)
        {
            if (service == null)
                return BadRequest();

            _serviceVal.IsEmailValid(service.MaintainerEmail, ModelState);
            _serviceVal.IsNameValid(service.Name, false, ModelState);
            _serviceVal.IsPortValid(service.Port, ModelState);
            _serviceVal.AreLabelsValid(service.Labels, ModelState);

            if(ModelState.ErrorCount > 0)
                return BadRequest(ModelState);

            var res = _serviceRep.InsertService(service);

            return Ok(res);
        }

        [HttpPatch]
        public ActionResult Patch(string serviceName, [FromBody] Dictionary<string, object> attributes)
        {
            _serviceVal.IsNameValid(serviceName, true, ModelState);

            if (ModelState.ErrorCount > 0)
                return BadRequest(ModelState);

            _serviceVal.AreAttributesValid(attributes, ModelState);

            if (ModelState.ErrorCount > 0)
                return BadRequest(ModelState);

            var result = _serviceRep.PatchService(serviceName, attributes);

            return Ok(result);
        }

        [HttpDelete]
        public ActionResult Delete(string serviceName)
        {
            _serviceVal.IsNameValid(serviceName, true, ModelState);

            if (ModelState.ErrorCount > 0)
                return BadRequest(ModelState);

            _serviceRep.DeleteService(serviceName);

            return Ok();


        }
    }
}
