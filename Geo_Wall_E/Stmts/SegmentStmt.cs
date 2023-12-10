namespace Geo_Wall_E
{
    public class SegmentStmt : Stmt
    {
        public override TypesOfToken Type => TypesOfToken.SegmentToken;
        public Token Name { get; private set; }
        public bool Sequence { get; private set; }
        public SegmentStmt(Token name, bool sequence)
        {
            Name = name;
            Sequence = sequence;
        }
        public static Sequence SegmentSequence()
        {
            Random random = new ();
            List<Type> segments = new ();
            int count = random.Next(100, 999);
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
                segments.Add(new Segment(P1, P2,""));
            }
            return new Sequence(segments,"");
        }
    }
}
