using System;
using Verse;

namespace Desynchronized;

public struct NullableType<T>(T initialValue) : IExposable
    where T : struct
{
    private bool hasValue = true;

    public bool HasValue => hasValue;

    private T Value => !HasValue
        ? throw new InvalidOperationException("NullableType<T> : IExposable has no value.")
        : initialValue;

    public void ExposeData()
    {
        Scribe_Values.Look(ref hasValue, "hasValue");
        Scribe_Values.Look(ref initialValue, "insideValue");
    }

    public T GetValueOrDefault(T defaultValue = default)
    {
        return HasValue ? Value : defaultValue;
    }

    public override bool Equals(object obj)
    {
        if (!hasValue)
        {
            return obj == null;
        }

        return obj != null && base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return hasValue ? Value.GetHashCode() : 0;
    }

    public override string ToString()
    {
        return hasValue ? Value.ToString() : "";
    }

    public static implicit operator NullableType<T>(T from)
    {
        return new NullableType<T>(from);
    }

    public static implicit operator NullableType<T>(T? from)
    {
        return from.HasValue ? new NullableType<T>(from.Value) : new NullableType<T>();
    }

    public static implicit operator T?(NullableType<T> from)
    {
        return from.hasValue ? from.Value : null;
    }

    public static explicit operator T(NullableType<T> from)
    {
        return from.Value;
    }
}