namespace Geo_Wall_E
{
    
    public class Measure : Type
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Measure;
        public Point P1 { get; private set;  }
        public Point P2 { get; private set;  }
        public string Name { get; private set;  }
        public double Measure_ { get; private set;  }

        public Measure(Point p1, Point p2, string name)
        {
            P1 = p1;
            P2 = p2;
            Name = name;
            Measure_ = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));
        }

        public Measure(double measure, string name)
        {
            P1 = new Point("");
            P2 = new Point("");
            Name = name;
            Measure_ = measure;
        }
        public double MeasureBetweenPoints()
        {
            double measure = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));
            return measure;
        }
    }
}