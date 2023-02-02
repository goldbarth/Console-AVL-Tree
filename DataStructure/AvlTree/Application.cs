namespace AvlTree;

public static class Application
{
    private static readonly AvlTree<int> Tree = new();
    
    private enum InputType { Number, Options, Restart }
    private static InputType _inputType;
    
    private static bool _isRandomInput;
    
    public static void Run()
    {
        Selection();
    }

    private static void Selection()
    {
        Intro();
        Initialize();
        PrintOptions();
        UserOptions();
    }
    
    private static void Intro()
    {
        Console.WriteLine("\n\n\t\t\tAVL-BAUM\n");
        Console.WriteLine();
        Console.WriteLine("\n\t1. Die Zahlen können manuell eingegeben oder generiert werden.");
        Console.WriteLine("\n\t2. Es gibt nachfolgend eine Liste mit Optionen um den Baum zu bearbeiten.");
        Console.WriteLine("\n\tTipp: Starte mit z.B. 3 Knoten und füge später mehr hinzu, so kann die Entstehung beobachtet werden.");
        Console.WriteLine("\n\n\tBeliebige Taste zum fortfahren.");
        Console.ReadKey();
        Console.Clear();
    }


    #region Selection Process
    
    private static void Initialize()
    {
        var array = NumberInput().ToArray();
        InsertArray(array);
    }
    private static IEnumerable<int> NumberInput()
    { 
        _inputType = InputType.Number;
        Console.WriteLine("\n\n\tErstelle einen AVL-Baum:");
        Console.WriteLine("\n\t1 Manuelle Eingabe\n\t2 Zufallszahlen\n");
        switch (KeyInput())
        {
            case ConsoleKey.D1:
                return ManualInput();
            case ConsoleKey.D2:
                return RandomInput();
        }

        return null!;
    }

    private static void InsertArray(IEnumerable<int> array)
    {
        foreach (var number in array)
        {
            Tree.Add(number);
        }
    }

    private static IEnumerable<int> ManualInput()
    {
        Console.WriteLine("\n\n\tFüge Zahlen zum sortieren hinzu(Leerzeichen, Komma oder Punkt zum Trennen): ");
        var separators = new[] { ' ', ',', '.', ';', ':' };
        var elements = Console.ReadLine()!.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        ClearCurrentConsoleLine();
        
        return elements.Select(int.Parse).ToArray();
    }
    
    private static IEnumerable<int> RandomInput()
    {
        _isRandomInput = true;
        Console.WriteLine("\n\n\tWähle eine Ganzzahl von 3 bis 42 die generiert werden sollen: ");
        var array = GenerateNumbers(ValidNumber());
        
        return array;
    }

    #endregion

    #region Options

     private static void UserOptions()
    {
        _inputType = InputType.Options;
        Console.WriteLine("\n\n\tWähle eine Option: ");
        Console.WriteLine("\n\n\t1 Knoten hinzufügen \n\t2 Knoten Suchen/Ausgeben\n\t3 Knoten-Abfrage\n\t4 Löschen eines Knoten" +
                          "\n\t5 Die maximale Höhe ausgeben\n\t6 Die Anzahl der Knoten ausgeben\n\t7 Baum ausgeben(einfache Grafik)" +
                          "\n\t8 Baum ausgeben(grafische Visualisierung)\n\t9 Programm beenden");
        switch (KeyInput())
        {
            case ConsoleKey.D1:
                Add();
                break;
            case ConsoleKey.D2:
                GetFoundNode();  
                break;
            case ConsoleKey.D3:
                FindNode();
                break;
            case ConsoleKey.D4:
                ClearNode();
                break;
            case ConsoleKey.D5:
                GetMaxHeight();
                break;
            case ConsoleKey.D6:
                GetCount();
                break;
            case ConsoleKey.D7:
                PrintOptions();
                UserOptions();
                break;
            case ConsoleKey.D8:
                PrintVisual();
                break;
            case ConsoleKey.D9:
                Environment.Exit(0);
                break;
        }
    }
    
    private static void PrintVisual()
    {
        Tree.VisualTree();
        UserOptions();
    }

    private static void GetMaxHeight()
    {
        Tree.GetMaxDepth();
        UserOptions();
    }


    private static void GetCount()
    {
        Tree.GetCount();
        UserOptions();
    }

    private static void Add()
    {
        _isRandomInput = false;
        Console.WriteLine("\n\tGebe eine Zahl zwischen 1 und 100 ein, um sie als Knoten einzufügen.");
        Tree.Add(ValidNumber());
        PrintOptions();
        UserOptions();
    }

    private static void FindNode()
    {
        _isRandomInput = false;
        PrintOptions();
        Console.WriteLine("\n\tGebe eine Zahl zwischen 1 und 100 ein, um herauszufinden ob der Knoten vorhanden ist.");
        Tree.Contains(ValidNumber());
        UserOptions();
    }

    private static void GetFoundNode()
    {
        _isRandomInput = false;
        PrintOptions();
        Console.WriteLine("\n\tSuchen/Finden eines vorhandenen Knoten in dem die Zahl des Knotens eingegeben wird: \n");
        Tree.GetNode(ValidNumber());
        UserOptions();
    }

    private static void ClearNode()
    {
        _isRandomInput = false;
        PrintOptions();
        Console.WriteLine("\n\tLösche einen vorhandenen Knoten in dem die Zahl des Knotens eingegeben wird: \n");
        Tree.RemoveNode(ValidNumber());
        Console.WriteLine("\n\tEin aktualisierter Baum wird gezeichnet.");
        PrintOptions();
        UserOptions();
    }
    
    private static void PrintOptions()
    {
        _inputType = InputType.Number;
        Console.WriteLine("\n\n\tWähle wie der Baum dargestellt werden soll: ");
        Console.WriteLine("\n\n\t1 Mit Balance Factor\n\t2 Ohne Balance Factor\n");
        switch (KeyInput())
        {
            case ConsoleKey.D1:
                AvlTree<int>.SetPrintWithBalanceFactor(ConsoleKey.D1);
                break;
            case ConsoleKey.D2:
                AvlTree<int>.SetPrintWithBalanceFactor(ConsoleKey.D2);
                break;
        }
        
        Tree.PrintTree();
    }

    #endregion

    #region User Inputs
    
    private static ConsoleKey KeyInput()
    {
        var inputKey = Console.ReadKey().Key;

        while (!ValidInput(inputKey)) // Input validation 
        {
            Console.WriteLine("\tDie Eingabe war keine Ganzzahl oder außerhalb des Bereiches.");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            
            inputKey = Console.ReadKey().Key;
        }

        Console.ReadKey();
        Console.Clear();

        return inputKey;
    }

    private static int ValidNumber()
    {
        var success = int.TryParse(Console.ReadLine(), out var inputValue);
        bool dependingValue;
        if (_isRandomInput) dependingValue = inputValue is >= 3 and <= 42;
        else dependingValue = inputValue is >= 1 and <= 100;
        var valid = success && dependingValue;
        while (!valid)
        {
            Console.WriteLine("\tDie Eingabe war keine Ganzzahl und/oder nicht zwischen 3 und 42.");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            success = int.TryParse(Console.ReadLine(), out inputValue);
            if (_isRandomInput) dependingValue = inputValue is >= 3 and <= 42;
            else dependingValue = inputValue is >= 1 and <= 100;
            valid = success && dependingValue;
        }
        
        return inputValue;
    }

    private static bool ValidInput(ConsoleKey input)
    {
        switch (_inputType)
        {
            case InputType.Number or InputType.Restart:
                return input is ConsoleKey.D1 or ConsoleKey.D2;
            case InputType.Options:
                return input is ConsoleKey.D1 or ConsoleKey.D2 or ConsoleKey.D3 or
                    ConsoleKey.D4 or ConsoleKey.D5 or ConsoleKey.D6 or 
                    ConsoleKey.D7 or ConsoleKey.D8 or ConsoleKey.D9;
        }
        
        return false;
    }

    private static IEnumerable<int> GenerateNumbers(int elementSize)
    {
        var numbers = new int[elementSize];
        var range = new Random();

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = range.Next(1, 101);
        }

        return numbers;
    }
    

    #endregion

    private static void ClearCurrentConsoleLine()
    {
        var currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }
}