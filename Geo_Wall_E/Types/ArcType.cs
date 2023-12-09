namespace Geo_Wall_E
{
    public class Arc : Type, IDrawable
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Arc;
        public Point Center { get; private set;  }
        public Point Start { get; private set;  }
        public Point End { get; private set;  }
        public Measure Measure { get; private set;  }
        public string Name { get; private set;  }

        public Arc(Point center, Point start, Point end, Measure measure, string name)
        {
            Center = center;
            Start = start;
            End = end;
            Measure = measure;
            Name = name;
        }
        Geo_Wall_E.Type IDrawable.Type => new Arc(Center,Start,End,Measure,Name);

        Color IDrawable.Color => Color.BLACK;

        string IDrawable.Phrase => Name;
    }
}