namespace Geo_Wall_E
{
    public class Segment : Type, IDrawable
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Segment;
        public Point Start { get; private set; }
        public Point End { get; private set; }
        public string Name { get; private set; }

        public Segment(Point start, Point end, string name)
        {
            Start = start;
            End = end;
            Name = name;
        }
        Type IDrawable.Type => new Segment(Start,End,"");

        Color IDrawable.Color => Color.BLACK;
        string IDrawable.Phrase => Name;

    }
}