// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Collections;
using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines a property map within custom user data in an Aseprite file.
/// This class cannot be inherited.
/// </summary>
public sealed class AsepriteUserDataPropertyMap :
    IEquatable<AsepriteUserDataPropertyMap>,
    IEnumerable<AsepriteUserDataProperty>
{
    private readonly AsepriteUserDataProperty[] _properties;

    /// <summary>
    /// ID of the property map.
    /// </summary>
    public uint Id { get; }

    /// <summary>
    /// Key/value pairs in the property map.
    /// </summary>
    public ReadOnlySpan<AsepriteUserDataProperty> Properties => _properties;

    internal AsepriteUserDataPropertyMap(uint id, AsepriteUserDataProperty[] properties) =>
        (Id, _properties) = (id, properties);

    /// <inheritdoc />
    public bool Equals(AsepriteUserDataPropertyMap? other)
    {
        return other is not null &&
               (ReferenceEquals(this, other) ||
                _properties.SequenceEqual(other._properties) &&
                Id == other.Id);
    }

    /// <inheritdoc />
    public IEnumerator<AsepriteUserDataProperty> GetEnumerator() =>
        _properties.AsEnumerable().GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) ||
               obj is AsepriteUserDataPropertyMap other &&
               Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(_properties, Id);
    }
}
