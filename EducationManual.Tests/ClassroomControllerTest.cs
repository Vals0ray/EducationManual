using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EducationManual.Controllers;
using EducationManual.Models;
using EducationManual.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationManual.Tests
{
    [TestClass]
    public class ClassroomControllerTest
    {
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
            mockSchoolService.Setup(s => s.GetSchoolAsync(1)).ReturnsAsync(new School());
            ClassroomController controller = 
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);

            // Act
            ViewResult result = controller.List(1).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // Arrange
            string expected = "Create";
            Classroom classroom = new Classroom();
            ClassroomController controller = 
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);
            controller.ModelState.AddModelError("Name", "Error!!!");

            // Act
            ViewResult result = controller.Create(classroom).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void CreatePostAction_RedirectToListView()
        {
            // Arrange
            string expected = "List";
            Classroom classroom = new Classroom() { Name = "TestClassroom", SchoolId = 1};
            ClassroomController controller = 
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);
            controller.ControllerContext = 
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Create(classroom).Result as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void UpdatePostAction_ModelError()
        {
            // Arrange
            string expected = "Update";
            Classroom classroom = new Classroom() { Name = "TestClassroom", SchoolId = 1 };
            ClassroomController controller = 
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);
            controller.ModelState.AddModelError("Name", "Error!!!");

            // Act
            ViewResult result = controller.Update(classroom).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void UpdatePostAction_RedirectToListView()
        {
            // Arrange
            string expected = "List";
            Classroom classroom = new Classroom() { Name = "TestClassroom", SchoolId = 1, ClassroomId = 1 };
            mockClassroomService.Setup(c => c.GetClassroomAsync(1))
                .ReturnsAsync(new Classroom() { Name = "TestClassroom2", SchoolId = 2 });
            ClassroomController controller =
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);
            controller.ControllerContext =
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Update(classroom).Result as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void DeletePostAction_RedirectToListView()
        {
            // Arrange
            string expected = "List";
            Classroom classroom = new Classroom() { Name = "TestClassroom", SchoolId = 1, ClassroomId = 1 };
            mockClassroomService.Setup(c => c.GetClassroomAsync(1))
                .ReturnsAsync(new Classroom() { Name = "TestClassroom2", SchoolId = 2 });

            ClassroomController controller =
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);

            controller.ControllerContext =
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Delete(1).Result as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }
    }
}
