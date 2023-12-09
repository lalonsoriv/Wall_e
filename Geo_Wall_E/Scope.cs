namespace Geo_Wall_E
{
    public class Scope
    {
        public Dictionary<string, Dictionary<int, FunctionStmt>> variablesInFunction = new();
        private Stack<Dictionary<string, Type>> scopes = new();
        public Scope()
        {
            scopes.Push([]);
            variablesInFunction = new();
        }
        public void SetScope()
        {
            scopes.Push([]);
        }
        public Type GetTypes(string id) 
        {
            foreach (var scope in scopes)
            {
                if (scope.TryGetValue(id, out Type? value))
                {
                    return value;
                }
            }
            throw new SemanticError(0, 0, "La variable " + id + " no ha sido declarada");
        }
        public void DeleteScope()
        {
            scopes.Pop();
        }
        public void SetTypes(string id, Type value)
        {
            scopes.Peek()[id] = value;
        }
        public Dictionary<string, Type> GetDictionary()
        {
            return scopes.Peek();
        }
        public void SetDictionary(Dictionary<string, Type> dict)
        {
            scopes.Push(dict);
        }
        //Busca la función y inicializa sus parámetros
        public void Search(FunctionStmt function)
        {
            Token functionName = function.Name;
            int variables = 0;
            //Si no posee argumentos la cantidad de variables será -1
            if (function.Arguments == null!) variables = -1;
            else
            {
                //De lo contrario se inicializa la cantidad de variables
                variables = function.Arguments.Count();
            }

            //Si ya existe esta función 
            if (variablesInFunction.ContainsKey(functionName.Lexeme!))
            {
                Dictionary<int, FunctionStmt> cantvariables = variablesInFunction[functionName.Lexeme!];
                //Si continene la misma cantidad de variables se entenderá como un error porque se estará tratando de sobreescribir la función
                if (cantvariables.ContainsKey(variables)) throw new SyntaxError(function.Name.Line, function.Name.Column, "No puede emplear la palabra " + "\"" + functionName + "\"" + " para declarar una función debido a que ya existe una función con este nombre y la misma cantidad de argumentos");
                //La cantidad de variables solo será -1 cuando la función posea argumentos vacíos por lo que eliminará los valores del diccionario
                if (variablesInFunction[functionName.Lexeme!].ContainsKey(-1)) variablesInFunction[functionName.Lexeme!].Remove(-1);
                //Se le agregan los argumentos y el cuerpo
                variablesInFunction[functionName.Lexeme!].Add(variables, function);
            }
            //De no existir se crea y se le añaden los valores
            else
            {
                Dictionary<int, FunctionStmt> cantvariables = new()
                {
                    { variables, function }
                };
                variablesInFunction.Add(functionName.Lexeme!, cantvariables);
            }
        }
    }
}
