using LT.DigitalOffice.Kernel.AccessValidatorEngine;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.UnitTests.AccessValidatorEngine
{
    public class AccessValidatorTests
    {
        private Mock<IRequestClient<ICheckUserIsAdminRequest>> _requestClientUSMock;
        private Mock<IRequestClient<ICheckUserRightsRequest>> _requestClientCRSMock;
        private Mock<Response<IOperationResult<bool>>> _isAdminBrokerResponseMock;
        private Mock<Response<IOperationResult<bool>>> _hasRightsBrokerResponseMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<ILogger<AccessValidator>> _loggerMock;
        private Mock<IOperationResult<bool>> _isAdminResultMock;
        private Mock<IOperationResult<bool>> _hasRightsResultMock;
        private Mock<HttpContext> _httpContextMock;

        private Guid _userId;
        private IAccessValidator _accessValidator;

        private int[] RightIds = new [] { 5, 4 };

        private void ConfigureIsAdminResult(bool isSuccess, bool body)
        {
            _isAdminResultMock
                .Setup(x => x.IsSuccess)
                .Returns(isSuccess);

            _isAdminResultMock
                .Setup(x => x.Body)
                .Returns(body);
        }

        private void ConfigureHasRightsResult(bool isSuccess, bool body)
        {
            _hasRightsResultMock
                .Setup(x => x.IsSuccess)
                .Returns(isSuccess);

            _hasRightsResultMock
                .Setup(x => x.Body)
                .Returns(body);
        }

        private void BrokerSetUp()
        {
            _isAdminResultMock = new Mock<IOperationResult<bool>>();
            _hasRightsResultMock = new Mock<IOperationResult<bool>>();

            _requestClientUSMock = new Mock<IRequestClient<ICheckUserIsAdminRequest>>();
            _requestClientCRSMock = new Mock<IRequestClient<ICheckUserRightsRequest>>();

            _isAdminBrokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
            _isAdminBrokerResponseMock
                .Setup(x => x.Message)
                .Returns(_isAdminResultMock.Object);

            _hasRightsBrokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
            _hasRightsBrokerResponseMock
                .Setup(x => x.Message)
                .Returns(_hasRightsResultMock.Object);

            _requestClientUSMock
                .Setup(x => x.GetResponse<IOperationResult<bool>>(It.IsAny<object>(), default, It.IsAny<RequestTimeout>()))
                .Returns(Task.FromResult(_isAdminBrokerResponseMock.Object));

            _requestClientCRSMock
                .Setup(x => x.GetResponse<IOperationResult<bool>>(It.IsAny<object>(), default, It.IsAny<RequestTimeout>()))
                .Returns(Task.FromResult(_hasRightsBrokerResponseMock.Object));
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userId = Guid.NewGuid();

            BrokerSetUp();

            _loggerMock = new Mock<ILogger<AccessValidator>>();

            _httpContextMock = new Mock<HttpContext>();

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(_httpContextMock.Object);

            _accessValidator = new AccessValidator(
                _httpContextAccessorMock.Object,
                _loggerMock.Object,
                _requestClientCRSMock.Object,
                _requestClientUSMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _loggerMock.Reset();

            _httpContextMock
                .Setup(x => x.Items[ConstStrings.UserId])
                .Returns(_userId.ToString());

            _httpContextMock
                .Setup(x => x.Items.ContainsKey(ConstStrings.UserId))
                .Returns(true);
        }

        [Test]
        public void ShouldReturnTrueWhenUserIsAdmin()
        {
            ConfigureIsAdminResult(true, true);

            Assert.IsTrue(_accessValidator.IsAdmin());
            Assert.IsTrue(_accessValidator.IsAdmin(_userId));
        }

        [Test]
        public void ShouldReturnFalseWhenUserIsNotAdmin()
        {
            ConfigureIsAdminResult(true, false);

            Assert.IsFalse(_accessValidator.IsAdmin());
            Assert.IsFalse(_accessValidator.IsAdmin(Guid.NewGuid()));
        }

        [Test]
        public void ShouldThrowExceptionWhenUserServiceConsumerRespondsWithErrors()
        {
            _isAdminBrokerResponseMock
                .Setup(x => x.Message)
                .Returns((IOperationResult<bool>)null);

            Assert.False(_accessValidator.IsAdmin());
        }

        [Test]
        public void ShouldReturnTrueWhenUserHasRights()
        {
            ConfigureIsAdminResult(true, false);
            ConfigureHasRightsResult(true, true);

            Assert.IsTrue(_accessValidator.HasRights(RightIds));
            Assert.IsTrue(_accessValidator.HasRights(null, RightIds));
            Assert.IsTrue(_accessValidator.HasRights(_userId, RightIds));
        }

        [Test]
        public void ShouldReturnFalseWhenUserDoesntHaveRights()
        {
            ConfigureIsAdminResult(true, false);
            ConfigureHasRightsResult(true, false);

            Assert.IsFalse(_accessValidator.HasRights(RightIds));
            Assert.IsFalse(_accessValidator.HasRights(null, RightIds));
            Assert.IsFalse(_accessValidator.HasRights(Guid.NewGuid(), RightIds));
        }

        [Test]
        public void ShouldThrowExceptionWhenCheckRightsServiceConsumerRespondsWithErrors()
        {
            ConfigureIsAdminResult(true, false);
            _hasRightsBrokerResponseMock
                .Setup(x => x.Message)
                .Returns((IOperationResult<bool>)null);

            Assert.False(_accessValidator.HasRights(null, RightIds));
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
                () => _accessValidator.HasRights(null, RightIds),
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
                () => _accessValidator.HasRights(null, RightIds),
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
                () => _accessValidator.HasRights(null, RightIds),
                "HttpContext does not contain UserId.");
        }
    }
}