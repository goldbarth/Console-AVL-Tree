namespace AvlTree;

internal sealed class NodeInfo<T>
{
    public Node<T>? Node;
    public string? Text;
    public string? Text2;
    public int StartPos;
    public int Size => Text!.Length;

    public int EndPos 
    { 
        get => StartPos + Size;
        set => StartPos = value - Size;
    }
    public NodeInfo<T>? Parent, Left, Right;
}