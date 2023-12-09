namespace Geo_Wall_E
{
    public class BetweenParenExpressions : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.BetweenParenExpression;
        public Expressions Expressions { get; private set; }

        public BetweenParenExpressions(Expressions exp)
        {
            Expressions = exp;
        }

        public Type Check(Scope scope)
        {
            return ((ICheckType)Expressions).Check(scope);
        }
    }
    public class Randoms : Expressions
    {
        public override TypesOfToken Type => TypesOfToken.RandomsToken;
        public Sequence RandomSequence { get; private set; }

        public Randoms()
        {
            List<Type> randoms = GenerateRandom();
            RandomSequence = new Sequence(randoms, "");
        }

        private List<Type> GenerateRandom()
        {
            List<Type> randoms = new();
            Random cant = new();
            Random numbers = new();
            for (int i = 0; i < cant.Next(0, 100); i++)
            {
                randoms.Add(new Number(numbers.NextDouble()));
            }
            return randoms;
        }

    }
    public class RandomPoints : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.PointsToken;
        public Expressions Figure { get; private set; }

        public RandomPoints(Expressions figure)
        {
            Figure = figure;
        }

        public Type Check(Scope scope)
        {
            Type fig = ((ICheckType)Figure).Check(scope);
            List<Type> points = CreateRandomPoints(fig);
            return new Sequence(points, "");
        }
        private List<Type> CreateRandomPoints(Type figure)
        {
            List<Type> randoms = new();
            Random random = new();
            for (int i = 0; i <  100000; i++)
            {
                Point point = new("");
                point.X = random.Next(0,1000);
                point.Y = random.Next(0,1000);
                bool isInFigure = Belongs(point, figure);
                if (isInFigure) randoms.Add(new Point(""));
                else continue;
            }
            return randoms;
        }

        private bool Belongs(Point p, Type figure)
        {
            if (figure is Line l)
            {
                if (p.Y - l.P1.Y == (l.P2.Y - l.P1.Y) / (l.P2.X - l.P1.X) * (p.X - l.P1.X)) return true;
                else return false;
            }
            else if (figure is Ray r)
            {
                double intercept = ((p.X - r.Start.X) * r.Point.X - r.Start.X + (p.Y - r.Start.Y) * r.Point.Y - r.Start.Y) / (Math.Pow(r.Point.X - r.Start.X, 2) + Math.Pow(r.Point.Y - r.Start.Y, 2));
                if (intercept >= 0) return true;
                else return false;
            }
            else if (figure is Segment s)
            {
                double intercept = ((p.X - s.Start.X) * s.End.X - s.Start.X + (p.Y - s.Start.Y) * s.End.Y - s.Start.Y) / (Math.Pow(s.End.X - s.Start.X, 2) + Math.Pow(s.End.Y - s.Start.Y, 2));
                if (intercept < 0 || intercept > 1) return true;
                else return false;
            }
            else if (figure is Circle c)
            {
                if (Math.Sqrt(Math.Pow(c.Center.X - p.X, 2) + Math.Pow(c.Center.Y - p.Y, 2)) == c.Radius.Measure_) return true;
                else return false;
            }
            else if (figure is Arc a)
            {
                // Calcula la distancia entre el centro del arco y el punto usando la ecuación de la circunferencia
                double distance = Math.Abs(Math.Sqrt(Math.Pow(p.X - a.Center.X, 2) + Math.Pow(p.Y - a.Center.Y, 2)));
                // Si esta es igual al radio de la circunferncia entonces el punto esta contenido 
                if (distance == Math.Abs(a.Measure.Measure_))
                {
                    //Calcular si el punto esta entre los limites del arco
                    double arcPointsDistance = Math.Abs(Math.Sqrt(Math.Pow(a.Start.X - a.End.X, 2) + Math.Pow(a.Start.Y - a.End.Y, 2)));
                    double starPointDistance = Math.Abs(Math.Sqrt(Math.Pow(p.X - a.Start.X, 2) + Math.Pow(p.Y - a.Start.Y, 2)));
                    double endPointDistance = Math.Abs(Math.Sqrt(Math.Pow(p.X - a.End.X, 2) + Math.Pow(p.Y - a.End.Y, 2)));
                    if (arcPointsDistance == (starPointDistance + endPointDistance))
                    {
                        return true;
                    }
                    return false;
                }
                else return false;
            }
            else throw new TypeCheckerError(0, 0, "No es posible realizar el intercepto");
        }
    }
    public class Samples : Expressions
    {
        public override TypesOfToken Type => TypesOfToken.SamplesToken;
        public Sequence Sequence { get; private set; }

        public Samples()
        {
            List<Type> points = GenerateSamples();
            Sequence = new Sequence(points, "");
        }
        private List<Type> GenerateSamples()
        {
            List<Type> randoms = new();
            Random cant = new();
            for (int i = 0; i < cant.Next(0, 100); i++)
            {
                randoms.Add(new Point(""));
            }
            return randoms;
        }
    }
    public class Count : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.CountToken;
        public Expressions Sequence { get; private set; }

        public Count(Expressions sequence)
        {
            Sequence = sequence;
        }

        public Type Check(Scope scope)
        {
            Type sequence = ((ICheckType)Sequence).Check(scope);
            if (sequence is Sequence seq)
            {
                if (seq.Elements.Count != 0 || seq.Elements.Count <= 100) return new Number(seq.Elements.Count);
                else return new Undefined();
            }
            else throw new TypeCheckerError(0, 0, "Se esperaba una secuencia al realizar la operación count()");
        }
    }
}
