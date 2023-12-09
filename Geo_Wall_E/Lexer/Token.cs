namespace Geo_Wall_E
{
    public enum TypesOfToken
    {
        PointToken,
        LineToken,
        SegmentToken,
        RayToken,
        CircleToken,
        SequenceToken,
        ImportToken,
        DrawToken,
        ArcToken,
        MeasureToken,
        IntersectToken,
        CountToken,
        RandomsToken,
        PointsToken,
        SamplesToken,
        LetToken,
        InToken,
        IfToken,
        ThenToken,
        ElseToken,
        RestToken,
        EmptyToken,
        ThreeDotsToken,             //'...'
        PlusToken,                  //'+'
        MinusToken,                 //'-'
        MultToken,                  //'*'
        DivToken,                   //'/'
        EqualToken,                 //'='
        NoEqualToken,               //'!='
        DoubleEqualToken,           //'=='
        PowToken,                   //'^'
        ExpoToken,                  //'exp'
        LogToken,                   //'log'
        SqrtToken,                  //'sqrt'
        PIToken,                    //'PI'
        EToken,                     //'E'
        SinToken,                   //'sin'
        CosToken,                   //'cos'
        ModuToken,                  //'%'
        NotToken,                   //'!'
        AndToken,                   //'&'
        OrToken,                    //'|'
        LessToken,                  //'<'
        MoreToken,                  //'>'
        LessOrEqualToken,           //'<='
        MoreOrEqualToken,           //'>='
        OpenParenthesisToken,       //'('
        CloseParenthesisToken,      //')'
        OpenBracketsToken,          //'{'
        CloseBracketsToken,         //'}'
        EndFileToken,               //''        
        SemicolonToken,             //';'
        SeparatorToken,             //','
        UndefinedToken,
        VariableToken,
        UnderscoreToken,
        RestoreToken,
        Number,
        String,
        ID,
        BinaryExpression,
        UnaryExpression,        
        BetweenParenExpression,
        LetInExpression,
        ConditionalExpression,
        FunctionCallExpression, 
        FunctionDeclaration,
        AssignationStatement,
        ColorToken,
        ColorBlackToken,
        ColorBlueToken,
        ColorCyanToken,
        ColorGrayToken,
        ColorGreenToken,
        ColorRedToken,
        ColorMagentaToken,
        ColorWhiteToken,
        ColorYellowToken,
    }

    public class Token
    {
        public TypesOfToken Type { get; }
        public int Line { get; }
        public int Column { get; }
        public object? Value { get; }
        public string? Lexeme { get; }
        public Token(TypesOfToken type, object? value, int line, int column, string lexeme)
        {
            Type = type;
            Value = value;
            Line = line;
            Column = column;
            Lexeme = lexeme;
        }
    }
}