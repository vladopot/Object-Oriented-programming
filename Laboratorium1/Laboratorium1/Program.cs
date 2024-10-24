using System;

class Programm 
{
    public static void Main()
    {
        //Ex1 firstEx = new Ex1();
        //Console.WriteLine(firstEx.isEven(int.Parse(Console.ReadLine())));
        //Ex2 secondEx = new Ex2();
        //secondEx.Nums(int.Parse(Console.ReadLine()));
        Ex3 tjitd = new Ex3();
        tjitd.ShowMenu();
    }
}

class Ex1
{
    public bool isEven(int num)
    {
        return num % 2 == 0;
    }
}

class Ex2
{
    public void Nums(int num)
    {
        int currentNum = 1;
        while (currentNum <= num)
        {
            Console.WriteLine(currentNum);
            currentNum++;
        }
    }
}

class Ex3
{
    public void ShowMenu()
    {
        List<string> menuVars = new List<string>()
        {
            "1: first Exersise",
            "2: second Exersise",
            "3: third Exersise",
            "Exit"
        };
        foreach (string var in menuVars) {
            Console.WriteLine(var);
        }
        Console.WriteLine("Choose your Exersice");
        int exNum = int.Parse(Console.ReadLine());
    }

}