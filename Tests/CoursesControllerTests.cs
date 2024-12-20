using Xunit;
using Moq;
using Demo3.Controllers;
using Demo3.Services;
using Demo3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo3.Tests
{
    public class CoursesControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithCourseList()
        {
            // Arrange
            var mockService = new Mock<ICoursesService>();
            mockService.Setup(service => service.GetAllFilter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int>()))
                       .ReturnsAsync(new PaginatedList<CourseViewModel>(
                           new List<CourseViewModel>
                           {
                               new CourseViewModel { Id = 1, Title = "Course 1", Topic = "Topic 1" },
                               new CourseViewModel { Id = 2, Title = "Course 2", Topic = "Topic 2" }
                           },
                           1, // pageIndex
                           2, // totalPages
                           10 // totalCount
                       ));

            var controller = new CoursesController(mockService.Object);

            // Act
            var result = await controller.Index(null, null, null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedList<CourseViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenCourseDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ICoursesService>();
            mockService.Setup(service => service.GetById(It.IsAny<int>()))
                       .ReturnsAsync((CourseViewModel?)null);

            var controller = new CoursesController(mockService.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var mockService = new Mock<ICoursesService>();
            var controller = new CoursesController(mockService.Object);
            var courseRequest = new CourseRequest { Title = "New Course", Topic = "Topic 1" };

            // Act
            var result = await controller.Create(courseRequest);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithCourseViewModel()
        {
            // Arrange
            var mockService = new Mock<ICoursesService>();
            mockService.Setup(service => service.GetById(It.IsAny<int>()))
                       .ReturnsAsync(new CourseViewModel { Id = 1, Title = "Course 1", Topic = "Topic 1" });

            var controller = new CoursesController(mockService.Object);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CourseViewModel>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndex_WhenCourseIsDeleted()
        {
            // Arrange
            var mockService = new Mock<ICoursesService>();
            var controller = new CoursesController(mockService.Object);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
