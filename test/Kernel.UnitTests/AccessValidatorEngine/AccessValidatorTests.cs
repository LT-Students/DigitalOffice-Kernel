using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Constants;

namespace LT.DigitalOffice.Kernel.UnitTests.AccessValidatorEngine
{
    public class OperationResult<T> : IOperationResult<T>
    {
        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }

        public T Body { get; set; }
    }

    public class AccessValidatorTests
    {
        private Mock<IRequestClient<IAccessValidatorUserServiceRequest>> _requestClientUSMock;
        private Mock<IRequestClient<IAccessValidatorCheckRightsServiceRequest>> _requestClientCRSMock;
        private Mock<Response<IOperationResult<bool>>> _responseBrokerMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<HttpContext> _httpContextMock;

        private string _userId;
        private IAccessValidator _accessValidator;

        private const int RightId = 5;

        private OperationResult<bool> _operationResult = new OperationResult<bool>();

        private void BrokerSetUp()
        {
            _requestClientUSMock = new Mock<IRequestClient<IAccessValidatorUserServiceRequest>>();
            _requestClientCRSMock = new Mock<IRequestClient<IAccessValidatorCheckRightsServiceRequest>>();

            _responseBrokerMock = new Mock<Response<IOperationResult<bool>>>();
            _responseBrokerMock
                .Setup(x => x.Message)
                .Returns(_operationResult);

            _requestClientUSMock
                .Setup(x => x.GetResponse<IOperationResult<bool>>(It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(_responseBrokerMock.Object));

            _requestClientCRSMock
                .Setup(x => x.GetResponse<IOperationResult<bool>>(It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(_responseBrokerMock.Object));
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userId = Guid.NewGuid().ToString();

            BrokerSetUp();

            _httpContextMock = new Mock<HttpContext>();

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(_httpContextMock.Object);

            _accessValidator = new AccessValidator(
                _httpContextAccessorMock.Object,
                _requestClientCRSMock.Object,
                _requestClientUSMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _httpContextMock
                .Setup(x => x.Items[ConstStrings.UserId])
                .Returns(_userId);

            _httpContextMock
                .Setup(x => x.Items.ContainsKey(ConstStrings.UserId))
                .Returns(true);
        }

        [Test]
        public void ShouldReturnTrueWhenUserIsAdmin()
        {
            _operationResult.IsSuccess = true;
            _operationResult.Body = true;

            Assert.IsTrue(_accessValidator.IsAdmin());
        }

        [Test]
        public void ShouldReturnFalseWhenUserIsNotAdmin()
        {
            _operationResult.IsSuccess = true;
            _operationResult.Body = false;

            Assert.IsFalse(_accessValidator.IsAdmin());
        }

        [Test]
        public void ShouldThrowExceptionWhenUserServiceConsumerRespondsWithErrors()
        {
            _responseBrokerMock
                .Setup(x => x.Message)
                .Returns((IOperationResult<bool>)null);

            Assert.Throws<NullReferenceException>(
                () => _accessValidator.IsAdmin(),
                "Failed to send request to UserService via the broker.");
        }

        [Test]
        public void ShouldReturnTrueWhenUserHasRights()
        {
            _operationResult.IsSuccess = true;
            _operationResult.Body = true;

            Assert.IsTrue(_accessValidator.HasRight(RightId));
        }

        [Test]
        public void ShouldReturnFalseWhenUserDoesntHaveRights()
        {
            _operationResult.IsSuccess = true;
            _operationResult.Body = false;

            Assert.IsFalse(_accessValidator.HasRight(RightId));
        }

        [Test]
        public void ShouldThrowExceptionWhenCheckRightsServiceConsumerRespondsWithErrors()
        {
            _responseBrokerMock
                .Setup(x => x.Message)
                .Returns((IOperationResult<bool>)null);

            Assert.Throws<NullReferenceException>(
                () => _accessValidator.HasRight(RightId),
                "Failed to send request to CheckRightService via the broker.");
        }

        [Test]
        public void ShouldThrowFormatExceptionWhenThereIsInvalidGuidInHeaders()
        {
            string text = "Not GUID text.";

            _httpContextMock
                .Setup(x => x.Items[ConstStrings.UserId])
                .Returns(text);

            Assert.Throws<InvalidCastException>(
                () => _accessValidator.IsAdmin(),
                $"UserId '{text}' value in HttpContext is not in Guid format.");
            Assert.Throws<InvalidCastException>(
                () => _accessValidator.HasRight(RightId),
                $"UserId '{text}' value in HttpContext is not in Guid format.");
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenThereIsNoUserIdInHeaders()
        {
            _httpContextMock
                .Setup(x => x.Items[ConstStrings.UserId])
                .Returns(null);

            Assert.Throws<ArgumentException>(
                () => _accessValidator.IsAdmin(),
                "UserId value in HttpContext is empty.");
            Assert.Throws<ArgumentException>(
                () => _accessValidator.HasRight(RightId),
                "UserId value in HttpContext is empty.");
        }

        [Test]
        public void HttpContextNotContainUserId()
        {
            _httpContextMock
                .Setup(x => x.Items.ContainsKey(ConstStrings.UserId))
                .Returns(false);

            Assert.Throws<ArgumentNullException>(
                () => _accessValidator.IsAdmin(),
                "HttpContext does not contain UserId.");
            Assert.Throws<ArgumentNullException>(
                () => _accessValidator.HasRight(RightId),
                "HttpContext does not contain UserId.");
        }
    }
}