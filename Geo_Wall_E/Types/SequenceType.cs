namespace Geo_Wall_E
{
    public class Sequence : Type
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Sequence;
        public List<Type> Elements { get; private set; }
        public string Name { get; private set; }

        public Sequence(List<Type> elements, string name)
        {
            Elements = elements;
            Name = name;
        }
    }
}