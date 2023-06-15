using 异步;

多线程.ThreadManagement ThreadManagement = new(new() { new(() => { Console.WriteLine("AAAA"); }) });
ThreadManagement.Start();


//Parallel.Invoke(A,B,C);
//Parallel.For(0,100,index => { Console.WriteLine(index); });
/*List<string> ProductList = C();
Parallel.ForEach(ProductList, (model) =>
{
    Console.WriteLine(model);
});
Console.WriteLine("------------------------------------");
foreach (var Data in ProductList)
{ 
    Console.WriteLine(Data);
}

void A()
{
    Console.WriteLine("A");
}
void B()
{
    Console.WriteLine("B");
}
List<string> C()
{
    List<string> C = new List<string>();
    for (int i = 0;i<10;i++)
        C.Add(i.ToString());
    return C;
}
*/