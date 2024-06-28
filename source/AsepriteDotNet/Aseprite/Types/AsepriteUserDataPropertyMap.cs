// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Collections;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines a property map within custom user data in an Aseprite file.
/// This class cannot be inherited.
/// </summary>
public sealed class AsepriteUserDataPropertyMap : IEnumerable<AsepriteUserDataProperty>
{
    private readonly AsepriteUserDataProperty[] _properties;

    /// <summary>
    /// ID of the property map.
    /// </summary>
    public uint ID { get; }

    /// <summary>
    /// Key/value pairs in the property map.
    /// </summary>
    public ReadOnlySpan<AsepriteUserDataProperty> Properties => _properties;

    /// <summary>
    /// Retrieves a property by name.
    /// </summary>
    public AsepriteUserDataProperty? this[string name] =>
        _properties.FirstOrDefault(prop => prop.Key == name);

    internal AsepriteUserDataPropertyMap(uint id, AsepriteUserDataProperty[] properties) =>
        (ID, _properties) = (id, properties);

    /// <inheritdoc />
    public IEnumerator<AsepriteUserDataProperty> GetEnumerator() =>
        _properties.AsEnumerable().GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() =>
        _properties.GetEnumerator();
}
