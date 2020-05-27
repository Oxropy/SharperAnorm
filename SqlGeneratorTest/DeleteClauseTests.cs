using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class DeleteClauseTests
    {
        [Test]
        public void BuildDeleteClause()
        {
            var result = new DeleteClause("Table").GetQuery();
            Assert.That(result, Is.EqualTo("DELETE FROM Table"));
        }
    }
}