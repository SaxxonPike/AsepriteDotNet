// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines a key/value pair within a property map in an Aseprite file.
/// This class cannot be inherited.
/// </summary>
public sealed class AsepriteUserDataProperty : IEquatable<AsepriteUserDataProperty>
{
    /// <summary>
    /// Name of the property.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Value of the property.
    /// </summary>
    public object? Value { get; }

    internal AsepriteUserDataProperty(string key, object? value) =>
        (Key, Value) = (key, value);

    /// <inheritdoc />
    public bool Equals(AsepriteUserDataProperty? other)
    {
        return other is not null &&
               (ReferenceEquals(this, other) ||
                Key == other.Key &&
                Equals(Value, other.Value));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) ||
               obj is AsepriteUserDataProperty other &&
               Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Value);
    }
}
