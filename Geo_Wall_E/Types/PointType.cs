namespace Geo_Wall_E
{
    public class Point : Type, IDrawable
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Point;
        public string Name { get; private set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Point(string name)
        {
            Name = name;
            X = RandomCoordinateX();
            Y = RandomCoordinateY();
        }
        Type IDrawable.Type => new Point("");

        Color IDrawable.Color => Color.BLACK;
        string IDrawable.Phrase => Name;

        public static int RandomCoordinateX()
        {
            //Genera un número random entre 0 y 628
            Random random = new();
            int MaxPointCoordinate = 628;
            return random.Next(0,MaxPointCoordinate);
        }
        public static int RandomCoordinateY()
        {
            //Genera un número random entre 0 y 552
            Random random = new();
            int MaxPointCoordinate = 552;
            return random.Next(0,MaxPointCoordinate);
        }
    }
}