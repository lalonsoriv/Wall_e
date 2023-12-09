namespace Geo_Wall_E
{
    public class RayStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.RayToken;
        public Token Name { get; private set; }
        public bool Sequence { get; private set; }
        public RayStmt(Token name, bool sequence)
        {
            Name = name;
            Sequence = sequence;
        }
        public static Sequence RaySequence()
        {
            Random random = new ();
            List<Type> ray = new ();
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
                ray.Add(new Ray(P1, P2,""));
            }
            return new Sequence(ray,"");
        }
    }
}