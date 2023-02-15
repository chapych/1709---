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
                
                var graph = Reader.ReadFromText(input);
                Assert.That(graph.Length, Is.EqualTo(2));
                Assert.That(graph.nodes[0].Connected.Select(x=>x.id), Is.EquivalentTo(new int[] { 1 }));
            }

            [Test]
            public void SimpleGraph_3x3()
            {
                var input = new string[] { "0 1 0", "1 0 1", "0 1 0" };
                var graph = Reader.ReadFromText(input);
                Assert.That(graph.Length, Is.EqualTo(3));
                Assert.That(graph.nodes[1].Connected.Select(x => x.id), Is.EquivalentTo(new int[] { 2,0 }));
            }

            [Test]
            public void SimpleGraph_Empty()
            {
                var input = new string[] { "0 0 0", "0 0 0", "0 0 0" };
                var graph = Reader.ReadFromText(input);
                Assert.That(graph.Length, Is.EqualTo(3));
                Assert.That(graph.nodes[1].Connected.Select(x => x.id), Is.EquivalentTo(new int[] {}));
            }
        }

        [TestFixture]
        public class CorrectConnectedSubGraphs
        {
            [Test]
            public void NonConnectedGraph()
            {
                var input = new string[] { "0 0 0", "0 0 0", "0 0 0" };
                var graph = Reader.ReadFromText(input);
                var actual = graph.FindConnected().Select(x => x.Select(node => node.id).ToArray()).ToList();
                int i = 0;
                foreach(var array in actual)
                {
                    Assert.That(array, Is.EquivalentTo(new int[] {i++}));
                }
            }

            [Test]
            public void ConnectedGraph1()
            {
                var input = new string[] { "0 1 1", "1 0 1", "1 1 0" };
                var graph = Reader.ReadFromText(input);
                var actual = graph.FindConnected().Select(x => x.Select(node => node.id).ToArray()).ToList();
                Assert.That(actual[0], Is.EquivalentTo(new int[] {0,2,1 }));
                
            }

            [Test]
            public void ConnectedGraph2()
            {
                var input = new string[] { "0 0 1 0", "0 0 0 1", "1 0 0 0", "0 1 0 0" };
                var graph = Reader.ReadFromText(input);
                var actual = graph.FindConnected().Select(x => x.Select(node => node.id).ToArray()).ToList();
                Assert.That(actual[0], Is.EquivalentTo(new int[] { 0, 2 }));

            }



        }

        [TestFixture]
        public class CorrectDestroyingACycle
        {
            

            [Test]
            public void TransformFromCycleToTree()
            {
                var input = new string[] { "0 1 0 0 0 0 1", "1 0 1 0 0 0 0", "0 1 0 1 0 0 0", "0 0 1 0 1 0 0", "0 0 0 1 0 1 0", "0 0 0 0 1 0 1", "1 0 0 0 0 1 0" };
                var graph = Reader.ReadFromText(input);
                graph.BreakCycles();
                Assert.That(graph.nodes[1].Connected.Select(x => x.id), Is.EquivalentTo(new int[] { 0,2 }));
                Assert.That(graph.nodes[5].Connected.Select(x => x.id), Is.EquivalentTo(new int[] { 4}));
            }
        }

        [TestFixture]
        public class CorrectRoad
        {
            [Test]
            public void Road1()
            {
                var input = new string[] { "0 1 1 0 0", "1 0 1 0 0", "1 1 0 0 0", "0 0 0 0 1", "0 0 0 1 0" };

                var graph = Solver.FindOptimalWay(input);
                Assert.That(graph.nodes[0].Connected.Select(x => x.id), Is.EquivalentTo(new int[] { 1,2,3 }));
                Assert.That(graph.nodes[4].Connected.Select(x => x.id), Is.EquivalentTo(new int[] {  3 }));

            }

            [Test]
            public void Road2()
            {
                var input = new string[] { "0 1 1 0 0 0", "1 0 1 0 0 0", "1 1 0 0 0 0", "0 0 0 0 1 1", "0 0 0 1 0 1", "0 0 0 1 1 0" };
                var graph = Solver.FindOptimalWay(input);
                Assert.That(graph.nodes[0].Connected.Select(x => x.id), Is.EquivalentTo(new int[] {  2,1,3 }));
                Assert.That(graph.nodes[4].Connected.Select(x => x.id), Is.EquivalentTo(new int[] { 3 }));
            }
        }
    }
}