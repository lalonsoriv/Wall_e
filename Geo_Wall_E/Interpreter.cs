using System.Runtime.InteropServices;

namespace Geo_Wall_E
{
    public class Interpreter
    {
        private Scope scope = new();
        private List<Node> Nodes = new();
        private Randoms random = new();
        private Samples samples = new();
        public Interpreter(List<Node> nodes)
        {
            Nodes = nodes;
        }

        public List<IDrawable> Evaluate()
        {
            List<IDrawable> toDraw = new();
            List<Type> list = new();
            try
            {
                foreach (var node in Nodes)
                {
                    if (node is Stmt stmt)
                    {
                        EvaluateStmt(stmt, scope, toDraw);
                    }
                    else if (node is Expressions expressions)
                    {
                        list.Add(TypeCheck(scope, expressions));
                    }
                    else list.Add(new EmptyType());

                }
            }
            catch (SemanticError error)
            {
                error.HandleException();
                return null!;
            }
            return toDraw;

        }

        private void EvaluateStmt(Stmt stmt, Scope scope, List<IDrawable> todraw)
        {
            switch (stmt)
            {
                case AssignationStmt:
                    EvaluateAssignation((AssignationStmt)stmt);
                    break;
                case ArcStmt:
                    if (((ArcStmt)stmt).Sequence) scope.SetTypes(((ArcStmt)stmt).Name.Lexeme!, ArcStmt.ArcSequence());
                    else scope.SetTypes(((ArcStmt)stmt).Name.Lexeme!, new Arc(new Point(""), new Point (""), new Point(""), new Measure(new Point(""), new Point(""), ""), ((ArcStmt)stmt).Name.Lexeme!));
                    break;
                case CircleStmt:
                    if (((CircleStmt)stmt).Sequence) scope.SetTypes(((CircleStmt)stmt).Name.Lexeme!, CircleStmt.CircleSequence());
                    else scope.SetTypes(((CircleStmt)stmt).Name.Lexeme!, new Circle(new Point(""), new Measure(new Point(""), new Point(""), ""), ((CircleStmt)stmt).Name.Lexeme!));
                    break;
                case DrawStmt:
                    EvaluateDraw(todraw, (DrawStmt)stmt);
                    break;
                case FunctionStmt:
                    scope.SetTypes(((FunctionStmt)stmt).Name.Lexeme!, new Function(((FunctionStmt)stmt).Name, ((FunctionStmt)stmt).Arguments, ((FunctionStmt)stmt).Body));
                    break;
                case LineStmt:
                    if (((LineStmt)stmt).Sequence) scope.SetTypes(((LineStmt)stmt).Name.Lexeme!, LineStmt.LineSequence());
                    else scope.SetTypes(((LineStmt)stmt).Name.Lexeme!, new Line(new Point(""), new Point(""), ((LineStmt)stmt).Name.Lexeme!));
                    break;
                case PointStmt:
                    if (((PointStmt)stmt).Sequence) scope.SetTypes(((PointStmt)stmt).Name.Lexeme!, PointStmt.PointSequence());
                    else scope.SetTypes(((PointStmt)stmt).Name.Lexeme!, new Point(""));
                    break;
                case RayStmt:
                    if (((RayStmt)stmt).Sequence) scope.SetTypes(((RayStmt)stmt).Name.Lexeme!, RayStmt.RaySequence());
                    else scope.SetTypes(((RayStmt)stmt).Name.Lexeme!, new Ray(new Point(""), new Point(""), ((RayStmt)stmt).Name.Lexeme!));
                    break;
                case SegmentStmt:
                    if (((SegmentStmt)stmt).Sequence) scope.SetTypes(((SegmentStmt)stmt).Name.Lexeme!, SegmentStmt.SegmentSequence());
                    else scope.SetTypes(((SegmentStmt)stmt).Name.Lexeme!, new Segment(new Point(""), new Point(""), ((SegmentStmt)stmt).Name.Lexeme!));
                    break;
            }
        }

        private void EvaluateDraw(List<IDrawable> toDraw, DrawStmt stmt)
        {
            if (stmt.ExpressionSequence != null)
            {
                foreach (var variable in stmt.ExpressionSequence)
                {
                    IDrawable fig = new Drawable(scope.GetTypes(variable.Name.Lexeme!), stmt.Color, stmt.Name);
                    toDraw.Add(fig);
                }
            }
            else if (stmt.Expression is Expressions)
            {
                switch (stmt.Expression)
                {
                    case ArcExpression:
                    case CircleExpression:
                    case FunctionCallExpression:
                    case LetInExpression:
                    case LineExpression:
                    case RayExpression:
                    case SegmentExpression:
                        toDraw.Add(new Drawable(((ICheckType)stmt.Expression).Check(scope), stmt.Color, stmt.Name));
                        break;
                    case VariableExpression:
                        var type = scope.GetTypes(((VariableExpression)stmt.Expression).Name.Lexeme!);
                        if (type is Sequence sequence)
                        {
                            foreach (var element in sequence.Elements)
                            {
                                toDraw.Add(new Drawable(element, stmt.Color, stmt.Name));
                            }
                        }
                        toDraw.Add(new Drawable(type, stmt.Color, stmt.Name));
                        break;
                }
            }
            else
            {
                switch (stmt.Expression)
                {
                    case ArcStmt:
                    case CircleStmt:
                    case FunctionStmt:
                    case LineStmt:
                    case PointStmt:
                    case RayStmt:
                    case SegmentStmt:
                        EvaluateStmt(stmt, scope, toDraw);
                        break;
                    default:
                        throw new SemanticError(0, 0, "No es posible evaluar esta expresión");
                }

            }
        }

        private void EvaluateAssignation(AssignationStmt asig)
        {
            if (asig.Name != null)
            {
                if (asig.Assigment is SequenceExpression || asig.Assigment is IntersectExpression)
                {
                    Sequence sequence = (Sequence)((ICheckType)asig.Assigment).Check(scope);
                    for (int i = 0; i < asig.Name.Count; i++)
                    {
                        if (asig.Name[i].Type == TypesOfToken.UnderscoreToken) continue;
                        if (asig.Name.Count > sequence.Elements.Count) scope.SetTypes(asig.Name[i].Lexeme!, new EmptyType());
                        if (asig.Name[i].Type == TypesOfToken.RestToken)
                        {
                            Sequence rest = new([], "");
                            for (int j = i; j < sequence.Elements.Count; j++)
                            {
                                rest.Elements.Add(sequence.Elements[j]);
                            }
                            scope.SetTypes(asig.Name[i].Lexeme!, rest);
                            break;
                        }
                        else scope.SetTypes(asig.Name[i].Lexeme!, sequence.Elements[i]);
                    }
                }
                if (asig.Assigment is UndefinedExpression)
                {
                    for (int i = 0; i < asig.Name.Count; i++)
                    {
                        scope.SetTypes(asig.Name[i].Lexeme!, new Undefined());
                    }
                }
                if (asig.Assigment is Randoms)
                {
                    for (int i = 0; i < asig.Name.Count; i++)
                    {

                        if (asig.Name[i].Type == TypesOfToken.UnderscoreToken || asig.Name[i].Type == TypesOfToken.RestToken) continue;
                        if (asig.Name.Count > random.RandomSequence.Elements.Count) scope.SetTypes(asig.Name[i].Lexeme!, new EmptyType());
                        scope.SetTypes(asig.Name[i].Lexeme!, random.RandomSequence.Elements[i]);
                    }
                }
                if (asig.Assigment is Samples)
                {
                    for (int i = 0; i < asig.Name.Count; i++)
                    {

                        if (asig.Name[i].Type == TypesOfToken.UnderscoreToken || asig.Name[i].Type == TypesOfToken.RestToken) continue;
                        if (asig.Name.Count > samples.Sequence.Elements.Count) scope.SetTypes(asig.Name[i].Lexeme!, new EmptyType());
                        scope.SetTypes(asig.Name[i].Lexeme!, samples.Sequence.Elements[i]);
                    }
                }
                if (asig.Assigment is RandomPoints)
                {
                    Sequence sequence = (Sequence)((ICheckType)asig.Assigment).Check(scope);
                    for (int i = 0; i < asig.Name.Count; i++)
                    {
                        if (asig.Name[i].Type == TypesOfToken.UnderscoreToken || asig.Name[i].Type == TypesOfToken.RestToken) continue;
                        if (asig.Name.Count > sequence.Elements.Count) scope.SetTypes(asig.Name[i].Lexeme!, new EmptyType());
                        else scope.SetTypes(asig.Name[i].Lexeme!, sequence.Elements[i]);
                    }
                }
            }
            else switch (asig.Assigment)
                {
                    case ArcExpression:
                    case CircleExpression:
                    case IntersectExpression:
                    case LetInExpression:
                    case LineExpression:
                    case MeasureExpression:
                    case NumberExpression:
                    case RayExpression:
                    case SegmentExpression:
                    case SequenceExpression:
                    case VariableExpression:
                        scope.SetTypes(asig.Name_!.Lexeme!, ((ICheckType)asig.Assigment).Check(scope));
                        break;
                    default:
                        throw new SemanticError(0, 0, "No es posible evaluar esta expresión");
                }
        }

        public Type TypeCheck(Scope scope, Expressions expressions)
        {
            return expressions switch
            {
                ArcExpression exp => exp.Check(scope),
                BetweenParenExpressions exp => exp.Check(scope),
                BinaryExpression exp => exp.Check(scope),
                CircleExpression exp => exp.Check(scope),
                ConcatenatedSequenceExpression exp => exp.Check(scope),
                ConditionalExpression exp => exp.Check(scope),
                Count exp => exp.Check(scope),
                EExpression exp => exp.Check(scope),
                FunctionCallExpression exp => exp.Check(scope),
                InfiniteSequenceExpression exp => exp.Check(scope),
                IntersectExpression exp => exp.Check(scope),
                LetInExpression exp => exp.Check(scope),
                LineExpression exp => exp.Check(scope),
                LogExpression exp => exp.Check(scope),
                MeasureExpression exp => exp.Check(scope),
                NumberExpression exp => exp.Check(scope),
                PIExpression exp => exp.Check(scope),
                RandomPoints exp => exp.Check(scope),
                RayExpression exp => exp.Check(scope),
                SegmentExpression exp => exp.Check(scope),
                SequenceExpression exp => exp.Check(scope),
                StringExpression exp => exp.Check(scope),
                UnaryExpression exp => exp.Check(scope),
                UndefinedExpression exp => exp.Check(scope),
                VariableExpression exp => exp.Check(scope),
                _ => throw new TypeCheckerError(0, 0, "No es posible realizar esta instrucción"),
            };
        }
    }
}
