// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Collections;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines a key/value pair within a property map in an Aseprite file.
/// This class cannot be inherited.
/// </summary>
public sealed class AsepritePropertyMapEntry
{
    /// <summary>
    /// Name of the property.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Original data type loaded from the file.
    /// </summary>
    public AsepritePropertyType Type { get; }

    /// <summary>
    /// Value of the property.
    /// </summary>
    public object? Value { get; }

    internal AsepritePropertyMapEntry(string key, AsepritePropertyType type, object? value) =>
        (Key, Type, Value) = (key, type, value);
}
