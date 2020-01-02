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
    public class SchoolControllerTest
    {
        private Mock<ISchoolService> mock;
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;

        [TestInitialize]
        public void SetupTests()
        {
            // Setup Moq
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            mock = new Mock<ISchoolService>();
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
            moqContext.Setup(x => x.Request.UserHostAddress).Returns("192.111.1.1");
            moqContext.Setup(x => x.User.Identity.Name).Returns("TestUser");
        }

        [TestMethod]
        public void ListViewModelNotNull()
        {
            // Arrange
            SchoolController controller = new SchoolController(mock.Object);

            // Act
            ViewResult result = controller.List().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // Arrange
            string expected = "Create";
            SchoolViewModel school = new SchoolViewModel();
            SchoolController controller = new SchoolController(mock.Object);
            controller.ModelState.AddModelError("Name", "Error!!!");

            // Act
            ViewResult result = controller.Create(school).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void CreatePostAction_RedirectToListView()
        {
            // Arrange
            string expected = "List";
            SchoolViewModel school = new SchoolViewModel() { Name = "TestSchool" };
            SchoolController controller = new SchoolController(mock.Object);
            controller.ControllerContext = 
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Create(school).Result as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void UpdatePostAction_ModelError()
        {
            // Arrange
            string expected = "Update";
            SchoolViewModel school = new SchoolViewModel();
            SchoolController controller = new SchoolController(mock.Object);
            controller.ModelState.AddModelError("Name", "Error!!!");

            // Act
            ViewResult result = controller.Update(school).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void UpdatePostAction_RedirectToListView()
        {
            // Arrange
            string expected = "List";
            SchoolViewModel school = new SchoolViewModel() { Name = "TestSchool", Id = 1 };
            mock.Setup(x => x.GetSchoolAsync(1)).ReturnsAsync(new School() { Name = "Test" });
            SchoolController controller = new SchoolController(mock.Object);
            controller.ControllerContext =
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Update(school).Result as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void DeletePostAction_RedirectToListView()
        {
            // Arrange
            string expected = "List";
            SchoolController controller = new SchoolController(mock.Object);
            controller.ControllerContext =
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Delete(1, "TestName").Result as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }
    }
}
