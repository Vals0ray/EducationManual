using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EducationManual.Controllers;
using EducationManual.Interfaces;
using EducationManual.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationManual.Tests
{
    [TestClass]
    public class ClassroomControllerTest
    {
        private Mock<IGenericService<School>> mockSchoolService;
        private Mock<IGenericService<Classroom>> mockClassroomService;
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;

        [TestInitialize]
        public void SetupTests()
        {
            // Setup Moq
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            mockSchoolService = new Mock<IGenericService<School>>();
            mockClassroomService = new Mock<IGenericService<Classroom>>();
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
            moqContext.Setup(x => x.Request.UserHostAddress).Returns("192.111.1.1");
            moqContext.Setup(x => x.User.Identity.Name).Returns("TestUser");
        }

        [TestMethod]
        public void ListViewModelNotNull()
        {
            // Arrange
            mockSchoolService
                .Setup(x => x.GetWithInclude(It.IsAny<Func<School, bool>>(), It.IsAny<Expression<Func<School, object>>[]> ()))
                .Returns((Func<School, bool> func, Expression<Func<School, object>>[] exp) => 
                    new List<School> { new School { Name = "TestSchool", SchoolId = 1 } });
            ClassroomController controller = 
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);

            // Act
            ViewResult result = controller.List(1) as ViewResult;

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
            ViewResult result = controller.Create(classroom) as ViewResult;

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
            DataSave.SchoolId = 1;
            mockClassroomService
                .Setup(x => x.Create(It.IsAny<Classroom>()));
            ClassroomController controller = 
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);
            controller.ControllerContext = 
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Create(classroom) as RedirectToRouteResult;

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
            ViewResult result = controller.Update(classroom) as ViewResult;

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
            mockClassroomService
                .Setup(x => x.Get(It.IsAny<Func<Classroom, bool>>()))
                .Returns((Func<Classroom, bool> func) =>
                    new List<Classroom> { new Classroom { Name = "TestSchool", ClassroomId = 1 } });
            ClassroomController controller =
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);
            controller.ControllerContext =
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Update(classroom) as RedirectToRouteResult;

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
            mockClassroomService
                .Setup(x => x.Get(It.IsAny<Func<Classroom, bool>>()))
                .Returns((Func<Classroom, bool> func) =>
                    new List<Classroom> { new Classroom { Name = "TestSchool", ClassroomId = 1 } });

            ClassroomController controller =
                new ClassroomController(mockClassroomService.Object, mockSchoolService.Object);

            controller.ControllerContext =
                new ControllerContext(moqContext.Object, new RouteData(), controller);

            // Act
            RedirectToRouteResult result = controller.Delete(1) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }
    }
}
