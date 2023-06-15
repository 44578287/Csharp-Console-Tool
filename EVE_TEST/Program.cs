// See https://aka.ms/new-console-template for more information



int x = 0, y = 0;

while (true)
{
    x++;
    for (int i = 0; i < x / 1; i++)
    {
        Console.Write(" ");
    }
    Console.WriteLine(x);

    Console.WriteLine("");
    y += 2;
    for (int i = 0; i < y / 1; i++)
    {
        Console.Write(" ");
    }
    Console.WriteLine(y);

    Thread.Sleep(1000);
    Console.Clear();
}