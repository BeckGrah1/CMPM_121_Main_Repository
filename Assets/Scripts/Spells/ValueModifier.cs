using JetBrains.Annotations;
using System.Collections.Generic;

public class ValueModifier
{
    public enum type
    {
        MULTIPLY,
        DIVIDE,
        ADD,
        SUBTRACT
    }

    public type modifierType;
    public float value;
    public static float Apply(List<ValueModifier> modifiers, float baseValue)
    {
        float value = baseValue;
        foreach (var modifier in modifiers)
        {
            switch (modifier.modifierType)
            {
                case type.MULTIPLY:
                    value *= modifier.value;
                    break;
                case type.DIVIDE:
                    value /= modifier.value;
                    break;
                case type.ADD:
                    value += modifier.value;
                    break;
                case type.SUBTRACT:
                    value -= modifier.value;
                    break;
            }
        }
        return value;
    }
}
