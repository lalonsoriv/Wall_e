namespace Geo_Wall_E
{
    public enum Color
    {
        BLACK,
        BLUE,
        CYAN,
        GRAY,
        GREEN,
        MAGENTA,
        RED,
        WHITE,
        YELLOW,
    }

    public class ColorStack
    {
        public Stack<Color> colorStack = new();
        public ColorStack()
        {
            //Se inicializa con negro como color por defecto
            colorStack.Push(Color.BLACK);
        }
        public Color Pop()
        {
            return colorStack.Pop();
        }
        public Color Peek()
        {
            return colorStack.Peek();
        }
        public void Push(Color color)
        {
            colorStack.Push(color);
        }
    }
}