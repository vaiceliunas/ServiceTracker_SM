using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KinlySmartMonitoringAssignment.Models.Validators
{
    public interface IServiceValidator
    {
        public bool IsNameValid(string serviceName, bool hasToExist, ModelStateDictionary modelState);
        public bool IsPortValid(int portNumber, ModelStateDictionary modelState);
        public bool IsEmailValid(string email, ModelStateDictionary modelState);
        public bool AreLabelsValid(ICollection<Label> labels, ModelStateDictionary modelState);
        public bool AreAttributesValid(Dictionary<string, object> attributes, ModelStateDictionary modelState);

        public bool IsKeyValuePairValid(string key, string value, ModelStateDictionary modelState);
    }
}
