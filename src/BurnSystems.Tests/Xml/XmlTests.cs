using System.Xml.Linq;
using BurnSystems.Xml.Html;
using NUnit.Framework;

namespace BurnSystems.Tests.Xml
{
    [TestFixture]
    public class XmlTests
    {
        [Test]
        public void TestNl2BrSimple()
        {
            var text = "x";
            var result = HtmlElement.ConvertNewLineToBreaks(text);

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.InstanceOf<string>());
            Assert.That(result[0] as string, Is.EqualTo("x"));
        }

        [Test]
        public void TestNl2BrSimpleTwoLines()
        {
            var text = "x\r\ny";
            var result = HtmlElement.ConvertNewLineToBreaks(text);

            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0], Is.InstanceOf<string>());
            Assert.That(result[0] as string, Is.EqualTo("x"));
            Assert.That(result[1], Is.InstanceOf<XElement>());
            Assert.That((result[1] as XElement).Name.ToString(), Is.EqualTo("br"));
            Assert.That(result[2], Is.InstanceOf<string>());
            Assert.That(result[2] as string, Is.EqualTo("y"));
        }
    }
}