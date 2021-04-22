using System.Collections.Generic;

public enum PrimitiveType
{
    CharType,
    StringType,
    Int32Type,
    DoubleType,
    FloatType,
    BoolType,
    GameObjectType,
    TransformType,
    MaterialType,
    ColorType,
    CustomType,
}

public static class TypeString
{
    public static readonly Dictionary<PrimitiveType, string> TypesDictionary = new Dictionary<PrimitiveType, string>
    {
        {PrimitiveType.StringType, "string"},
        {PrimitiveType.CharType, "char"},
        {PrimitiveType.Int32Type, "int"},
        {PrimitiveType.DoubleType, "double"},
        {PrimitiveType.FloatType, "float"},
        {PrimitiveType.BoolType, "bool"},
        {PrimitiveType.GameObjectType, "GameObject"},
        {PrimitiveType.TransformType, "Transform"},
        {PrimitiveType.MaterialType, "Material"},
        {PrimitiveType.ColorType, "Color"},
    };
}