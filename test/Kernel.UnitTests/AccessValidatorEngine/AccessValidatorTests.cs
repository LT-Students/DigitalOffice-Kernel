﻿using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
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
        private Mock<IRequestClient<IAccessValidatorUserServiceRequest>> requestClientUSMock;
        private Mock<IRequestClient<IAccessValidatorCheckRightsServiceRequest>> requestClientCRSMock;
        private Mock<Response<IOperationResult<bool>>> responseBrokerMock;
        private Mock<IHttpContextAccessor> httpContextMock;

        private string userId;
        private IAccessValidator accessValidator;

        private const int RIGHT_ID = 5;

        private OperationResult<bool> operationResult;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            userId = Guid.NewGuid().ToString();

            BrokerSetUp();

            httpContextMock = new Mock<IHttpContextAccessor>();

            accessValidator = new AccessValidator(
                httpContextMock.Object,
                requestClientCRSMock.Object,
                requestClientUSMock.Object);
        }

        public void BrokerSetUp()
        {
            requestClientUSMock = new Mock<IRequestClient<IAccessValidatorUserServiceRequest>>();
            requestClientCRSMock = new Mock<IRequestClient<IAccessValidatorCheckRightsServiceRequest>>();

            responseBrokerMock = new Mock<Response<IOperationResult<bool>>>();

            requestClientUSMock.Setup(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(responseBrokerMock.Object));

            requestClientCRSMock.Setup(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(responseBrokerMock.Object));
        }

        [SetUp]
        public void SetUp()
        {
            operationResult = new OperationResult<bool>
            {
                IsSuccess = true,
                Errors = new List<string>(),
                Body = new bool()
            };

            responseBrokerMock
                .SetupGet(x => x.Message)
                .Returns(operationResult);
        }

        [Test]
        public void ShouldReturnTrueWhenUserIsAdmin()
        {
            operationResult.IsSuccess = true;
            operationResult.Body = true;

            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns(userId);

            var result = accessValidator.IsAdmin();

            Assert.AreEqual(true, result);
        }

        [Test]
        public void ShouldReturnFalseWhenUserIsNotAdmin()
        {
            operationResult.IsSuccess = true;
            operationResult.Body = false;

            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns(userId);

            var result = accessValidator.IsAdmin();

            Assert.AreEqual(false, result);
        }

        [Test]
        public void ShouldThrowExceptionWhenUserServiceConsumerRespondsWithErrors()
        {
            operationResult = null;

            responseBrokerMock
                .SetupGet(x => x.Message)
                .Returns(operationResult);

            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns(userId);

            Assert.Throws<NullReferenceException>(
                () => accessValidator.IsAdmin(),
                "Failed to send request via the broker.");
        }

        [Test]
        public void ShouldReturnTrueWhenUserHasRights()
        {
            operationResult.IsSuccess = true;
            operationResult.Body = true;

            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns(userId);

            var result = accessValidator.HasRights(RIGHT_ID);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void ShouldReturnFalseWhenUserDoesntHaveRights()
        {
            operationResult.IsSuccess = true;
            operationResult.Body = false;

            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns(userId);

            var result = accessValidator.HasRights(RIGHT_ID);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void ShouldThrowExceptionWhenCheckRightsServiceConsumerRespondsWithErrors()
        {
            operationResult = null;

            responseBrokerMock
                .SetupGet(x => x.Message)
                .Returns(operationResult);

            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns(userId);

            Assert.Throws<NullReferenceException>(
                () => accessValidator.HasRights(RIGHT_ID),
                "Failed to send request via the broker.");
        }

        [Test]
        public void ShouldThrowFormatExceptionWhenThereIsInvalidGuidInHeaders()
        {
            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns("SampleText");

                Assert.Throws<BadRequestException>(() => accessValidator.IsAdmin());
                Assert.Throws<BadRequestException>(() => accessValidator.HasRights(RIGHT_ID));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenThereIsNoUserIdInHeaders()
        {
            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns<StringValues>(null);

            Assert.Throws<BadRequestException>(() => accessValidator.IsAdmin());
            Assert.Throws<BadRequestException>(() => accessValidator.HasRights(RIGHT_ID));
        }

        [Test]
        public void ShouldThrowListenerExceptionWhenThereIsMoreThanOneUserIdInHeaders()
        {
            var stringValues = new StringValues(new string[]
            {
                "Guid1", "Guid2"
            });

            httpContextMock
                .Setup(h => h.HttpContext.Request.Headers["UserId"])
                .Returns(stringValues);

            Assert.Throws<BadRequestException>(() => accessValidator.IsAdmin());
            Assert.Throws<BadRequestException>(() => accessValidator.HasRights(RIGHT_ID));
        }
    }
}