namespace AvlTree;

internal class AvlTree<T>
{
    public Node<T>? Root { get; private set; }
    private int Count { get; set; }

    private delegate int CompareDelegate(T a, T b);

    private static readonly CompareDelegate Compare = Comparer<T>.Default.Compare;

    public AvlTree()
    {
        Root = null;
    }

    #region Wrapper´s
    
    public void Add(T data)
    {
        Count++;
        var newNode = new Node<T>(data);
        Root = Root is null ? Root = newNode : Insert(Root, newNode);
    }

    public void GetCount()
    {
        Console.WriteLine($"\n\tDie Anzahl der Knoten ist: {Count}");
    }

    public Node<T>? GetNode(T target)
    {
        var foundNode = Search(Root, target);
        if (foundNode is null)
        {
            Console.WriteLine("\n\tDer gesuchte Knoten ist nicht vorhanden.");
            return null;
        }
            
        Console.WriteLine($"\n\tDer Knoten mit der Zahl {foundNode!.Data} wurde gefunden.");
        return foundNode;
    }
    
   public void GetMaxDepth()
   {
       Console.WriteLine($"\n\tDie maximale Höhe des Baums ist: {GetDepth(Root).ToString()}");
   }
    
    public void Contains(T target)
    {
        if (Contains(Root, target)) Console.WriteLine($"\n\tDer Knoten mit der Zahl {target} wurde gefunden");
        else if (!Contains(Root, target)) Console.WriteLine($"\n\tDer Knoten mit der Zahl {target} wurde nicht gefunden");
    }
    
    public void Remove(T target)
    {
        if (target is null) 
            Console.WriteLine($"\n\tZu der eingegebenen Zahl ({target}) gibt es keinen Knoten.");
        else
        {
            Root = Remove(Root, target);
            Console.WriteLine($"\n\tDer Knoten mit der Zahl ({target}) wurde gelöscht.");
            Count--;
        }
    }
    
    public static void SetPrintWithBalanceFactor(ConsoleKey key)
    {
        if (key is ConsoleKey.D1) TreePrinter<T>.PrintWithBalanceFactor = true;
        if (key is ConsoleKey.D2) TreePrinter<T>.PrintWithBalanceFactor = false;
    }

    public void Print()
    {
        TreePrinter<T>.Print(Root);
    }

    public void VisualTree()
    {
        TreePrinter<T>.Visualize(Root);
    }

    #endregion
    
    #region Body

    private Node<T>? Insert(Node<T>? current, Node<T> node)
    {
        if (current is null) 
        {
            current = node;
            return current;
        }

        if (Compare(node.Data, current.Data) < 0) // go left
            current.Left = Insert(current.Left, node); 
        else if (Compare(node.Data, current.Data) > 0) // go right
            current.Right = Insert(current.Right, node);
        else
            return current; // go to leaf node

        UpdateHeight(current);
        return Rebalance(current, node);
    }

    private Node<T> Rebalance(Node<T>? current, Node<T>? node)
    {
        if (BalanceFactor(current) > 1)
            current = (Compare(node!.Data, current!.Left!.Data) < 0)
                ? RotateRight(current)
                : RotateLeftRight(current);
        else if (BalanceFactor(current) < -1)
            current = (Compare(node!.Data, current!.Right!.Data) > 0)
                ? RotateLeft(current)
                : RotateRightLeft(current);

        current!.Balance = SetBalance(current);
        return current;
    }

    #endregion

    #region Extensions

    private Node<T>? Remove(Node<T>? current, T data)
    {
        if (current is null) return null;
        if (Compare(data, current.Data) < 0)
            current.Left = Remove(current.Left, data);
        else if (Compare(data, current.Data) > 0)
            current.Right = Remove(current.Right, data);
        else
        {
            var left = current.Left;
            var right = current.Right;
            if (right is null) return left; // if right is empty, in left could be only one node (balanced)
                                                // current node can be replaced, even if it is null
            var min = GetMin(right);
            min!.Right = RemoveMin(right);
            min.Left = left;
            return Rebalance(min, Root);
        }

        return Rebalance(current, Root);
    }

    private Node<T>? RemoveMin(Node<T> current)
    {
        if (current.Left is null) return current.Right;
        current.Left = RemoveMin(current.Left);
        return Rebalance(current, Root);
    }

    private static Node<T>? Search(Node<T>? root, T data)
    {
        if (root is null) return null;
        if (Compare(data, root.Data) < 0)
            return Search(root.Left, data);
        if (Compare(data, root.Data) > 0)
            return Search(root.Right, data);
        
        return new Node<T>(data).Data is null ? null : new Node<T>(data);
    }

    private static bool Contains(Node<T>? current, T data)
    {
        return Search(current, data) is not null;
    }

    #endregion

    #region Calculations

    private static int Height(Node<T>? current)
    {
        return current?.Height ?? 0;
    }

    private static Node<T>? GetMin(Node<T> current)
    {
        return current.Left is not null ? GetMin(current.Left) : current;
    }

    private static int SetBalance(Node<T>? current)
    {
        return current!.Balance = BalanceFactor(current);
    }

    private static int GetDepth(Node<T>? current)
    {
        return current is null ? 0 : Math.Max(GetDepth(current.Left), GetDepth(current.Right) + 1);
    }
    
    private static int BalanceFactor(Node<T>? current)
    {
        return current is null ? 0 : Height(current.Left) - Height(current.Right);
    }
    
    private static void UpdateHeight(Node<T>? current)
    { 
        current!.Height = Math.Max(Height(current.Left), Height(current.Right)) + 1;
    }

    private static Node<T> RotateLeft(Node<T>? current)
    {
        var parent = current?.Right;
        current!.Right = parent?.Left;
        parent!.Left = current;

        UpdateHeight(current); 
        UpdateHeight(parent);
        
        return parent;
    }
    
    private Node<T> RotateRight(Node<T>? current)
    {
        var parent = current?.Left;
        current!.Left = parent?.Right;
        parent!.Right = current;

        UpdateHeight(current);
        UpdateHeight(parent);
        
        return parent;
    }

    private Node<T> RotateLeftRight(Node<T>? parent)
    {
        parent!.Left = RotateLeft(parent.Left);
        return RotateRight(parent);
    }

    private Node<T> RotateRightLeft(Node<T>? parent)
    {
        parent!.Right = RotateRight(parent.Right);
        return RotateLeft(parent);
    }

    #endregion
}