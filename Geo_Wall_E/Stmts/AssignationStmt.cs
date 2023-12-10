using System.Linq.Expressions;

namespace Geo_Wall_E
{
    public class AssignationStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.AssignationStatement;
        public List<Token>? Name { get; private set; }
        public Token? Name_ { get; private set; }
        public Expressions Assignment { get; private set; }
        public AssignationStmt(List<Token> name, Expressions assignment)
        {
            Name = name;
            Assignment = assignment;
        }
        public AssignationStmt(Token name, Expressions assignment)
        {
            Name_ = name;
            Assignment = assignment;
        }
    }
}
