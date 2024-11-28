class Programm
{
    public static void Main(String[] args)
    {
        List<Shape> Shapes = new List<Shape>();
        Shapes.Add(new Rectangle());
        Shapes.Add(new Triangle());
        Shapes.Add(new Circle());

        foreach (Shape shape in Shapes) 
        {
            Console.WriteLine(shape.Draw());
        }
    }
}

class Shape{
    public int X;
    public int Y;
    public int Height;
    public int Width;

    public virtual string Draw()
    {
        return "HYYYYYYYYYYj";
    }
}

class Rectangle : Shape
{
    public override string Draw()
    {
        return "This is Rectangle";
    }
}

class Triangle : Shape
{
    public override string Draw()
    {
        return "This is Triangle";
    }
}

class Circle : Shape
{
    public override string Draw()
    {
        return "This is Circle";
    }
}