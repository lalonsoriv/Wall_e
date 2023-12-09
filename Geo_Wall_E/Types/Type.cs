using System.Reflection.Metadata.Ecma335;

namespace Geo_Wall_E
{
    public enum TypeOfElement
    {
        Arc,
        Boolean,
        Circle,
        Constant,
        Empty,
        Function,
        Line,
        Measure,
        Number,
        Point,
        Ray,
        Segment,
        String,
        Sequence,
        Undefined,
    }

    public abstract class Type
    {
        public abstract TypeOfElement TypeOfElement { get; }
    }
}