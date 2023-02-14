namespace TestProject
{
    public class Tests
    {

        [TestFixture]
        public class CorrectReading
        {
            [Test]
            public void SimpleGraph_2x2()
            {
                var input = new string[] { "0 1", "1 0" };
                
                var graph = Reader.ReadFromText(input.Length, input);
                Assert.AreEqual(graph.Length, 2);
                Assert.AreEqual(graph.nodes[0].ToString(), "1");
            }

            [Test]
            public void SimpleGraph_3x3()
            {
                var input = new string[] { "0 1 0", "1 0 1", "0 1 0" };
                var graph = Reader.ReadFromText(input.Length, input);
                Assert.AreEqual(graph.Length, 3);
                Assert.AreEqual(graph.nodes[1].ToString(), "02");
            }
        }
    }
}