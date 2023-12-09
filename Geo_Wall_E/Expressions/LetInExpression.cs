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
            foreach (var stmt in Let)
            {
                SetStmt(stmt, scope);
            }
            Type result = ((ICheckType)In).Check(scope);
            scope.DeleteScope();
            return result;
        }


        public Scope SetStmt(Stmt stmt, Scope scope)
        {
            switch (stmt)
            {
                case AssignationStmt:
                    if (((AssignationStmt)stmt).Name != null) scope.SetTypes("", GetAssignation((AssignationStmt)stmt, scope));
                    else scope.SetTypes(((AssignationStmt)stmt).Name_!.Lexeme!, GetAssignation((AssignationStmt)stmt, scope));
                    break;
                case CircleStmt:
                    scope.SetTypes(((CircleStmt)stmt).Name.Lexeme!, new Circle(new Point(""), new Measure(new Point(""), new Point(""), ""), ((CircleStmt)stmt).Name.Lexeme!));
                    break;
                case FunctionStmt:
                    scope.SetTypes(((FunctionStmt)stmt).Name.Lexeme!, new Function(((FunctionStmt)stmt).Name, ((FunctionStmt)stmt).Arguments, ((FunctionStmt)stmt).Body));
                    break;
                case LineStmt:
                    scope.SetTypes(((LineStmt)stmt).Name.Lexeme!, new Line(new Point(""), new Point(""), ((LineStmt)stmt).Name.Lexeme!));
                    break;
                case PointStmt:
                    scope.SetTypes(((PointStmt)stmt).Name.Lexeme!, new Point(""));
                    break;
                case RayStmt:
                    scope.SetTypes(((RayStmt)stmt).Name.Lexeme!, new Ray(new Point(""), new Point(""), ((RayStmt)stmt).Name.Lexeme!));
                    break;
                case SegmentStmt:
                    scope.SetTypes(((SegmentStmt)stmt).Name.Lexeme!, new Segment(new Point(""), new Point(""), ((SegmentStmt)stmt).Name.Lexeme!));
                    break;
                default:
                    throw new TypeCheckerError(0, 0, "No es posible realizar esta instrucción");
            }
            return scope;
        }
        private Type GetAssignation(AssignationStmt stmt, Scope scope)
        {
            return stmt.Assigment switch
            {
                ArcExpression asig => asig.Check(scope),
                CircleExpression asig => asig.Check(scope),
                BetweenParenExpressions asig => asig.Check(scope),
                BinaryExpression asig => asig.Check(scope),
                IntersectExpression asig => asig.Check(scope),
                LetInExpression asig => asig.Check(scope),
                LineExpression asig => asig.Check(scope),
                MeasureExpression asig => asig.Check(scope),
                NumberExpression asig => asig.Check(scope),
                RayExpression asig => asig.Check(scope),
                SegmentExpression asig => asig.Check(scope),
                SequenceExpression asig => asig.Check(scope),
                StringExpression asig => asig.Check(scope),
                VariableExpression asig => asig.Check(scope),
                UnaryExpression asig => asig.Check(scope),
                UndefinedExpression asig => asig.Check(scope),
                _ => throw new TypeCheckerError(0, 0, "No es posible realizar esta instrucción"),
            };
        }
    }
}