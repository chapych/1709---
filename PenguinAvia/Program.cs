
using System.Text;

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
var finalPrice = 0;
var connectedList = graph.FindConnected().Select(x=>new Graph(x)); // graphs with only more than two nodes are needed

foreach (var connected in connectedList)
{
    connected.FindCycles();
}


public static class Reader
{
    public static Graph ReadFromText(int n, params string[] text)
    {
        var graph = new Graph(n);
        for (int j = 0; j<n; j++)
        {
            var line = text[j].Split(' ').Select(x => Int32.Parse(x)).ToArray();
            for (int i = j; i < n; i++)
            {
                if (line[i] == 1)
                    graph.Connect(i, j);
            }
        }
        return graph;
    }
}


public class Graph
{
    public Node[] nodes;
    public int Length { get { return nodes.Length; } }
    public Graph(int n)
    {
        nodes = new Node[n]; ////add nodes initialisings
        for (int i = 0; i < n; i++)
            nodes[i] = new Node(i);
    }

    public Graph(IEnumerable<Node> values)
    {
        nodes = new Node[values.Count()];
        int i = 0;
        foreach(var el in values)
            nodes[i++] = el;
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
            foreach (var node in nodes) 
                yield return node;
        }

    
    }

    public IEnumerable<IEnumerable<Node>> FindConnected()
    {
        var visited = new HashSet<Node>();
        var length = this.Length;
        while (true)
        {
            var nextNode = this.Nodes.Where(node => !visited.Contains(node)).FirstOrDefault();
            if (nextNode == null) break;
            var breadthSearch = this.BreadthSearch(nextNode).ToList(); 
            //result.Add(breadthSearch.ToList());
            yield return breadthSearch;
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
            foreach(var node in current.Connected)
            {
                queue.Enqueue(node);
            }
            visited.Add(current);
            yield return current;
        }
    }

    public void FindCycles()
    {
        var nodes = this.Nodes;
        var visited = new HashSet<Node>();  // Серые вершины
        var finished = new HashSet<Node>(); // Черные вершины
        var nextPoints = new Stack<Node>();
        var previousPoints = new Stack<Node>();
        visited.Add(nodes.First());
        nextPoints.Push(nodes.First());
        
        while (nextPoints.Count != 0)
        {
            var node = nextPoints.Pop();
            previousPoints.Push(node);
            foreach (var nextNode in node.Connected)
            {
                if (finished.Contains(nextNode)) continue;
                if (visited.Contains(nextNode))
                {
                    var root = nextNode.Connected.Where(x => finished.Contains(x))
                                                 .FirstOrDefault();
                    nextNode.DisconnectExpectFor(root);
                    DestroyCycle(previousPoints, root);
                }
                visited.Add(nextNode);
                nextPoints.Push(nextNode);
            }
            finished.Add(node); // красим в черный, когда рассмотрели все пути из node(no inc or all inc nodes are blACK)         
        }

    }

    void DestroyCycle(Stack<Node> stack, Node root)
    {
        while(true)
        {
            var current = stack.Pop();
            if (current == null) return;////idk why could this possibly happen
            if(current == root) return;
            current.DisconnectExpectFor(root);
        }
    }

    public int Reduce()
    {
        int result = 0;
        var baseNode = Nodes.OrderByDescending(x => x.Connected.Count)
                .FirstOrDefault();
        if (baseNode == null) return 0;
        foreach(var node in Nodes)
        {
            if (node == baseNode) continue;
            node.DisconnectExpectFor(baseNode);
            result++;
        }
        return result;
    }

}

public class Node
{
    public int id;
    public HashSet<Node> Connected;

    public Node(int id)
    {
        this.id = id;
        Connected = new HashSet<Node>();
    }

    public void Connect(Node other)
    {
        Connected.Add(other);
        other.Connected.Add(this);
    }

    public void DisconnectExpectFor(Node restNode)
    {
        Connected.Clear();
        this.Connect(restNode);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach(var el in Connected)
            sb.Append(el.id);
        return sb.ToString();
    }
}


	
