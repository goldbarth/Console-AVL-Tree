namespace AvlTree;

internal class AvlTree<T>
{
    public Node<T>? Root { get; set; }
    
    public delegate int CompareDelegate(T a, T b);
    private readonly CompareDelegate _compare = Comparer<T>.Default.Compare;

    public AvlTree()
    {
        Root = null;
    }
    
    public AvlTree(CompareDelegate compare) : this()
    {
        _compare = compare;
    }

    #region Wrapper´s

    
    public void Insert(T data)
    {
        var newNode = new Node<T>(data);
        Root = Root is null ? Root = newNode : Insert(Root, newNode, _compare);
    }

    public void Search(T target)
    {
        var newNode = new Node<T>(target);
        Search(newNode, target, out var found, _compare);
        Console.WriteLine($"This node is {found!.Data}");
    }
    
   public void GetMaxDepth(Node<T>? root)
   {
       Console.WriteLine($"Max depth is {GetDepth(root)}");
   }
    
    public void Find(T target)
    {
        if (Contains(Root, target, _compare))
            Console.WriteLine($"{target} was Found");
        else if (!Contains(Root, target, _compare))
            Console.WriteLine($"{target} was not Found");
    }
    
    public void Delete(T target)
    {
        Delete(Root, target, _compare);
    }
    
    public bool Contains(T target)
    {
        return Contains(Root, target, _compare);
    }
    
    public void PrintTree()
    {
        TreePrinter<T>.Print(Root);
    }

    public void VisualTree()
    {
        TreePrinter<T>.Visualize(Root);
    }

    #endregion
    
    #region Body
    
    public Node<T>? Insert(Node<T>? currentNode, Node<T> newNode, CompareDelegate compare)
    {
        if (currentNode is null) 
        {
            currentNode = newNode;
            return currentNode;
        }

        if (compare(newNode.Data, currentNode.Data) < 0) // go left
            currentNode.Left = Insert(currentNode.Left, newNode, compare); 
        else if (compare(newNode.Data, currentNode.Data) > 0) // go right
            currentNode.Right = Insert(currentNode.Right, newNode, compare);
        else
            return currentNode; // go to leaf node

        UpdateHeight(currentNode);
        return Rebalance(currentNode, newNode, compare);
    }

    private static Node<T>? Rebalance(Node<T>? childNode, Node<T>? newNode, CompareDelegate compare)
    {
        var balance = BalanceFactor(childNode); // -1 left, 0 balanced, 1 right
        if (balance > 1)
            childNode = (compare(newNode!.Data, childNode!.Left!.Data) < 0)
                ? RotateRight(childNode)
                : RotateLeftRight(childNode);
        else if (balance < -1)
            childNode = (compare(newNode!.Data, childNode!.Right!.Data) > 0)
                ? RotateLeft(childNode)
                : RotateRightLeft(childNode);

        SetBalance(childNode);
        return childNode;
    }

    #endregion

    #region Extensions

    private static Node<T>? Delete(Node<T>? current, T data, CompareDelegate compare)
    {
        if (current is null) return null;
        
        if (compare(data, current.Data) < 0)
            current.Left = Delete(current.Left, data, compare);
        {
            current.Left = Delete(current.Left, data, compare);
            if (BalanceFactor(current) == -2) // left heavy
            {
                current = BalanceFactor(current.Right) <= 0
                    ? RotateRight(current) 
                    : RotateLeftRight(current);
            }
        }
        if (compare(data, current.Data) > 0)
        {
            current.Right = Delete(current.Right, data, compare);
            if (BalanceFactor(current) == 2) // right heavy
            {
                current = BalanceFactor(current.Right) >= 0 
                    ? RotateLeft(current) 
                    : RotateRightLeft(current);
            }
        }
        else // when the target node is archived
        {
            if (current.Right is not null)
            {
                var parent = current.Right;
                while (current.Left is not null)
                {
                    current = current.Left;
                }

                current.Data = parent.Data;
                current.Right = Delete(current.Right, parent.Data, compare);
                if (BalanceFactor(current.Left) == 2)
                {
                    current = BalanceFactor(current) >= 0
                        ? RotateLeft(current)
                        : RotateRightLeft(current);
                }
            }
            else
            {
                return current.Left;
            }
        }
        
        return current;
    }

    private static Node<T>? Search(Node<T>? current, T data, CompareDelegate compare)
    {
        if (current is null) return null;

        if (compare(data, current.Data) < 0)
            return Search(current.Left, data, compare);
        if (compare(data, current.Data) > 0)
            return Search(current.Right, data, compare);

        return current;
    }

    private static bool Search(Node<T>? current, T data, out Node<T>? node, CompareDelegate compare)
    {
        node = Search(current, data, compare);
        return node is not null;
    }

    private static bool Contains(Node<T>? current, T data, CompareDelegate compare)
    {
        return Search(current, data, compare) is not null;
    }

    #endregion

    #region Traversals
    
    public void PreOrder(Node<T>? current) 
    {
        if (current is null) return;
        Console.Write($"[{current.Data}] ");
        PreOrder(current.Left);
        PreOrder(current.Right);
    }

    public void InOrder(Node<T>? current)
    {
        if (current is null) return;
        InOrder(current.Left);
        Console.Write($"[{current.Data}] ");
        InOrder(current.Right);
    }

    public void PostOrder(Node<T>? current)
    {
        if (current is null) return;
        InOrder(current.Left);
        InOrder(current.Right);
        Console.Write($"[{current.Data}] ");
    }

    #endregion

    #region Calculations

    private static int Height(Node<T>? current)
    {
        return current?.Height ?? 0;
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

    private static Node<T> RotateLeft(Node<T>? x)
    {
        var y = x?.Right;
        var t2 = y?.Left;
        
        y!.Left = x;
        x!.Right = t2;
        
        UpdateHeight(x); 
        UpdateHeight(y);
        
        return y;
    }
    
    private static Node<T> RotateRight(Node<T>? y)
    {
        var x = y?.Left;
        var t2 = x?.Right;
        
        x!.Right = y;
        y!.Left = t2;
        
        UpdateHeight(y);
        UpdateHeight(x);
        
        return x;
    }

    private static Node<T> RotateLeftRight(Node<T>? parent)
    {
        parent!.Left = RotateLeft(parent.Left);
        return RotateRight(parent);
    }

    private static Node<T> RotateRightLeft(Node<T>? parent)
    {
        parent!.Right = RotateRight(parent.Right);
        return RotateLeft(parent);
    }

    #endregion
}