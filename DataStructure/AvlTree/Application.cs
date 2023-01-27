using System.Diagnostics;

namespace AvlTree;

public static class Application
{
    private static readonly AvlTree<int> Tree = new();
    
    private enum InputType { Number, Options, Restart }
    private static InputType inputType;

    private static readonly Stopwatch Timer = new ();    
    
    public static void Run()
    {
        Selection();
    }

    private static void Selection()
    {
        Intro();
        Initialize();
        // TODO: UserOptions(); menu erweitern
    }
    
    private static void Initialize()
    {
        var array = NumberInput().ToArray();
        InsertArray(array);
        Tree.PrintTree();
    }

    private static void Intro()
    {
        Console.WriteLine("\n\n\t\t\tAVL-BAUM\n");
        Console.WriteLine();
        Console.WriteLine("\n\tDie Zahlen können manuell eingegeben oder generiert werden.\n");
        Console.WriteLine("\n\tDanach ist es möglich: - einen Knoten zu Suchen und zu Finden" +
                            "\n\t\t\t       - abfagen ob ein bestimmter Wert vorhanden ist\n" +
                            "\t\t\t       - einen Knoten zu Löschen\n" +
                            "\t\t\t       - einen Knoten hinzuzufügen\n" +
                            "\t\t\t       - die Höhe des Baumes auszugeben\n" +
                            "\t\t\t       - die Höhe des Baumes zu auszugeben\n" +
                            "\t\t\t       - die Anzahl der Knoten zu ermitteln\n");
        Console.WriteLine("\n\n\tBeliebige Taste zum fortfahren.");
        Console.ReadKey();
        Console.Clear();
    }
    
    private static int[] NumberInput()
    { 
        inputType = InputType.Number;
        Console.WriteLine("\n\n\tErstelle einen AVL-Baum:");
        Console.WriteLine("\n\t1 manuelle Eingabe\n\t2 Zufallszahlen\n");
        switch (KeyInput())
        {
            case ConsoleKey.D1:
                return ManualInput();
            case ConsoleKey.D2:
                return RandomInput();
        }

        return null!;
    }

    private static void InsertArray(int[] array)
    {
        foreach (var number in array)
        {
            Tree.Insert(number);
        }
    }

    private static int[] ManualInput()
    {
        Console.WriteLine("\n\n\tFüge Zahlen zum sortieren hinzu(Leerzeichen, Komma oder Punkt zum Trennen): ");
        var separators = new[] { ' ', ',', '.', ';', ':' };
        var elements = Console.ReadLine()!.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        ClearCurrentConsoleLine();
        
        return elements.Select(int.Parse).ToArray();
    }
    
    private static int[] RandomInput()
    {
        Console.WriteLine("\n\n\tWähle eine Ganzzahl von 10 bis 1000 die generiert werden sollen: ");
        var array = GenerateNumbers(ValidNumber());
        
        return array;
    }
    
    private static void UserOptions(int data)
    {
        inputType = InputType.Options;
        Console.WriteLine("\n\n\tWähle eine Option: ");
        Console.WriteLine("\n\n\t1 für Knoten Suchen/Ausgeben\n\t2 für Knoten-Abfrage\n\t\n\t3 für Löschen" +
                          "\n\t4 für Höhe ausgeben\n\t5 für Anzahl der Knoten ausgeben\n\t6 für Baum ausgeben(einfache Grafik)" +
                          "\n\t7 für Baum ausgeben(grafische Visualisierung)\n\t8 für Baum zurücksetzen\n\t9 für Programm beenden");
        switch (KeyInput())
        {
            case ConsoleKey.D1:
                Tree.Search(data);
                break;
            case ConsoleKey.D2:
                Tree.Find(data);
                break;
            case ConsoleKey.D3:
                Tree.Clear(data);
                break;
            case ConsoleKey.D4:
                Tree.GetMaxDepth(Tree.Root);
                break;
            case ConsoleKey.D5:
                Tree.GetCount();
                break;
            case ConsoleKey.D6:
                Tree.PrintTree();
                break;
            case ConsoleKey.D7:
                Tree.VisualTree();
                break;
            case ConsoleKey.D8:
                Tree.ClearTree(); 
                break;
            case ConsoleKey.D9:
                Environment.Exit(0);
                break;
        }
    }

    private static ConsoleKey KeyInput()
    {
        var inputKey = Console.ReadKey().Key;

        while (!ValidInput(inputKey)) // Input validation 
        {
            Console.WriteLine("\tDie Eingabe war keine Ganzzahl.");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            
            inputKey = Console.ReadKey().Key;
        }

        Console.ReadKey();
        Console.Clear();

        return inputKey;
    }

    private static int ValidNumber()
    {
        bool success = int.TryParse(Console.ReadLine(), out var inputValue);
        bool valid = success && inputValue is >= 10 and <= 1000;
        while (!valid)
        {
            Console.WriteLine("\tDie Eingabe war keine Ganzzahl und/oder nicht zwischen 10 und 1000.");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            success = int.TryParse(Console.ReadLine(), out inputValue);
            valid = success && inputValue is >= 10 and <= 1000;
        }
        
        return inputValue;
    }

    private static bool ValidInput(ConsoleKey input)
    {
        switch (inputType)
        {
            case InputType.Number or InputType.Restart:
                return input is ConsoleKey.D1 or ConsoleKey.D2;
            case InputType.Options:
                return input is ConsoleKey.D1 or ConsoleKey.D2 or 
                    ConsoleKey.D4 or ConsoleKey.D5 or ConsoleKey.D6 or ConsoleKey.D7 or ConsoleKey.D8 or ConsoleKey.D9;
        }
        
        return false;
    }
    
    private static void RestartOptions()
    {
        inputType = InputType.Restart;
        Console.WriteLine($"\n\n\n\t1. Neu starten\n\t2. Beenden\n");
        switch (KeyInput())
        {
            case ConsoleKey.D1:
                Console.Clear();
                Run();
                break;
            case ConsoleKey.D2:
                Environment.Exit(0);
                break;
        }
    }

    private static int[] GenerateNumbers(int elementSize)
    {
        var numbers = new int[elementSize];
        var range = new Random();

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = range.Next(1, 1000);
        }

        return numbers;
    }
    
    private static void ClearCurrentConsoleLine()
    {
        var currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }
}