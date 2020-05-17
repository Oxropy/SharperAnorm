using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class QueryBuilderTests
    {
        [Test]
        public void CreateFieldReferenceWithoutTable()
        {
            var result = new FieldReferenceExpression("Name").GetQuery();
            Assert.That(result, Is.EqualTo("Name"));
        }

        [Test]
        public void CreateFieldReferenceWithTable()
        {
            var result = new FieldReferenceExpression("Name", "Table").GetQuery();
            Assert.That(result, Is.EqualTo("Table.Name"));
        }
    }
}