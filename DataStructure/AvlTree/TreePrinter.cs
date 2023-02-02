namespace AvlTree;

// Source: https://stackoverflow.com/questions/36311991/c-sharp-display-a-binary-search-tree-in-console
internal static class TreePrinter<T>
{
    #region Print Standard

    public static bool PrintWithBalanceFactor { get; set; }
    
    private static string ConvertBalance(Node<T> current)
    {
        if (current.Balance is -1)
            return "-";
        if (current.Balance is 1)
            return "+";

        return "";
    }
    
    public static void Print(Node<T>? root, string textFormat = "0", int spacing = 3, int topMargin = 2, int leftMargin = 2)
    {
        if (root == null) return;
        var rootTop = Console.CursorTop + topMargin;
        var last = new List<NodeInfo<T>>();
        var next = root;
        for (int level = 0; next != null; level++)
        {
            var item = new NodeInfo<T> { Node = next, Text = next.Data?.ToString(), Text2 = ConvertBalance(next) };
            if (level < last.Count)
            {
                item.StartPos = last[level].EndPos + spacing;
                last[level] = item;
            }
            else
            {
                item.StartPos = leftMargin;
                last.Add(item);
            }
            if (level > 0)
            {
                item.Parent = last[level - 1];
                if (next == item.Parent.Node?.Left)
                {
                    item.Parent.Left = item;
                    item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos - 1);
                }
                else
                {
                    item.Parent.Right = item;
                    item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos + 1);
                }
            }
            next = next.Left ?? next.Right;
            for (; next == null; item = item.Parent)
            {
                var top = rootTop + 2 * level;
                if (PrintWithBalanceFactor)
                {
                    Print(item.Text, top, item.StartPos, s2: item.Text2);
                }
                else
                {
                    Print(item.Text, top, item.StartPos);
                }
                
                if (item?.Left != null)
                {
                    Print("/",top + 1, item.Left.EndPos);
                    Print("_", top, item.Left.EndPos + 1, item.StartPos);
                }
                if (item?.Right != null)
                {
                    Print("_", top, item.EndPos, item.Right.StartPos - 1);
                    Print("\\", top + 1, item.Right.StartPos - 1);
                }
                if (--level < 0) break;
                if (item == item?.Parent?.Left)
                {
                    item!.Parent!.StartPos = item.EndPos + 1;
                    next = item.Parent.Node?.Right;
                }
                else
                {
                    if (item?.Parent!.Left == null)
                        item!.Parent!.EndPos = item.StartPos - 1;
                    else
                        item.Parent.StartPos += (item.StartPos - 1 - item.Parent.EndPos) / 2;
                }
            }
        }
        Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
    }

    private static void Print(string? s, int top, int left, int right = -1, string? s2 = "")
    {
        Console.SetCursorPosition(left, top);
        if (right < 0) right = left + s!.Length;
        while (Console.CursorLeft < right) Console.Write(s + s2);
    }
    
    #endregion

    #region Graphic Print

     public static void Visualize(Node<T>? root, int topMargin = 2, int leftMargin = 2)
    {
        if (root is null) return;
        var rootTop = Console.CursorTop + topMargin;
        var last = new List<NodeInfo<T>>();
        var next = root;
        for (int level = 0; next != null; level++)
        {
            var item = new NodeInfo<T> { Node = next, Text = next.Data?.ToString() };
            if (level < last.Count)
            {
                item.StartPos = last[level].EndPos + 1;
                last[level] = item;
            }
            else
            {
                item.StartPos = leftMargin;
                last.Add(item);
            }
            if (level > 0)
            {
                item.Parent = last[level - 1];
                if (next == item.Parent.Node?.Left)
                {
                    item.Parent.Left = item;
                    item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos);
                }
                else
                {
                    item.Parent.Right = item;
                    item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos);
                }
            }
            next = next.Left ?? next.Right;
            for (; next == null; item = item.Parent)
            {
                Visualize(item, rootTop + 2 * level);
                if (--level < 0) break;
                if (item == item.Parent?.Left)
                {
                    item.Parent.StartPos = item.EndPos;
                    next = item.Parent.Node?.Right;
                }
                else
                {
                    if (item.Parent?.Left == null)
                        item.Parent!.EndPos = item.StartPos;
                    else
                        item.Parent.StartPos += (item.StartPos - item.Parent.EndPos) / 2;
                }
            }
        }
        Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
    }

    private static void Visualize(NodeInfo<T> item, int top)
    {
        SwapColors();
        Visualize(item.Text!, top, item.StartPos);
        SwapColors();
        if (item.Left != null)
            Visualize(top + 1, "┌", "┘", item.Left.StartPos + item.Left.Size / 2, item.StartPos);
        if (item.Right != null)
            Visualize(top + 1, "└", "┐", item.EndPos - 1, item.Right.StartPos + item.Right.Size / 2);
    }

    private static void Visualize(int top, string start, string end, int startPos, int endPos)
    {
        Visualize(start, top, startPos);
        Visualize("─", top, startPos + 1, endPos);
        Visualize(end, top, endPos);
    }

    private static void Visualize(string s, int top, int left, int right = -1)
    {
        Console.SetCursorPosition(left, top);
        if (right < 0) right = left + s.Length;
        while (Console.CursorLeft < right) Console.Write(s);
    }

    private static void SwapColors()
    {
        (Console.ForegroundColor, Console.BackgroundColor) = 
            (Console.BackgroundColor, Console.ForegroundColor);
    }

    #endregion
}