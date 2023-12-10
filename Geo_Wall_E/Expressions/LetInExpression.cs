namespace Geo_Wall_E
{
    public class LetInExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.LetInExpression;
        public List<Stmt> Let { get; private set; }
        public Expressions In { get; private set; }
        public LetInExpression(List<Stmt> let, Expressions _in)
        {
            Let = let;
            In = _in;
        }

        public Type Check(Scope scope)
        {
            scope.SetScope();
            List<IDrawable> todraw = new();
            foreach (var stmt in Let)
            {
                Interpreter.EvaluateStmt(stmt, scope, todraw);
            }
            Type result = ((ICheckType)In).Check(scope);
            scope.DeleteScope();
            return result;
        }
    }
}
