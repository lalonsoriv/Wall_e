namespace Geo_Wall_E
{
    public class Circle : Type, IDrawable
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Circle;
        public Point Center { get; private set; }
        public Measure Radius { get; private set; }
        public string Name { get; private set; }
        public Circle(Point center, Measure radius, string name)
        {
            Center = center;
            Radius = radius;
            Name = name;
        }

        Color IDrawable.Color => Color.BLACK;
        
        string IDrawable.Phrase => Name;

        Type IDrawable.Type => new Circle(Center,Radius,Name);
    }
}