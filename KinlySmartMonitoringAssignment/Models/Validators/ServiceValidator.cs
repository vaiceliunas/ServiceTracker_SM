using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KinlySmartMonitoringAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestApi.Models;

namespace KinlySmartMonitoringAssignment.Models.Validators
{
    public class ServiceValidator : IServiceValidator
    {
        public readonly IServiceRepository Repository;
        public readonly string[] PatchableAttributes = new string[] {"port", "maintainerEmail" };

        public ServiceValidator(IServiceRepository repository)
        {
            Repository = repository;
        }

        public bool IsNameValid(string serviceName, bool hasToExist, ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                modelState.AddModelError("Name", "name was null or empty");
                return false;
            }

            if (serviceName.Length < 4)
            {
                modelState.AddModelError("Name", "name is less than 4 characters {" + serviceName + "}");
                return false;
            }

            if (serviceName.Length > 30)
            {
                modelState.AddModelError("Name", "name is more than 30 characters {" + serviceName + "}");
                return false;
            }

            var serviceNameExists = Repository.ServiceNameExists(serviceName);

            if (hasToExist && !serviceNameExists)
            {
                modelState.AddModelError("Name", "name doesn't exist {" + serviceName + "}");
                return false;
            }

            if (!hasToExist && serviceNameExists)
            {
                modelState.AddModelError("Name", "name already exists {" + serviceName + "}");
                return false;
            }

            return true;
        }

        public bool IsPortValid(int portNumber, ModelStateDictionary modelState)
        {
            if (portNumber < 1 || portNumber > 65535)
            {
                modelState.AddModelError("Port", "Invalid port number {" + portNumber + "}");
                return false;
            }

            return true;
        }

        public bool IsEmailValid(string email, ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(email))
            {
                modelState.AddModelError("Email", "email was null or empty");
                return false;
            }

            if (!new EmailAddressAttribute().IsValid(email))
            {
                modelState.AddModelError("Email", "was recognized as invalid {" + email + "}");
                return false;
            }

            return true;
        }

        public bool AreLabelsValid(ICollection<Label> labels, ModelStateDictionary modelState)
        {
            if (labels == null)
            {
                modelState.AddModelError("Labels", "No labels found");
                return false;
            }

            foreach (var label in labels)
            {
                if (string.IsNullOrEmpty(label.LabelKey) || string.IsNullOrEmpty(label.LabelValue))
                {
                    modelState.AddModelError("Label key value pair", "Key/value pair had null or empty value {" + 
                                                                     (string.IsNullOrEmpty(label.LabelKey) ? string.Empty : label.LabelKey) +
                                                                     "/" +
                                                                     (string.IsNullOrEmpty(label.LabelValue) ? string.Empty : label.LabelValue) + "}");
                }
            }

            return modelState.ErrorCount > 0;
        }

        public bool IsIdentityNotSet(int id, ModelStateDictionary modelState)
        {
            if (id != 0)
            {
                modelState.AddModelError("Id", "Cannot set id on insert {" + id + "}");
                return false;
            }

            return true;
        }

        public bool AreAttributesValid(Dictionary<string, object> attributes, ModelStateDictionary modelState)
        {
            if (attributes == null || attributes.Count == 0)
            {
                modelState.AddModelError("Attributes", "No attributes found");
                return false;
            }

            foreach (var attr in attributes)
            {
                if(!PatchableAttributes.Contains(attr.Key))
                    modelState.AddModelError("Attributes", "Attribute to patch was not recognized {" + attr.Key + "}");

                if (attr.Key == "port")
                {
                    if (int.TryParse(attr.Value.ToString(), out var outPort))
                        IsPortValid(outPort, modelState);
                    else 
                        modelState.AddModelError("Attributes", "Unable to parse port attribute {" + attr.Value + "}");
                }

                if (attr.Key == "maintainerEmail")
                    IsEmailValid(attr.Value.ToString(), modelState);
            }

            return modelState.ErrorCount > 0;
        }

        public bool IsKeyValuePairValid(string key, string value, ModelStateDictionary modelState)
        {
            if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                modelState.AddModelError("Key/Value pair for label", "Unable to parse key/value pair {" + key + "/" + value + "}");
                return false;
            }

            return true;
        }
    }
}
