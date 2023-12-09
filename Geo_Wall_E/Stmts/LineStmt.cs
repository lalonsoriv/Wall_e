namespace Geo_Wall_E
{
    public class LineStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.LineToken;
        public Token Name { get; private set; }
        public bool Sequence { get; private set; }
        public LineStmt(Token name, bool sequence)
        {
            Name = name;
            Sequence = sequence;
        }

        public static Sequence LineSequence()
        {
            Random random = new ();
            List<Type> lines = new ();
            int count = random.Next(100, 1000);
            for (int i = 0; i < count; i++)
            {
                Point P1 = new("")
                {
                    X = random.Next(0, 1000),
                    Y = random.Next(0, 1000)
                };
                Point P2 = new("")
                {
                    X = random.Next(0, 1000),
                    Y = random.Next(0, 1000)
                };
                lines.Add(new Line(P1, P2,""));
            }
            return new Sequence(lines,"");
        }

    }
}