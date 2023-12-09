
namespace Geo_Wall_E
{
    public class IntersectExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.IntersectToken;
        public Expressions Figure1 { get; private set; }
        public Expressions Figure2 { get; private set; }
        public IntersectExpression(Expressions f1, Expressions f2)
        {
            Figure1 = f1;
            Figure2 = f2;
        }

        public Type Check(Scope scope)
        {
            Type figure1 = ((ICheckType)Figure1).Check(scope);
            Type figure2 = ((ICheckType)Figure2).Check(scope);
            List<Type> points = new();
            if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Circle c;
                if (figure1 is Point) { p = (Point)figure1; c = (Circle)figure2; }
                else { c = (Circle)figure1; p = (Point)figure2; }
                // Calcula la distancia entre el centro de la circunferencia y el punto usando la ecuación de la circunferencia
                double distance = Math.Abs(Math.Sqrt(Math.Pow(p.X - c.Center.X, 2) + Math.Pow(p.Y - c.Center.Y, 2)));
                // Si esta es igual al radio de la circunferncia entonces el punto esta contenido 
                if (distance == Math.Abs(c.Radius.Measure_))
                {
                    points.Add(p);
                }
                return new Sequence(points, "");
            }
            if (figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Line)
            {
                Line l;
                Circle c;
                if (figure1 is Line) { l = (Line)figure1; c = (Circle)figure2; }
                else { c = (Circle)figure1; l = (Line)figure2; }
                points = IntersectCircleLine(c.Center, c.Radius, l.P1, l.P2);
                return new Sequence(points, "");
            }
            if (figure1.TypeOfElement == TypeOfElement.Ray && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Ray)
            {
                Ray r;
                Circle c;
                if (figure1 is Ray) { r = (Ray)figure1; c = (Circle)figure2; }
                else { c = (Circle)figure1; r = (Ray)figure2; }
                points = IntersectCircleLine(c.Center, c.Radius, r.Start, r.Point);
                double dx = r.Point.X - r.Start.X;
                double dy = r.Point.Y - r.Start.Y;
                // Verifica que el punto pertenezca a la semirrecta
                foreach (var point in points)
                {
                    double intercept = ((((Point)point).X - r.Start.X) * dx + (((Point)point).Y - r.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                    if (intercept < 0)
                    {
                        points.Remove(point);
                    }
                }
                return new Sequence(points, "");
            }
            if (figure1.TypeOfElement == TypeOfElement.Segment && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Segment)
            {
                Segment s;
                Circle c;
                if (figure1 is Line) { s = (Segment)figure1; c = (Circle)figure2; }
                else { c = (Circle)figure1; s = (Segment)figure2; }
                points = IntersectCircleLine(c.Center, c.Radius, s.Start, s.End);
                double dx = s.End.X - s.Start.X;
                double dy = s.End.Y - s.Start.Y;
                // Verifica que el punto pertenezca a la segmento
                foreach (var point in points)
                {
                    double intercept = ((((Point)point).X - s.Start.X) * dx + (((Point)point).Y - s.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                    if (intercept < 0 || intercept > 1)
                    {
                        points.Remove(point);
                    }
                }
                return new Sequence(points, "");
            }
            if (figure1.TypeOfElement == TypeOfElement.Arc && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Arc)
            {

            }
            if (figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Circle)
            {

            }
            if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Line || figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Line l;
                if (figure1 is Point) { p = (Point)figure1; l = (Line)figure2; }
                else { l = (Line)figure1; p = (Point)figure2; }
                if (p.Y - l.P1.Y == (l.P2.Y - l.P1.Y) / (l.P2.X - l.P1.X) * (p.X - l.P1.X))
                {
                    points.Add(p);
                }
                return new Sequence(points, "");
            }
            if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Ray || figure1.TypeOfElement == TypeOfElement.Ray && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Ray r;
                if (figure1 is Point) { p = (Point)figure1; r = (Ray)figure2; }
                else { r = (Ray)figure1; p = (Point)figure2; }
                double dx = r.Point.X - r.Start.X;
                double dy = r.Point.Y - r.Start.Y;
                double intercept = ((p.X - r.Start.X) * dx + (p.Y - r.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (intercept >= 0)
                {
                    points.Add(p);
                }
                return new Sequence(points, "");
            }
            if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Segment || figure1.TypeOfElement == TypeOfElement.Segment && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Segment s;
                if (figure1 is Segment) { s = (Segment)figure1; p = (Point)figure2; }
                else { p = (Point)figure1; s = (Segment)figure2; }
                double dx = s.End.X - s.Start.X;
                double dy = s.End.Y - s.Start.Y;
                // Verifica que el punto pertenezca a la segmento

                double intercept = ((p.X - s.Start.X) * dx + (p.Y - s.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (intercept < 0 || intercept > 1)
                {
                    points.Remove(p);
                }
                return new Sequence(points, "");
            }
            if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Line || figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Line l;
                if (figure1 is Point) { p = (Point)figure1; l = (Line)figure2; }
                else { l = (Line)figure1; p = (Point)figure2; }
                if (p.Y - l.P1.Y == (l.P2.Y - l.P1.Y) / (l.P2.X - l.P1.X) * (p.X - l.P1.X))
                {
                    points.Add(p);
                }
                return new Sequence(points, "");
            }
            if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Line || figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Line l;
                if (figure1 is Point) { p = (Point)figure1; l = (Line)figure2; }
                else { l = (Line)figure1; p = (Point)figure2; }
                if (p.Y - l.P1.Y == (l.P2.Y - l.P1.Y) / (l.P2.X - l.P1.X) * (p.X - l.P1.X))
                {
                    points.Add(p);
                }
                return new Sequence(points, "");
            }
            throw new TypeCheckerError(0, 0, "No es posible calcular la intersección entre " + Figure1 + " y " + Figure2);
        }
        private List<Type> IntersectCircleLine(Point Center, Measure Radius, Point P1, Point P2)
        {
            List<Type> points = new();
            // m es la pendiente de la recta 
            double m = (P2.Y - P1.Y) / (P2.X - P1.X);
            // ecuación de la recta 
            double b = P1.Y - m * P1.X;
            // coordenada x del centro de la circunferencia
            double h = Center.X;
            // coordenada y del centro de la circunferencia
            double k = Center.Y;
            // radio de la circunferencia
            double r = Radius.Measure_;
            // A, B y C son coeficientes de la ecuación cuadrática que se obtiene al sustituir la ecuación de la recta en la ecuación de la circunferencia para encontrar las coordenadas de los puntos de intersección 
            double A = 1 + Math.Pow(m, 2);
            double B = -2 * h + 2 * m * b - 2 * k * m;
            double C = Math.Pow(h, 2) + Math.Pow(b, 2) + Math.Pow(k, 2) - Math.Pow(r, 2) - 2 * b * k;
            double discriminant = Math.Pow(B, 2) - 4.0 * A * C;

            // Si el discriminante es negativo no existe intersección
            // Si el discriminante es cero existe un solo punto de intersección
            if (discriminant == 0)
            {
                double x = -B / (2 * A);
                double y = m * x + b;
                Point intersect = new("")
                {
                    X = x,
                    Y = y
                };
                points.Add(intersect);
            }
            // Si el discriminante es positivo existen dos puntos de intersección
            else
            {
                double x1 = (-B + Math.Sqrt(discriminant)) / (2.0 * A);
                double y1 = m * x1 + b;
                double x2 = (-B - Math.Sqrt(discriminant)) / (2.0 * A);
                double y2 = m * x2 + b;
                Point intersect1 = new("")
                {
                    X = x1,
                    Y = y1
                };
                Point intersect2 = new("")
                {
                    X = x2,
                    Y = y2
                };
                points.Add(intersect1);
                points.Add(intersect2);
            }
            return points;
        }
    }
}
