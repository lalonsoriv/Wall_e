//Representa los elementos que se pueden dibujar en la pantalla
namespace Geo_Wall_E
{
    public interface IDrawable
    {
        public Type Type { get; }
        public Color Color { get; }
        public string Phrase { get; }
    }
    public class Drawable : IDrawable
    {
        private Type type;
        private Color color;
        private string name;
        public Drawable(Type type, Color color, string name)
        {
            this.type = type;
            this.color = color;
            this.name = name;
        }

        public Type Type => type;

        public Color Color => color;

        public string Phrase => name;
    } 
}