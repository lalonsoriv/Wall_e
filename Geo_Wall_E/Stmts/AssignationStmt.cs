using System.Linq.Expressions;

namespace Geo_Wall_E
{
    public class AssignationStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.AssignationStatement;
        public List<Token>? Name { get; private set; }
        public Token? Name_ { get; private set; }
        public Expressions Assigment { get; private set; }
        public AssignationStmt(List<Token> name, Expressions assigment)
        {
            Name = name;
            Assigment = assigment;
        }
        public AssignationStmt(Token name, Expressions assigment)
        {
            Name_ = name;
            Assigment = assigment;
        }
    }
}