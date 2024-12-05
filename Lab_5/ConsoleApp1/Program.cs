using System.Reflection;
using System.Reflection.Metadata.Ecma335;

class Program
{
    static void Main(string[] args)
    {
        Square square = new Square();
        Circle circle = new Circle();
        Console.WriteLine(square.CalculateArea(10, 10));
        Console.WriteLine(circle.CalculateArea(3));
    }
}

abstract class Shape
{
    public abstract object CalculateArea(int width= 0, int lenth = 0, int radius = 0);
}

class Square : Shape
{
    public override object CalculateArea(int width, int lenth, int radious = 0)
    {
        return width * lenth;
    }
}

class Circle : Shape {
    public override object CalculateArea(int radious, int width = 0, int lenth = 0)
    {
        double result = 3.14 * radious * radious;
        return result;
    }
}

interface IVehile
{
    void Start();
    void Stop();
    int MaxSpeed
    {
        get;
        set;
    }
}

class Car : IVehile
{
    public int MaxSpeed {
        get;
        set;
    }

    public void Start()
    {
        Console.WriteLine("Started");
    }

    public void Stop()
    {
        Console.WriteLine("Stopped");
    }
}