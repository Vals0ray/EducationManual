using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EducationManual.Controllers;
using EducationManual.Models;
using EducationManual.Services;
using EducationManual.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationManual.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        private Mock<IUserService> mockUserService;
        private Mock<ISchoolService> mockSchoolService;
        private Mock<IClassroomService> mockClassroomService;
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;

        [TestInitialize]
        public void SetupTests()
        {
            // Setup Moq
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            mockUserService = new Mock<IUserService>();
            mockSchoolService = new Mock<ISchoolService>();
            mockClassroomService = new Mock<IClassroomService>();
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
            moqContext.Setup(x => x.Request.UserHostAddress).Returns("192.111.1.1");
            moqContext.Setup(x => x.User.Identity.Name).Returns("TestUser");
        }

        [TestMethod]
        public void ListViewModelNotNull()
        {
            // Arrange
            mockUserService.Setup(u => u.GetUserByRoleAsync("Student")).ReturnsAsync(new List<ApplicationUser>());
            UserController controller =
                new UserController(mockUserService.Object, mockSchoolService.Object, mockClassroomService.Object);

            // Act
            ViewResult result = controller.List("Student", null).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void DetailsPostAction_ModelError()
        {
            // Arrange
            UserViewModel user = new UserViewModel() { Role = "Student", SchoolName = "TestSchool" };

            UserController controller =
                new UserController(mockUserService.Object, mockSchoolService.Object, mockClassroomService.Object);
            controller.ModelState.AddModelError("Name", "Error!!!");

            // Act
            ViewResult result = controller.Details(user).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user, result.Model);
        }

        [TestMethod]
        public void DeletePostAction_RedirectToListView()
        {
            // Arrange
            string expected = "List";
            UserViewModel user = new UserViewModel() { Id = "1", Email = "Email", Role = "Student", SchoolName = "TestSchool" };

            UserController controller =
                new UserController(mockUserService.Object, mockSchoolService.Object, mockClassroomService.Object);
            controller.ControllerContext =
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectResult result = controller.Delete(user, expected).Result as RedirectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Url);
        }
    }
}
