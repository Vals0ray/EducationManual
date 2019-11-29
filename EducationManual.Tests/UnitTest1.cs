using System;
using Moq;
using System.Web.Mvc;
using EducationManual.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EducationManual.Services;

namespace EducationManual.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var mock = new Mock<ISchoolService>();
            SchoolController controller = new SchoolController(mock.Object);

            // Act
            ViewResult result = controller.List().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
