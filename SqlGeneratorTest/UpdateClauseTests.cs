using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class UpdateClauseTests
    {
        [Test]
        public void BuildUpdateValue()
        {
            var result = new UpdateValue("Name", "Value").GetQuery();
            Assert.That(result, Is.EqualTo("Name = Value"));
        }
        
        [Test]
        public void BuildUpdateClause()
        {
            var value1 = new UpdateValue("Name1", "Value");
            var value2 = new UpdateValue("Name2", 12);
            var result = new UpdateClause("Table", new []{value1, value2}).GetQuery();
            Assert.That(result, Is.EqualTo("UPDATE Table SET (Name1 = Value, Name2 = 12)"));
        }
    }
}