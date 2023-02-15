using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Sum
{
    private static void Main()
    {
        int n = Int32.Parse(Console.ReadLine());
        var text = new string[n];
        var prices = Console.ReadLine().Split(' ').Select(x => Int32.Parse(x)).ToArray();
        var d = prices[0];
        var a = prices[1];
        for (int i = 0; i < n; i++)
            text[i] = Console.ReadLine();
        var graph = Solver.FindOptimalWay(text);
        var res = Reader.FindPrice(graph, a, d);
        Console.WriteLine(res / 2);
        Reader.ReadFromGraph(graph);
    }
}

public static class Reader
{
    public static Graph ReadFromTextWithSpaces(params string[] text)
    {
        var n = text.Length;
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

    public static Graph ReadFromText(params string[] text)
    {
        var n = text.Length;
        var graph = new Graph(n);
        for (int j = 0; j < n; j++)
        {
            var line = text[j];
            for (int i = j; i < n; i++)
            {
                if ((int)line[i] == 49)
                    graph.Connect(i, j);
            }
        }
        return graph;
    }

    public static void ReadFromGraph(Graph graph)
    {
        for (int i = 0; i < graph.Length; i++)
        {
            for (int j = 0; j < graph.Length; j++)
            {
                var current = graph.Boarders[i][j];
                Console.Write(current);
            }
            Console.Write("\n");
        }
    }

    public static Int64 FindPrice(Graph graph, int a, int d)
    {
        Int64 result = 0;
        for (int i = 0; i < graph.Length; i++)
        {
            for (int j = 0; j < graph.Length; j++)
            {
                var current = graph.Boarders[i][j];
                if (current == "d") result += d;
                if (current == "a") result += a;
            }
        }
        return result;
    }

}

public static class Solver
{
    public static Graph FindOptimalWay(string[] text)
    {
        int n = text.Length;
        var graph = Reader.ReadFromText(text);
        var connectedList = graph.FindConnected().Select(x => new Graph(x)).ToList(); // graphs with only more than two nodes are needed

        foreach (var connected in connectedList)
        {
            connected.BreakCycles();
        }
        var joint = connectedList.First().nodes[0];
        for (int i = 1; i < connectedList.Count; i++)
        {
            var current = connectedList[i].nodes[0];
            current.Connect(joint);
            graph.Boarders[0][current.id] = "a";
            graph.Boarders[current.id][0] = "a";
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
            nodes[i] = new Node(i, this);
        Boarders = new string[Length][];
        for(int i = 0; i < Length; i++)
        {
            Boarders[i] = new string[Length];
            for (int j = 0; j < Length; j++)
                Boarders[i][j] = "0";
        }

    }

    public Graph(IEnumerable<Node> values)
    {
        nodes = new Node[values.Count()];
        int i = 0;
        foreach(var el in values)
            nodes[i++] = el;

    }

    public string[][] Boarders;
    
    public Node this[int i] => nodes[i];
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
            var breadthSearch = BreadthSearch(nextNode).ToList(); 
            foreach(var el in breadthSearch)
            {
                visited.Add(el);
            }
            yield return breadthSearch;
        }
    }

    static IEnumerable<Node> BreadthSearch(Node start)
    {
        var visited = new HashSet<Node>();
        var queue = new Queue<Node>();
        queue.Enqueue(start);
        visited.Add(start);
        while (queue.Count>0)
        {
            var current = queue.Dequeue();
            foreach (var node in current.Connected)
                if (!visited.Contains(node))
                {
                    queue.Enqueue(node);
                    visited.Add(node);
                }
            yield return current;
        }
    }

    public void BreakCycles()
    {
        var visited = new HashSet<Node>();  // Серые вершины
        var finished = new HashSet<Node>(); // Черные вершины
        var stack = new Stack<Node>();
        visited.Add(this[0]);
        stack.Push(this[0]);
        while (stack.Count != 0)
        {
            var node = stack.Pop();
            var incidentNodes = node.Connected.OrderBy(x => x.id).ToList();
            for(int i = incidentNodes.Count-1; i>=0; i--)  
            {
                Node nextNode = incidentNodes[i];
                if (finished.Contains(nextNode)) continue;
                if (visited.Contains(nextNode))
                {
                    node.Disconnect(nextNode);

                }
                visited.Add(nextNode);
                stack.Push(nextNode);
            }
            finished.Add(node); // красим в черный, когда рассмотрели все пути из node(no inc or all inc nodes are blACK) 
        }
    }
}

public class Node
{
    public int id;
    public List<Node> Connected;
    public Graph graph;
    public Node(int id, Graph graph)
    {
        this.id = id;
        Connected = new List<Node>();
        this.graph = graph;
    }

    public void Connect(Node other)
    {
        if(Connected.Contains(other)) return;
        Connected.Add(other);
        other.Connected.Add(this);
    }

    public void Disconnect(Node other)
    {
        Connected.Remove(other);
        other.Connected.Remove(this);
        graph.Boarders[this.id][other.id] = "d";
        graph.Boarders[other.id][this.id] = "d";
    }

    public override string ToString()   //just required for UnitTests
    {
        return this.id.ToString();
    }
}
