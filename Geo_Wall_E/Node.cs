namespace Geo_Wall_E
{
    public abstract class Node
    {
        public abstract TypesOfToken Type { get; }
    }
    //Los Stmt son las declaraciones de distintas que modifican el progrma
    public abstract class Stmt : Node
    {
        public abstract override TypesOfToken Type { get; }
    }
    //Las expressiones representan valores o elemntos
    public abstract class Expressions : Node
    {
        public abstract override TypesOfToken Type { get; }
    }
    //Secuncia vacías
    public class Empty : Node
    {
        public override TypesOfToken Type => TypesOfToken.EmptyToken;
    }
}