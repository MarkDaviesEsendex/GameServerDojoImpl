using System.Collections.Generic;
using System.Linq;
using Server.Web.Controllers;
using NUnit.Framework;

namespace Server.Web.Tests.Controllers
{
    [TestFixture]
    public class ValuesControllerTest
    {
        [Test]
        public void Get()
        {
            var controller = new ValuesController();

            var result = controller.Get();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [Test]
        public void GetById()
        {
            var controller = new ValuesController();
            var result = controller.Get(5);
            Assert.AreEqual("value", result);
        }

        [Test]
        public void Post()
        {
            var controller = new ValuesController();
            controller.Post("value");
        }

        [Test]
        public void Put()
        {
            var controller = new ValuesController();
            controller.Put(5, "value");
        }

        [Test]
        public void Delete()
        {
            var controller = new ValuesController();
            controller.Delete(5);
        }
    }
}
