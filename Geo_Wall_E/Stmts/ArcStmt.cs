namespace Geo_Wall_E
{
    public class ArcStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.CircleToken;
        public Token Name { get; private set; }
        public bool Sequence { get; private set; }
        public ArcStmt(Token name, bool sequence)
        {
            Name = name;
            Sequence = sequence;
        }
        public static Sequence ArcSequence()
        {
            Random random = new();
            List<Type> arcs = new();
            int count = random.Next(100, 1000);
            for (int i = 0; i < count; i++)
            {
                Point p = new("")
                {
                    X = random.Next(0, 1000),
                    Y = random.Next(0, 1000)
                };
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
                Point Pm1 = new("")
                {
                    X = random.Next(0, 1000),
                    Y = random.Next(0, 1000)
                };
                Point Pm2 = new("")
                {
                    X = random.Next(0, 1000),
                    Y = random.Next(0, 1000)
                };
                Measure m = new(Pm1, Pm2, "");
                Arc c = new(p,P1,P2, m, "");
                arcs.Add(c);
            }
            return new Sequence(arcs, "");
        }
    }
}