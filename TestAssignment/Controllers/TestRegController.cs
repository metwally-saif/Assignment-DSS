using Assignment_DSS.controllers;
using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignment.Controllers
{
    public class TestRegController
    {
        private readonly RegController _controller;
        private readonly Mock<IUserRepo> _mockRepo;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<ISession> _sessionMock;

        public TestRegController()
        {
            _mockRepo = new Mock<IUserRepo>();
            _controller = new RegController(_mockRepo.Object);
            _httpContextMock = new Mock<HttpContext>();
            _sessionMock = new Mock<ISession>();
        }

        [Fact]
        public async Task Reg_ReturnsViewResult_WhenGetMethodIsCalled()
        {
            // Arrange
            var model = new User();

            _httpContextMock.Setup(c => c.Session).Returns(_sessionMock.Object);

            var tempData = new TempDataDictionary(_httpContextMock.Object, Mock.Of<ITempDataProvider>());

            _controller.TempData = tempData;

            // Act
            var result = await _controller.Reg(model.Name, model.Password);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Reg_ReturnsViewResultWithModelError_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new User();
            _controller.ModelState.AddModelError("Name", "Name is required.");

            _httpContextMock.Setup(c => c.Session).Returns(_sessionMock.Object);

            var tempData = new TempDataDictionary(_httpContextMock.Object, Mock.Of<ITempDataProvider>());

            _controller.TempData = tempData;

            // Act
            var result = await _controller.Reg(model.Name, model.Password);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(1, _controller.ModelState.ErrorCount);
        }
    }
}
