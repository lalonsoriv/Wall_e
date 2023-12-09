namespace Geo_Wall_E
{
    public class PointStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.PointToken;
        public Token Name { get; private set; }
        public bool Sequence { get; private set; }
        public PointStmt(Token name, bool sequence)
        {
            Name = name;
            Sequence = sequence;
        }

        public static Sequence PointSequence()
        {
            Random random = new();
            List<Type> points = new();
            int count = random.Next(100, 1000);
            for (int i = 0; i < count; i++)
            {
                Point p = new("")
                {
                    X = random.Next(0, 1000),
                    Y = random.Next(0, 1000)
                };
                points.Add(p);
            }
            return new Sequence(points, "");
        }
    }
}