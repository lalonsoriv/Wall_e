namespace Geo_Wall_E
{
    public class CircleStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.CircleToken;
        public Token Name { get; private set; }
        public bool Sequence { get; private set; }
        public CircleStmt(Token name, bool sequence)
        {
            Name = name;
            Sequence = sequence;
        }
        public static Sequence CircleSequence()
        {
            Random random = new();
            List<Type> circles = new();
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
                Measure m = new(P1, P2, "");
                Circle c = new(p, m, "");
                circles.Add(c);
            }
            return new Sequence(circles, "");
        }
    }
}