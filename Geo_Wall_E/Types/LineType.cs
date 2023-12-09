namespace Geo_Wall_E
{
    public class Line : Type, IDrawable
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Line;
        public Point P1 { get; private set; }
        public Point P2 { get; private set; }
        public string Name { get; private set; }

        public Line(Point p1, Point p2, string name)
        {
            P1 = p1;
            P2 = p2;
            Name = name;
        }

        Type IDrawable.Type => new Line(P1,P2,"");

        Color IDrawable.Color => Color.BLACK;
        string IDrawable.Phrase => Name;

    }
}