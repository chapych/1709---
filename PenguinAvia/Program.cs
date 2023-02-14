int n   = Int32.Parse(Console.ReadLine());
var prices = Console.ReadLine().Split(' ').Select(x=>Int32.Parse(x)).ToArray();
var d = prices[0];
var a = prices[1];
var graph = new Graph(n);

for (int j = n; j > 0; j--)
{
    var line = Console.ReadLine().Split(' ').Skip(j).Select(x => Int32.Parse(x)).ToArray();
    for (int i = 0; i < n; i++)
    {
        if (line[i] == 1)
            graph.Connect(i, n - j);
    }

}

public class Graph
{
    public Node[] nodes;
    public int Length { get { return nodes.Length; } }
    public Graph(int n)
    {
        nodes = new Node[n];
    }

    public int this[int i] => nodes[i].id;
    public void Connect(int a, int b)
    {
        nodes[a].Connect(nodes[b]);
    }

    public IEnumerable<Node> Nodes
    {
        get
        {
           foreach(var node in nodes)
                yield return node;
    }

    
    }

    IEnumerable<Node> FindConnected(Graph graph)
    {
        var visited = new HashSet<Node>();
        var length = graph.Length;
        while (true)
        {
            var nextNode = graph.Nodes.Where(node => !visited.Contains(node)).FirstOrDefault();
            if (nextNode == null) break;
            var breadthSearch = graph.BreadthSearch(nextNode).ToList(); 
            //result.Add(breadthSearch.ToList());
            foreach (var node in breadthSearch)
            {
                visited.Add(node);
                yield return node;
            }
            
        }
    }

    IEnumerable<Node> BreadthSearch(Node start)
    {
        var visited = new HashSet<Node>();
        var queue = new Queue<Node>();
        queue.Enqueue(start);
        while (true)
        {
            var current = queue.Dequeue();
            foreach(var node in current.connected)
            {
                queue.Enqueue(node);
            }
            visited.Add(current);
            yield return current;
        }
    }


}

public class Node
{
    public int id;
    public List<Node> connected;

    public Node(int id)
    {
        this.id = id;
        connected = new List<Node>();
    }

    public void Connect(Node other)
    {
        connected.Add(other);
        other.connected.Add(this);
    }
}


	
