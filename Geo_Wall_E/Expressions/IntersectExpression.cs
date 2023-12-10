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

            //Punto, circulo
            if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Circle c;
                if (figure1 is Point) { p = (Point)figure1; c = (Circle)figure2; }
                else { c = (Circle)figure1; p = (Point)figure2; }
                // Calcula la distancia entre el centro de la circunferencia y el punto usando la ecuación de la circunferencia
                double distance = Math.Abs(Math.Sqrt(Math.Pow(p.X - c.Center.X, 2) + Math.Pow(p.Y - c.Center.Y, 2)));
                // Si esta es igual al radio de la circunferencia entonces el punto esta contenido 
                if (distance == Math.Abs(c.Radius.Measure_))
                {
                    points.Add(p);
                }
                return new Sequence(points, "");
            }
            //Linea, circulo
            else if (figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Line)
            {
                Line l;
                Circle c;
                if (figure1 is Line) { l = (Line)figure1; c = (Circle)figure2; }
                else { c = (Circle)figure1; l = (Line)figure2; }
                points = IntersectCircleLine(c.Center, c.Radius, l.P1, l.P2);
                return new Sequence(points, "");
            }
            //Ray, circulo
            else if (figure1.TypeOfElement == TypeOfElement.Ray && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Ray)
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
            //Segmento, circulo
            else if (figure1.TypeOfElement == TypeOfElement.Segment && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Segment)
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
            //Arco, circulo
            else if (figure1.TypeOfElement == TypeOfElement.Arc && figure2.TypeOfElement == TypeOfElement.Circle || figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Arc)
            {
                Arc a1;
                Circle c;
                if (figure1 is Arc) { a1 = (Arc)figure1; c = (Circle)figure2; }
                else { c = (Circle)figure1; a1 = (Arc)figure2; }

                double inicialAng1 = Math.Atan2(a1.Start.Y - a1.Center.Y, a1.Start.X - a1.Center.X);
                double endAng1 = Math.Atan2(a1.End.Y - a1.Center.Y, a1.End.X - a1.Center.X);
                points = IntersectCircles(a1.Center, a1.Measure, c.Center, c.Radius);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    //Angulo del punto de intersección respecto al arco
                    double angulo = Math.Atan2(p.Y - a1.Center.Y, p.X - a1.Center.X);
                    if (angulo >= inicialAng1 && angulo <= endAng1)
                    {
                        return new Sequence(points, "");
                    }
                    else
                    {
                        points.Remove(p);
                    }
                }

                if (points.Count == 2)
                {
                    Point p1 = (Point)points[0];
                    //Angulo del punto de intersección respecto al  arco
                    double angulo = Math.Atan2(p1.Y - a1.Center.Y, p1.X - a1.Center.X);
                    if (angulo >= inicialAng1 && angulo <= endAng1)
                    {
                    }
                    else
                    {
                        points.Remove(p1);
                    }

                    Point p2 = (Point)points[1];
                    //Angulo del punto de intersección respecto al  arco
                    angulo = Math.Atan2(p2.Y - a1.Center.Y, p2.X - a1.Center.X);
                    if (angulo >= inicialAng1 && angulo <= endAng1)
                        return new Sequence(points, "");

                    else
                    {
                        points.Remove(p2);
                    }

                    return new Sequence(points, "");

                }
            }
            //Circulo, circulo
            else if (figure1.TypeOfElement == TypeOfElement.Circle && figure2.TypeOfElement == TypeOfElement.Circle)
            {
                Circle c1 = (Circle)figure1;
                Circle c2 = (Circle)figure2;
                points = IntersectCircles(c1.Center, c1.Radius, c2.Center, c2.Radius);
                if (points.Count != 0)
                    return new Sequence(points, "");

            }
            //Punto, punto
            else if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p = (Point)figure1;
                Point s = (Point)figure2;
                if (p.X == s.X && p.Y == s.Y)
                {
                    points.Add(p);
                }
                return new Sequence(points, "");
            }
            //Punto, linea
            else if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Line || figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Point)
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
            //Punto, ray
            else if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Ray || figure1.TypeOfElement == TypeOfElement.Ray && figure2.TypeOfElement == TypeOfElement.Point)
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

            //Punto, segmento
            else if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Segment || figure1.TypeOfElement == TypeOfElement.Segment && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Segment s;
                if (figure1 is Segment) { s = (Segment)figure1; p = (Point)figure2; }
                else { p = (Point)figure1; s = (Segment)figure2; }
                double dx = s.End.X - s.Start.X;
                double dy = s.End.Y - s.Start.Y;
                // Verifica que el punto pertenezca a la segmento

                double intercept = ((p.X - s.Start.X) * dx + (p.Y - s.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (intercept >= 0 && intercept <= 1)
                {
                    points.Add(p);
                }
                return new Sequence(points, "");
            }

            //Punto, arco
            else if (figure1.TypeOfElement == TypeOfElement.Point && figure2.TypeOfElement == TypeOfElement.Arc || figure1.TypeOfElement == TypeOfElement.Arc && figure2.TypeOfElement == TypeOfElement.Point)
            {
                Point p;
                Arc c;
                if (figure1 is Point) { p = (Point)figure1; c = (Arc)figure2; }
                else { c = (Arc)figure1; p = (Point)figure2; }
                // Calcula la distancia entre el centro del arco y el punto usando la ecuación de la circunferencia
                double distance = Math.Abs(Math.Sqrt(Math.Pow(p.X - c.Center.X, 2) + Math.Pow(p.Y - c.Center.Y, 2)));
                // Si esta es igual al radio de la circunferencia entonces el punto esta contenido 
                if (distance == Math.Abs(c.Measure.Measure_))
                {
                    //Calcular si el punto esta entre los limites del arco
                    double arcPointsDistance = Math.Abs(Math.Sqrt(Math.Pow(c.Start.X - c.End.X, 2) + Math.Pow(c.Start.Y - c.End.Y, 2)));
                    double starPointDistance = Math.Abs(Math.Sqrt(Math.Pow(p.X - c.Start.X, 2) + Math.Pow(p.Y - c.Start.Y, 2)));
                    double endPointDistance = Math.Abs(Math.Sqrt(Math.Pow(p.X - c.End.X, 2) + Math.Pow(p.Y - c.End.Y, 2)));
                    if (arcPointsDistance == (starPointDistance + endPointDistance))
                    {
                        points.Add(p);
                    }
                }
                return new Sequence(points, "");
            }
            //Arco, arco
            else if (figure1.TypeOfElement == TypeOfElement.Arc && figure2.TypeOfElement == TypeOfElement.Arc)
            {
                Arc a1 = (Arc)figure1;
                Arc a2 = (Arc)figure2;

                double inicialAng1 = Math.Atan2(a1.Start.Y - a1.Center.Y, a1.Start.X - a1.Center.X);
                double endAng1 = Math.Atan2(a1.End.Y - a1.Center.Y, a1.End.X - a1.Center.X);

                double inicialAng2 = Math.Atan2(a2.Start.Y - a2.Center.Y, a2.Start.X - a2.Center.X);
                double endAng2 = Math.Atan2(a2.End.Y - a2.Center.Y, a2.End.X - a2.Center.X);

                points = IntersectCircles(a1.Center, a1.Measure, a2.Center, a2.Measure);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    //Angulo del punto de intersección respecto al primer arco
                    double angulo = Math.Atan2(p.Y - a1.Center.Y, p.X - a1.Center.X);
                    if (angulo >= inicialAng1 && angulo <= endAng1)
                    {
                        //Angulo del punto de intersección respecto al segundo arco
                        angulo = Math.Atan2(p.Y - a2.Center.Y, p.X - a2.Center.X);
                        if (angulo >= inicialAng2 && angulo <= endAng2)
                        {
                            return new Sequence(points, "");
                        }
                        else
                        {
                            points.Remove(p);
                        }
                    }
                    else
                    {
                        points.Remove(p);
                    }
                }
                if (points.Count == 2)
                {
                    Point p1 = (Point)points[1];
                    //Angulo del punto de intersección respecto al primer arco
                    double angulo = Math.Atan2(p1.Y - a1.Center.Y, p1.X - a1.Center.X);
                    if (angulo >= inicialAng1 && angulo <= endAng1)
                    {
                        //Angulo del punto de intersección respecto al segundo arco
                        angulo = Math.Atan2(p1.Y - a2.Center.Y, p1.X - a2.Center.X);
                        if (!(angulo >= inicialAng2) && !(angulo <= endAng2))
                        {
                            points.Remove(p1);
                        }
                    }
                    else
                    {
                        points.Remove(p1);
                    }

                    Point p2 = (Point)points[0];
                    //Angulo del punto de intersección respecto al primer arco
                    angulo = Math.Atan2(p2.Y - a1.Center.Y, p2.X - a1.Center.X);
                    if (angulo >= inicialAng1 && angulo <= endAng1)
                    {
                        //Angulo del punto de intersección respecto al segundo arco
                        angulo = Math.Atan2(p2.Y - a2.Center.Y, p2.X - a2.Center.X);
                        if (angulo >= inicialAng2 && angulo <= endAng2)
                        {
                            return new Sequence(points, "");
                        }
                        else
                        {
                            points.Remove(p2);
                        }
                    }
                    else
                    {
                        points.Remove(p2);
                    }
                    if (points.Count == 1)
                    {
                        return new Sequence(points, "");
                    }

                }

            }
            //Linea, linea 
            else if (figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Line)
            {
                Line l1 = (Line)figure1;
                Line l2 = (Line)figure2;
                points = IntersectLines(l1.P1, l1.P2, l2.P1, l2.P2);
                if (points.Count == 1)
                {
                    return new Sequence(points, "");
                }
            }
            //Linea, segmento
            else if (figure1.TypeOfElement == TypeOfElement.Segment && figure2.TypeOfElement == TypeOfElement.Line || figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Segment)
            {
                Segment s;
                Line l;
                if (figure1 is Segment) { s = (Segment)figure1; l = (Line)figure2; }
                else { l = (Line)figure1; s = (Segment)figure2; }
                points = IntersectLines(l.P1, l.P2, s.Start, s.End);
                if (points.Count == 1)
                {
                    double dx = s.End.X - s.Start.X;
                    double dy = s.End.Y - s.Start.Y;
                    Point p = (Point)points[0];
                    // Verifica que el punto pertenezca a la segmento
                    double intercept = ((p.X - s.Start.X) * dx + (p.Y - s.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                    if (!(intercept < 0) && !(intercept > 1))
                    {
                        points.Remove(p);
                    }
                    else return new Sequence(points, "");
                }
            }
            //Ray, lines
            else if (figure1.TypeOfElement == TypeOfElement.Ray && figure2.TypeOfElement == TypeOfElement.Line || figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Ray)
            {
                Ray r;
                Line l;
                if (figure1 is Segment) { r = (Ray)figure1; l = (Line)figure2; }
                else { l = (Line)figure1; r = (Ray)figure2; }
                points = IntersectLines(l.P1, l.P2, r.Start, r.Point);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    double dx = r.Point.X - r.Start.X;
                    double dy = r.Point.Y - r.Start.Y;
                    double intercept = ((p.X - r.Start.X) * dx + (p.Y - r.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                    if (intercept >= 0)
                    {
                        return new Sequence(points, "");
                    }
                    else points.Remove(p);
                }
            }
            //Linea, arco
            else if (figure1.TypeOfElement == TypeOfElement.Arc && figure2.TypeOfElement == TypeOfElement.Line || figure1.TypeOfElement == TypeOfElement.Line && figure2.TypeOfElement == TypeOfElement.Arc)
            {
                Arc a;
                Line l;
                if (figure1 is Segment) { a = (Arc)figure1; l = (Line)figure2; }
                else { l = (Line)figure1; a = (Arc)figure2; }
                points = IntersectCircleLine(a.Center, a.Measure, l.P1, l.P2);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    if (IntersectPointArc(p, a))
                        return new Sequence(points, "");
                    points.Remove(p);
                }
                if (points.Count == 2)
                {
                    Point p1 = (Point)points[0];
                    if (!IntersectPointArc(p1, a))
                    {
                        points.Remove(p1);
                    }
                    Point p2 = (Point)points[1];
                    if (IntersectPointArc(p2, a))
                        return new Sequence(points, "");
                    else points.Remove(p2);
                    if (points.Count == 1)
                        return new Sequence(points, "");
                }
            }
            //segmento, segmento
            else if (figure1.TypeOfElement == TypeOfElement.Segment && figure2.TypeOfElement == TypeOfElement.Segment)
            {
                Segment s1 = (Segment)figure1;
                Segment s2 = (Segment)figure2;
                points = IntersectLines(s1.Start, s1.End, s2.Start, s2.End);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    if (IntersectPointSegment(p, s1) && IntersectPointSegment(p, s2))
                        return new Sequence(points, "");
                    else points.Remove(p);
                }
            }
            //Segmento, ray
            else if (figure1.TypeOfElement == TypeOfElement.Ray && figure2.TypeOfElement == TypeOfElement.Segment || figure1.TypeOfElement == TypeOfElement.Segment && figure2.TypeOfElement == TypeOfElement.Ray)
            {
                Segment s;
                Ray r;
                if (figure1 is Segment) { s = (Segment)figure1; r = (Ray)figure2; }
                else { r = (Ray)figure1; s = (Segment)figure2; }
                points = IntersectLines(r.Start, r.Point, s.Start, s.End);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    if (!IntersectPointRay(p, r) || !IntersectPointSegment(p, s))
                    {
                        points.Remove(p);
                    }

                }
                return new Sequence(points, "");
            }
            //Segmento, arco
            else if (figure1.TypeOfElement == TypeOfElement.Arc && figure2.TypeOfElement == TypeOfElement.Segment || figure1.TypeOfElement == TypeOfElement.Segment && figure2.TypeOfElement == TypeOfElement.Arc)
            {
                Segment s;
                Arc a;
                if (figure1 is Segment) { s = (Segment)figure1; a = (Arc)figure2; }
                else { a = (Arc)figure1; s = (Segment)figure2; }
                points = IntersectCircleLine(a.Center, a.Measure, s.Start, s.End);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    if (!IntersectPointArc(p, a) || !IntersectPointSegment(p, s))
                    {
                        points.Remove(p);
                    }
                }
                return new Sequence(points, "");
            }

            //Ray, ray
            else if (figure1.TypeOfElement == TypeOfElement.Ray && figure2.TypeOfElement == TypeOfElement.Ray)
            {
                Ray r1 = (Ray)figure1;
                Ray r2 = (Ray)figure2;
                points = IntersectLines(r1.Start, r1.Point, r2.Start, r2.Point);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    if (!IntersectPointRay(p, r1) || !IntersectPointRay(p, r2))
                    {
                        points.Remove(p);
                    }
                }
                return new Sequence(points, "");
            }
            //Ray,Arc
            else if (figure1.TypeOfElement == TypeOfElement.Arc && figure2.TypeOfElement == TypeOfElement.Ray || figure1.TypeOfElement == TypeOfElement.Ray && figure2.TypeOfElement == TypeOfElement.Arc)
            {
                Ray r;
                Arc a;
                if (figure1 is Ray) { r = (Ray)figure1; a = (Arc)figure2; }
                else { a = (Arc)figure1; r = (Ray)figure2; }
                points = IntersectCircleLine(a.Center, a.Measure, r.Start, r.Point);
                if (points.Count == 1)
                {
                    Point p = (Point)points[0];
                    if (!IntersectPointArc(p, a) || !IntersectPointRay(p, r))
                    {
                        points.Remove(p);
                    }
                }
                return new Sequence(points, "");
            }


            throw new TypeCheckerError(0, 0, "No es posible calcular la intersección entre " + Figure1 + " y " + Figure2);
        }

        bool IntersectPointSegment(Point p, Segment s)
        {
            double dx = s.End.X - s.Start.X;
            double dy = s.End.Y - s.Start.Y;
            // Verifica que el punto pertenezca a la segmento
            double intercept = ((p.X - s.Start.X) * dx + (p.Y - s.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
            if (intercept < 0 || intercept > 1)
            {
                return false;
            }
            else return true;
        }

        bool IntersectPointRay(Point p, Ray r)
        {
            double dx = r.Point.X - r.Start.X;
            double dy = r.Point.Y - r.Start.Y;
            double intercept = ((p.X - r.Start.X) * dx + (p.Y - r.Start.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
            if (intercept >= 0)
            {
                return true;
            }
            else return false;
        }

        bool IntersectPointArc(Point p, Arc a1)
        {
            double inicialAng1 = Math.Atan2(a1.Start.Y - a1.Center.Y, a1.Start.X - a1.Center.X);
            double endAng1 = Math.Atan2(a1.End.Y - a1.Center.Y, a1.End.X - a1.Center.X);
            double angulo = Math.Atan2(p.Y - a1.Center.Y, p.X - a1.Center.X);
            if (angulo >= inicialAng1 && angulo <= endAng1)
            {
                return true;
            }
            else return false;
        }

        List<Type> IntersectCircleLine(Point Center, Measure Radius, Point P1, Point P2)
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

        List<Type> IntersectCircles(Point Center1, Measure Radius1, Point Center2, Measure Radius2)
        {
            List<Type> points = new();
            //distancia entre los centros de las circunferencias
            double d = Math.Sqrt(Math.Pow(Center2.X - Center1.X, 2) + Math.Pow(Center2.Y - Center1.Y, 2));
            //si la distancia es igual a la suma de los radios entonces son tangentes
            if (d == Radius1.Measure_ + Radius2.Measure_)
            {
                double x = (Radius1.Measure_ * Center2.X - Radius2.Measure_ * Center1.X) / (Radius1.Measure_ - Radius2.Measure_);
                double y = (Radius1.Measure_ * Center2.Y - Radius2.Measure_ * Center1.Y) / (Radius1.Measure_ - Radius2.Measure_);
                Point p = new("")
                {
                    X = x,
                    Y = y
                };
                points.Add(p);
            }
            //en el primer caso uno de los círculos estaría contenido dentro del otro y en el otro no se intersectan
            else if (d > Math.Abs(Radius1.Measure_ - Radius2.Measure_) && d < Radius1.Measure_ + Radius2.Measure_)
            {
                double a = (Math.Pow(Radius1.Measure_, 2) - Math.Pow(Radius2.Measure_, 2) + Math.Pow(d, 2)) / (2 * d);
                double h = Math.Sqrt(Math.Pow(Radius1.Measure_, 2) - Math.Pow(a, 2));

                double x2 = Center1.X + a * (Center2.X - Center1.X) / d;
                double y2 = Center1.Y + a * (Center2.Y - Center1.Y) / d;

                Point p1 = new("")
                {
                    X = x2 + h * (Center2.Y - Center1.Y) / d,
                    Y = y2 - h * (Center2.X - Center1.X) / d
                };
                Point p2 = new("")
                {
                    X = x2 - h * (Center2.Y - Center1.Y) / d,
                    Y = y2 + h * (Center2.X - Center1.X) / d
                };
                points.Add(p1);
                points.Add(p2);
            }
            return points;
        }

        List<Type> IntersectLines(Point firstlinep1, Point firstlinep2, Point secondtlinep1, Point secondtlinep2)
        {
            List<Type> points = new();
            //Hallar pendiente de la primera linea
            double m1 = (firstlinep1.Y - firstlinep2.Y) / (firstlinep1.X - firstlinep2.X);
            //Hallar punto medio de la primera linea
            //Para coordenadas X
            double xMiddle1 = (firstlinep1.X + firstlinep2.X) / 2;
            //Para coordenadas Y
            double yMiddle1 = (firstlinep1.Y + firstlinep2.Y) / 2;

            //Hallar pendiente de la segunda linea
            double m2 = (secondtlinep1.Y - secondtlinep2.Y) / (secondtlinep1.X - secondtlinep2.X);
            //Hallar punto medio de la primera linea
            //Para coordenadas X
            double xMiddle2 = (secondtlinep1.X + secondtlinep2.X) / 2;
            //Para coordenadas Y
            double yMiddle2 = (secondtlinep1.Y + secondtlinep2.Y) / 2;
            if (m1 != m2)
            {
                // Coordenada x del punto de intersección
                double x = (yMiddle2 - yMiddle1 + m1 * xMiddle1 - m2 * xMiddle1) / (m1 - m2);
                // Coordenada y del punto de intersección
                double y = yMiddle1 + m1 * (x - xMiddle1);
                Point intersect = new("")
                {
                    X = x,
                    Y = y
                };
                points.Add(intersect);
            }
            return points;
        }
    }
}
