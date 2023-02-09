// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System.Runtime.CompilerServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace SixLabors;

internal static partial class Guard
{
    /// <summary>
    /// Ensures that the value is a value type.
    /// </summary>
    /// <param name="value">The target object, which cannot be null.</param>
    /// <param name="parameterName">The name of the parameter that is to be checked.</param>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <exception cref="ArgumentException"><paramref name="value"/> is not a value type.</exception>
    [MethodImpl(InliningOptions.ShortMethod)]
    public static void SamplerMustBeValueType<TValue>(TValue value, [CallerArgumentExpression("value")] string? parameterName = null)
        where TValue : IResampler
    {
        if (value.GetType().IsValueType)
        {
            return;
        }

        ThrowHelper.ThrowArgumentException("Type must be a struct.", parameterName!);
    }
}
