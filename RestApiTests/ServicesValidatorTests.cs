using KinlySmartMonitoringAssignment.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using KinlySmartMonitoringAssignment.Controllers;
using KinlySmartMonitoringAssignment.Models;
using KinlySmartMonitoringAssignment.Models.Validators;
using KinlySmartMonitoringAssignment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;

namespace RestApiTests
{
    public class ServicesValidatorTests
    {
        private readonly IServiceValidator _validator;
        private readonly Mock<IServiceRepository> _serviceRepMock = new();

        public ServicesValidatorTests()
        {
            _validator = new ServiceValidator(_serviceRepMock.Object);
        }

        [Fact]
        public void IsNameValid_WithNameTooShort_ReturnsInvalidModel()
        {
            //Arrange
            var serviceName = "aa";
            var model = new ModelStateDictionary();

            //Act
            _validator.IsNameValid(serviceName, false, model);
            
            //Assert
            Assert.True(!model.IsValid);
        }


        [Fact]
        public void IsNameValid_WithNameTooLong_ReturnsInvalidModel()
        {
            //Arrange
            var serviceName = "service name which exceeds 30 symbol count";
            var model = new ModelStateDictionary();

            //Act
            _validator.IsNameValid(serviceName, false, model);

            //Assert
            Assert.True(!model.IsValid);
        }

        [Fact]
        public void IsNameValid_WithRequiredNameWhichDoesntExist_ReturnsInvalidModel()
        {
            //Arrange
            var serviceName = "legit service name";
            var model = new ModelStateDictionary();
            _serviceRepMock.Setup(o => o.ServiceNameExists(serviceName)).Returns(false);

            //Act
            _validator.IsNameValid(serviceName, true, model);

            //Assert
            Assert.True(!model.IsValid);
        }

        [Fact]
        public void IsNameValid_WithRequiredNameWhichExists_ReturnsValidModel()
        {
            //Arrange
            var serviceName = "legit service name";
            var model = new ModelStateDictionary();
            _serviceRepMock.Setup(o => o.ServiceNameExists(serviceName)).Returns(true);

            //Act
            _validator.IsNameValid(serviceName, true, model);

            //Assert
            Assert.True(model.IsValid);
        }

        [Fact]
        public void IsPortValid_WithInvalidPort_ReturnsInvalidModel()
        {
            //Arrange
            var invalidPort = 860355;
            var model = new ModelStateDictionary();

            //Act
            _validator.IsPortValid(invalidPort, model);

            //Assert
            Assert.True(!model.IsValid);
        }

        [Fact]
        public void IsPortValid_WithValidPort_ReturnsValidModel()
        {
            //Arrange
            var validPort = 5065;
            var model = new ModelStateDictionary();

            //Act
            _validator.IsPortValid(validPort, model);

            //Assert
            Assert.True(model.IsValid);
        }

        [Fact]
        public void IsEmailValid_WithInvalidEmail_ReturnsInValidModel()
        {
            //Arrange
            var invalidEmail = "someinvalidEmail";
            var model = new ModelStateDictionary();

            //Act
            _validator.IsEmailValid(invalidEmail, model);

            //Assert
            Assert.True(!model.IsValid);
        }

        [Fact]
        public void IsEmailValid_WithValidEmail_ReturnsValidModel()
        {
            //Arrange
            var invalidEmail = "somevalidemail@gmail.com";
            var model = new ModelStateDictionary();

            //Act
            _validator.IsEmailValid(invalidEmail, model);

            //Assert
            Assert.True(model.IsValid);
        }

        [Fact]
        public void AreAttributesValid_WithInvalidAttributes_ReturnsInValidModel()
        {
            //Arrange
            var invalidAttributes = new Dictionary<string, object>()
            {
                {"portas", 506},
                {"emailas", "mailas"},
            };
            var model = new ModelStateDictionary();

            //Act
            _validator.AreAttributesValid(invalidAttributes, model);

            //Assert
            Assert.True(!model.IsValid);
        }

        [Fact]
        public void AreAttributesValid_WithMixedAttributes_ReturnsInValidModel()
        {
            //Arrange
            var invalidAttributes = new Dictionary<string, object>()
            {
                {"port", 506},
                {"emailas", "mailas"},
            };
            var model = new ModelStateDictionary();

            //Act
            _validator.AreAttributesValid(invalidAttributes, model);

            //Assert
            Assert.True(!model.IsValid);
        }


        [Fact]
        public void AreAttributesValid_WithValidAttributes_ReturnsValidModel()
        {
            //Arrange
            var invalidAttributes = new Dictionary<string, object>()
            {
                {"port", 506},
                {"maintainerEmail", "mailasgkm@mail.com"},
            };
            var model = new ModelStateDictionary();

            //Act
            _validator.AreAttributesValid(invalidAttributes, model);

            //Assert
            Assert.True(model.IsValid);
        }

        [Fact]
        public void IsKeyValuePairValid_WithInvalidKey_ReturnsInValidModel()
        {
            //Arrange
            var key = string.Empty;
            var value = "someValue";
            var model = new ModelStateDictionary();

            //Act
            _validator.IsKeyValuePairValid(key, value, model);

            //Assert
            Assert.True(!model.IsValid);
        }

        [Fact]
        public void IsKeyValuePairValid_WithInvalidValue_ReturnsInValidModel()
        {
            //Arrange
            var key = "SomeValue";
            var value = string.Empty;
            var model = new ModelStateDictionary();

            //Act
            _validator.IsKeyValuePairValid(key, value, model);

            //Assert
            Assert.True(!model.IsValid);
        }

        [Fact]
        public void IsKeyValuePairValid_WithValidPair_ReturnsValidModel()
        {
            //Arrange
            var key = "SomeValue";
            var value = "SomeValue";
            var model = new ModelStateDictionary();

            //Act
            _validator.IsKeyValuePairValid(key, value, model);

            //Assert
            Assert.True(model.IsValid);
        }
    }
}
