namespace Geo_Wall_E
{
    public class FunctionCallExpression : Expressions, ICheckType

    {
        public override TypesOfToken Type => TypesOfToken.FunctionCallExpression;
        public Token Name { get; private set; }
        public List<Expressions> Arguments { get; private set; }
        public Node Body { get; private set; }

        public FunctionCallExpression(Token name, List<Expressions> args, Node body)
        {
            Name = name;
            Arguments = args;
            Body = body;
        }
        int maxCantCall = 100;
        int cantCall = 0;
        public Type Check(Scope scope)
        {
            if (cantCall < maxCantCall)
            {
                Type function = scope.GetTypes(Name.Lexeme!);
                if (function.TypeOfElement == TypeOfElement.Function)
                {
                    if (Arguments.Count == ((Function)function).Arguments.Count)
                    {
                        Dictionary<string, Type> arguments = new();
                        for (int i = 0; i < Arguments.Count; i++)
                        {
                            VariableExpression variable = (VariableExpression)((Function)function).Arguments[i];
                            try
                            {
                                arguments.Add(variable.Name.Lexeme!, ((ICheckType)Arguments[i]).Check(scope));
                            }
                            catch (TypeCheckerError)
                            {
                                throw new TypeCheckerError(0, 0, "No se puede evaluar el argumento " + ((Function)function).Arguments[i]!.ToString()!);
                            }
                        }
                        scope.SetScope();
                        cantCall++;
                        foreach (var arg in arguments)
                        {
                            scope.SetTypes(arg.Key, arg.Value);
                        }
                        Type result = ((ICheckType)((Function)function).Body).Check(scope);
                        scope.DeleteScope();
                        cantCall--;
                        return result;
                    }
                    throw new TypeCheckerError(0, 0, "La función " + ((Function)function).Name + " esperaba " + ((Function)function).Arguments.Count + " elementos");
                }
                throw new TypeCheckerError(0, 0, "La función " + ((Function)function).Name + " no ha sido definida");
            }
            throw new SemanticError(0, 0, "Stack Overflow");
        }
    }
}
