namespace AvlTree;

// Sources: https://www.programiz.com/dsa/avl-tree,
// https://www.geeksforgeeks.org/introduction-to-avl-tree/

//TODO: fix balance, add user input, dll?
internal static class Program
{
    private static void Main()
    {
        // Application.Run();
        var tree = new AvlTree<int>();

        tree.Insert(12);
        tree.Insert(34);
        tree.Insert(54);
        tree.Insert(11);
        tree.Insert(19);
        tree.Insert(3);
        tree.Insert(9);
        
        tree.VisualTree();
        Console.ReadKey();
        // tree.Clear(54);
        Console.ReadKey();
        tree.Insert(31);
        tree.Insert(7);
        tree.VisualTree();
        tree.Clear(11);
        tree.Insert(8);
        Console.ReadKey();
        tree.VisualTree();
        
    }
}