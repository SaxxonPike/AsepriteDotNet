// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Collections;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines a property map within custom user data in an Aseprite file.
/// This class cannot be inherited.
/// </summary>
public sealed class AsepriteUserDataPropertyMap
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

    internal AsepriteUserDataPropertyMap(uint id, AsepriteUserDataProperty[] properties) =>
        (ID, _properties) = (id, properties);
}
