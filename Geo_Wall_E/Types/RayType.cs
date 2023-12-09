namespace Geo_Wall_E
{
    public class Ray : Type, IDrawable
    {
        public override TypeOfElement TypeOfElement =>TypeOfElement.Ray;
        public Point Start { get; private set; }
        public Point Point { get; private set; }
        public string Name { get; private set; }

        public Ray(Point start, Point point, string name)
        {
            Start = start;
            Point = point;
            Name = name;
        }
        Type IDrawable.Type => new Ray(Start,Point,"");

        Color IDrawable.Color => Color.BLACK;
        string IDrawable.Phrase => Name;

    }
}