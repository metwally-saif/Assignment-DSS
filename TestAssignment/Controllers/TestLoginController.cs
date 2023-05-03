using Assignment_DSS.controllers;
using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using AutoFixture.Xunit2;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestAssignment.Controllers
{

    public class TestLoginController
    {
        private readonly Mock<IUserRepo> _mockUserRepo;
        private readonly Mock<HttpContext> _mockHttpContext;

        public TestLoginController()
        {
            _mockUserRepo = new Mock<IUserRepo>();
            _mockHttpContext = new Mock<HttpContext>();
        }

        [Fact]
        public async Task Login_InvalidUser_ReturnsView()
        {
            // Arrange
            _mockUserRepo.Setup(repo => repo.GetByNameAndPass(It.IsAny<User>()))
                .ReturnsAsync(new List<User>());

            _mockHttpContext.Setup(context => context.Session)
                .Returns(new Mock<ISession>().Object);

            var controller = new LoginController(_mockUserRepo.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object
                }
            };

            var name = "invaliduser";
            var password = "invalidpassword";

            // Act
            var result = await controller.Login(name, password) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Logout_ClearsSession_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.Clear());

            _mockHttpContext.Setup(context => context.Session)
                .Returns(mockSession.Object);

            var controller = new LoginController(null)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _mockHttpContext.Object
                }
            };

            // Act
            var result =  controller.Logout() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Home", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }
    }
}

