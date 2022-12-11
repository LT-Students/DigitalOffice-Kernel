using System;
using System.Threading.Tasks;
using DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Constants;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace LT.DigitalOffice.Kernel.UnitTests.AccessValidatorEngine
{
  public class AccessValidatorTests
  {
    private Mock<IRequestClient<ICheckUserIsAdminRequest>> _requestClientUSMock;
    private Mock<IRequestClient<ICheckUserRightsRequest>> _requestClientCRSMock;
    private Mock<IRequestClient<ICheckUserAnyRightRequest>> _requestClientARMock;
    private Mock<IRequestClient<ICheckProjectManagerRequest>> _requestClientPM;
    private Mock<IRequestClient<ICheckDepartmentManagerRequest>> _requestClientDM;
    private Mock<Response<IOperationResult<bool>>> _isAdminBrokerResponseMock;
    private Mock<Response<IOperationResult<bool>>> _hasRightsBrokerResponseMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<ILogger<AccessValidator>> _loggerMock;
    private Mock<IOperationResult<bool>> _isAdminResultMock;
    private Mock<IOperationResult<bool>> _hasRightsResultMock;
    private Mock<HttpContext> _httpContextMock;

    private Guid _userId;
    private IAccessValidator _accessValidator;

    private int[] RightIds = new[] { 5, 4 };

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
      _requestClientARMock = new Mock<IRequestClient<ICheckUserAnyRightRequest>>();

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
        _requestClientUSMock.Object,
        _requestClientARMock.Object,
        _requestClientPM.Object,
        _requestClientDM.Object);
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
    public async Task ShouldReturnTrueWhenUserIsAdmin()
    {
      ConfigureIsAdminResult(true, true);

      Assert.IsTrue(await _accessValidator.IsAdminAsync());
      Assert.IsTrue(await _accessValidator.IsAdminAsync(_userId));
    }

    [Test]
    public async Task ShouldReturnFalseWhenUserIsNotAdmin()
    {
      ConfigureIsAdminResult(true, false);

      Assert.IsFalse(await _accessValidator.IsAdminAsync());
      Assert.IsFalse(await _accessValidator.IsAdminAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task ShouldThrowExceptionWhenUserServiceConsumerRespondsWithErrors()
    {
      _isAdminBrokerResponseMock
        .Setup(x => x.Message)
        .Returns((IOperationResult<bool>)null);

      Assert.False(await _accessValidator.IsAdminAsync());
    }

    [Test]
    public void ShouldThrowExceptionWhenRequestedIdsListIsNullOrEmpty()
    {
      Assert.ThrowsAsync<ArgumentException>(() => _accessValidator.HasRightsAsync());
      Assert.ThrowsAsync<ArgumentException>(() => _accessValidator.HasRightsAsync(null, true, null));
      Assert.ThrowsAsync<ArgumentException>(() => _accessValidator.HasRightsAsync(null, true));
    }

    [Test]
    public async Task ShouldReturnTrueWhenUserHasRights()
    {
      ConfigureIsAdminResult(true, false);
      ConfigureHasRightsResult(true, true);

      Assert.IsTrue(await _accessValidator.HasRightsAsync(RightIds));
      Assert.IsTrue(await _accessValidator.HasRightsAsync(null, RightIds));
      Assert.IsTrue(await _accessValidator.HasRightsAsync(null, true, RightIds));
      Assert.IsTrue(await _accessValidator.HasRightsAsync(null, false, RightIds));
      Assert.IsTrue(await _accessValidator.HasRightsAsync(_userId, true, RightIds));
      Assert.IsTrue(await _accessValidator.HasRightsAsync(_userId, false, RightIds));

      ConfigureIsAdminResult(true, true);
      ConfigureHasRightsResult(true, false);

      Assert.IsTrue(await _accessValidator.HasRightsAsync(RightIds));
      Assert.IsTrue(await _accessValidator.HasRightsAsync(null, true, RightIds));
      Assert.IsTrue(await _accessValidator.HasRightsAsync(_userId, true, RightIds));
    }

    [Test]
    public async Task ShouldReturnFalseWhenUserDoesntHaveRights()
    {
      ConfigureIsAdminResult(true, false);
      ConfigureHasRightsResult(true, false);

      Assert.IsFalse(await _accessValidator.HasRightsAsync(RightIds));
      Assert.IsFalse(await _accessValidator.HasRightsAsync(null, true, RightIds));
      Assert.IsFalse(await _accessValidator.HasRightsAsync(Guid.NewGuid(), true, RightIds));
    }

    [Test]
    public async Task ShouldThrowExceptionWhenCheckRightsServiceConsumerRespondsWithErrors()
    {
      ConfigureIsAdminResult(true, false);
      _hasRightsBrokerResponseMock
        .Setup(x => x.Message)
        .Returns((IOperationResult<bool>)null);

      Assert.False(await _accessValidator.HasRightsAsync(null, true, RightIds));
    }

    [Test]
    public void ShouldThrowFormatExceptionWhenThereIsInvalidGuidInHeaders()
    {
      string text = "Not GUID text.";

      _httpContextMock
        .Setup(x => x.Items[ConstStrings.UserId])
        .Returns(text);

      Assert.ThrowsAsync<InvalidCastException>(
        () => _accessValidator.IsAdminAsync(),
        $"UserId '{text}' value in HttpContext is not in Guid format.");

      Assert.ThrowsAsync<InvalidCastException>(
        () => _accessValidator.HasRightsAsync(null, true, RightIds),
        $"UserId '{text}' value in HttpContext is not in Guid format.");
    }

    [Test]
    public void ShouldThrowNullReferenceExceptionWhenThereIsNoUserIdInHeaders()
    {
      _httpContextMock
        .Setup(x => x.Items[ConstStrings.UserId])
        .Returns(null);

      Assert.ThrowsAsync<ArgumentException>(
        () => _accessValidator.IsAdminAsync(),
        "UserId value in HttpContext is empty.");

      Assert.ThrowsAsync<ArgumentException>(
        () => _accessValidator.HasRightsAsync(null, true, RightIds),
        "UserId value in HttpContext is empty.");
    }

    [Test]
    public void HttpContextNotContainUserId()
    {
      _httpContextMock
        .Setup(x => x.Items.ContainsKey(ConstStrings.UserId))
        .Returns(false);

      Assert.ThrowsAsync<ArgumentNullException>(
        () => _accessValidator.IsAdminAsync(),
        "HttpContext does not contain UserId.");

      Assert.ThrowsAsync<ArgumentNullException>(
        () => _accessValidator.HasRightsAsync(null, true, RightIds),
        "HttpContext does not contain UserId.");
    }
  }
}
