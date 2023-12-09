
namespace Geo_Wall_E
{
    public class CircleExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.CircleToken;
        public Expressions Center { get; private set; }
        public Expressions Radius { get; private set; }
        public CircleExpression(Expressions center, Expressions radius)
        {
            Center = center;
            Radius = radius;
        }

        public Type Check(Scope scope)
        {
            Type center = ((ICheckType)Center).Check(scope);
            Type radius = ((ICheckType)Radius).Check(scope);
            if (center.TypeOfElement != TypeOfElement.Point) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + center.TypeOfElement);
            else if (radius.TypeOfElement != TypeOfElement.Measure) throw new TypeCheckerError(0, 0, "Se esperaba un punto pero en su lugar se obtuvo " + radius.TypeOfElement);
            else return new Circle((Point)center, (Measure)radius, "");
        }
    }
}