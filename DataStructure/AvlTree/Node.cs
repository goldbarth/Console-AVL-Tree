namespace AvlTree;

internal sealed class Node<T>
{
    public T Data;
    public Node<T>? Left;
    public Node<T>? Right;
    public int Height;
    public int Balance;

    public Node(T data)
    {
        Data = data;
        Left = null;
        Right = null;
        Height = 1;
        Balance = 0;
    }
}