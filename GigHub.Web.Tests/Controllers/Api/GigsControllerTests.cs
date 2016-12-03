using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

using GigHub.Web.Core;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.Repositories;
using GigHub.Web.Controllers.Api;
using GigHub.Web.Tests.Extensions;

namespace GigHub.Web.Tests.Controllers.Api
{
    public class GigsControllerTests
    {
        GigsController _controller;

        public GigsControllerTests()
        {
            var mockUserManager = new Mock<UserManager<ApplicationUser>>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IGigRepository>();

            mockUnitOfWork.SetupGet(u => u.Gigs).Returns(mockRepository.Object);
            
            _controller = 
                new GigsController(
                    mockUnitOfWork.Object,
                    mockUserManager.Object,
                    mockLoggerFactory.Object);
            _controller.MockCurrentUser("1", "user1@somewhere.com");
        }

        [Fact]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.Cancel("1");

            Assert.IsType<NotFoundResult>(result);
        }
/*
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
        */
    }
}
