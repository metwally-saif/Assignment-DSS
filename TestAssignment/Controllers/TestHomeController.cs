using Assignment_DSS.controllers;
using Assignment_DSS.Interfaces;
using Assignment_DSS.modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignment.Controllers
{
    public class TestHomeController
    {
        private readonly Mock<IPostsRep> _postsRepMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IUserRepo> _usersRepoMock;
        private readonly Mock<ICommentsRep> _commentRepMock;

        private readonly HomeController _controller;

        public TestHomeController()
        {
            _postsRepMock = new Mock<IPostsRep>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _usersRepoMock = new Mock<IUserRepo>();
            _commentRepMock = new Mock<ICommentsRep>();

            _controller = new HomeController(
                _postsRepMock.Object,
                _usersRepoMock.Object,
                _commentRepMock.Object);
        }

        [Fact]
        public async Task Home_ReturnsViewWithAllPosts()
        {
            // Arrange
            var expectedPosts = new List<Posts>
        {
            new Posts { Id = 1, Title = "Post 1", Body = "Content 1" },
            new Posts { Id = 2, Title = "Post 2", Body = "Content 2" },
            new Posts { Id = 3, Title = "Post 3", Body = "Content 3" }
        };
            _postsRepMock.Setup(r => r.GetAll()).ReturnsAsync(expectedPosts);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Session).Returns(new Mock<ISession>().Object);

            var controllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            _controller.ControllerContext = controllerContext;

            // Act
            var result = await _controller.Home();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualPosts = Assert.IsAssignableFrom<IEnumerable<Posts>>(viewResult.ViewData.Model);
            Assert.Equal(expectedPosts.Count, actualPosts.Count());
            Assert.Equal(expectedPosts, actualPosts);
        }

        [Fact]
        public async Task Post_ReturnsViewWithBlogAndComments()
        {
            // Arrange
            int postId = 1;
            var expectedPost = new Posts { Id = postId, Title = "Post 1", Body = "Content 1" };
            var expectedComments = new List<Comment>
            {
                new Comment { Id = 1, PostId = postId, body = "Comment 1" },
                new Comment { Id = 2, PostId = postId, body = "Comment 2" },
                new Comment { Id = 3, PostId = postId, body = "Comment 3" }
            };
            _postsRepMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(expectedPost);
            _commentRepMock.Setup(r => r.GetAll(postId)).ReturnsAsync(expectedComments);

            // Act
            var result = await _controller.Post(postId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualData = Assert.IsType<CommentAndContentDTO>(viewResult.ViewData.Model);
            Assert.Equal(expectedPost, actualData.blog);
            Assert.Equal(expectedComments, actualData.AllComments);
        }
        [Fact]
        public async Task DeleteComment_DeletesCommentAndRedirectsToPostWithCorrectId()
        {
            // Arrange
            int postId = 1;
            int commentId = 2;
            var comment = new Comment { Id = commentId, body = "Test Comment", PostId = postId, UserId = 1, UserName = "Test User" };
            _commentRepMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);

            // Act
            var result = await _controller.DeleteComment(postId, commentId);

            // Assert
            _commentRepMock.Verify(r => r.Delete(comment), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Post", redirectResult.ActionName);
            Assert.Equal(postId, redirectResult.RouteValues["Id"]);
        }
        [Fact]
        public async Task DeletePost_DeletesPostAndCommentsAndRedirectsToHome()
        {
            // Arrange
            var postId = 1;
            var comments = new List<Comment>
            {
                new Comment { Id = 1, PostId = postId },
                new Comment { Id = 2, PostId = postId }
            };
            var post = new Posts { Id = postId };

            _postsRepMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(post);
            _commentRepMock.Setup(x => x.GetAll(postId)).ReturnsAsync(comments);

            // Act
            var result = await _controller.DeletePost(postId) as RedirectToActionResult;

            // Assert
            _postsRepMock.Verify(x => x.Delete(post), Times.Once);
            _commentRepMock.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Exactly(2));
            Assert.Equal("Home", result.ActionName);
        }

    }
}
