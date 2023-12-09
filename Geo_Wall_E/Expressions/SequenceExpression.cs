namespace Geo_Wall_E
{
    public class SequenceExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.SequenceToken;
        public List<Expressions>? Elements { get; private set; }
        public Token? Start { get; private set; }
        public Token? End { get; private set; }
        public ConcatenatedSequenceExpression? Sequence { get; private set; }
        public InfiniteSequenceExpression? InfiniteSequence { get; private set; }

        public SequenceExpression(List<Expressions> elements)
        {
            Elements = elements;
        }
        public SequenceExpression(Token start, Token end)
        {
            Start = start;
            End = end;
        }
        public SequenceExpression(Token strat)
        {
            Start = strat;
        }
        public SequenceExpression(ConcatenatedSequenceExpression sequence)
        {
            Sequence = sequence;
        }

        public Type Check(Scope scope)
        {
            List<Type> elements = new();
            if (Elements != null)
            {
                foreach (var element in Elements)
                {
                    elements.Add(((ICheckType)element).Check(scope));
                }
                if (elements.All(x => x.TypeOfElement == elements[0].TypeOfElement)) return new Sequence(elements, "");
                else throw new TypeCheckerError(0, 0, "Los elementos de la secuencia no son el mismo tipo");
            }
            if (Start != null && End != null)
            {
                //si la intersección es muy grande se considerará que es infinita
                if ((int)Start.Value! - (int)End.Value! > 100) return (Sequence)new InfiniteSequenceExpression(Start).Check(scope);
                List<Type> sequence = (List<Type>)Enumerable.Range((int)Start.Value!, (int)End.Value!).Select(x => new Number(x));
                return new Sequence(sequence, "");
            }
            if (Start != null && End == null)
            {
                ///deberia devolver una secuencia infita
                return (Sequence)new InfiniteSequenceExpression(Start).Check(scope);
            }
            if (Sequence != null)
            {
                ///deberia devolver una secuencia concatenada
                return (Sequence)((ICheckType)Sequence).Check(scope);
            }
            else throw new TypeCheckerError(0, 0, "La secuencia no es válida");
        }
    }

    public class ConcatenatedSequenceExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.SequenceToken;
        public SequenceExpression? FirstSquence { get; private set; }
        public UndefinedExpression? Undefined { get; private set; }
        public SequenceExpression? SecondSquence { get; private set; }

        public ConcatenatedSequenceExpression(SequenceExpression first, SequenceExpression second)
        {
            FirstSquence = first;
            SecondSquence = second;
        }

        public ConcatenatedSequenceExpression(SequenceExpression first, UndefinedExpression undefined)
        {
            FirstSquence = first;
            Undefined = undefined;
        }

        public ConcatenatedSequenceExpression(UndefinedExpression undefined, SequenceExpression second)
        {
            SecondSquence = second;
            Undefined = undefined;
        }

        public Type Check(Scope scope)
        {
            if (FirstSquence != null && SecondSquence != null)
            {
                Sequence first = (Sequence)((ICheckType)FirstSquence!).Check(scope);
                Sequence second = (Sequence)((ICheckType)SecondSquence!).Check(scope);
                List<Type> sequence = new(first.Elements.Concat(second.Elements));
                if (sequence.All(x => x.TypeOfElement == sequence[0].TypeOfElement)) return new Sequence(sequence, "");
                else throw new TypeCheckerError(0, 0, "Para poder concatenar las secuencias deben ser del mismo tipo");
            }
            if (FirstSquence != null && Undefined != null)
            {
                Sequence first = (Sequence)((ICheckType)FirstSquence!).Check(scope);
                Undefined undefined = (Undefined)((ICheckType)Undefined!).Check(scope);
                first.Elements.Add(undefined);
                return new Sequence(first.Elements, "");
            }
            if (SecondSquence != null && Undefined != null)
            {
                return new Undefined();
            }
            else throw new TypeCheckerError(0, 0, "La secuencia no es válida");
        }
    }

    public class InfiniteSequenceExpression : Expressions, ICheckType
    {
        public override TypesOfToken Type => TypesOfToken.SequenceToken;
        public Token Start { get; private set; }
        public InfiniteSequenceExpression(Token start)
        {
            Start = start;
        }

        public Type Check(Scope scope)
        {
            List<Type> sequence = (List<Type>)Enumerable.Range((int)Start.Value!, (int)Start.Value! + 100).Select(x => new Number(x));
            return new Sequence(sequence,"");
        }
    }
}