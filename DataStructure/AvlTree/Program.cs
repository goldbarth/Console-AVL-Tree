namespace AvlTree;

// Sources: https://www.programiz.com/dsa/avl-tree,
// https://www.geeksforgeeks.org/introduction-to-avl-tree/

//TODO: fix balance, add user input, dll?
internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("AVL Tree: \n");

        var tree = new AvlTree<int>();

        tree.Insert(20);
        tree.Insert(4);
        tree.Insert(26);
        tree.Insert(9);
        tree.Insert(3);
        tree.Insert(21);
        tree.Insert(30);
        tree.Insert(11);
        tree.Insert(7);
        tree.Insert(2);
        tree.Insert(8);
        
        tree.PrintTree();
    }
}