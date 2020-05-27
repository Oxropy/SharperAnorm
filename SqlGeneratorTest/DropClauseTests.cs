using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class DropClauseTests
    {
        [Test]
        public void BuildDropClause()
        {
            var result = new DropClause("Table").GetQuery();
            Assert.That(result, Is.EqualTo("DROP TABLE Table"));
        }
    }
}