namespace Geo_Wall_E
{
    public class DrawStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.DrawToken;
        public string Name { get; private set; }
        public Node? Expression { get; private set; }
        public List<VariableExpression>? ExpressionSequence { get; private set; }
        public Color Color { get; private set;}
        public DrawStmt(string name, Node exp, Color color)
        {
            Name = name;
            Expression = exp;
            Color = color;
        }
        public DrawStmt(string name, List<VariableExpression> expSequence, Color color)
        {
            Name = name;
            ExpressionSequence = expSequence;
            Color = color;
        }
    }
}