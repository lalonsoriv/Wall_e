namespace Geo_Wall_E
{
    public class Number : Type
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Number;

        public double Value { get; private set; }

        public Number(double value)
        {
            Value = value;
        }

    }

    public class String : Type
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.String;
        public  string Value { get; private set; }

        public String(string value)
        {
            Value = value;
        }
    }
    public class Boolean : Type
    {
        public override TypeOfElement TypeOfElement => TypeOfElement.Boolean;
        public int Value { get; private set;}
        public Boolean(bool value)
        {
            if (value) Value = 1;
            else Value = 0;
        }
    }
}