// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines a key/value pair within a property map in an Aseprite file.
/// This class cannot be inherited.
/// </summary>
public sealed class AsepriteUserDataProperty
{
    /// <summary>
    /// Name of the property.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Type of value stored in <see cref="Value"/>.
    /// </summary>
    public AsepriteUserDataPropertyType Type { get; }

    /// <summary>
    /// Value of the property.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// If the value is a property map, attempts to get the property with the specified key.
    /// </summary>
    public AsepriteUserDataProperty? this[string key] =>
        Value is AsepriteUserDataPropertyMap map
            ? map[key]
            : default;

    /// <summary>
    /// If the value is an array (vector), attempts to get the element at the specified index.
    /// </summary>
    public object? this[int index] =>
        index >= 0 && Value is object?[] vector && index < vector.Length
            ? vector[index]
            : default;

    internal AsepriteUserDataProperty(string key, AsepriteUserDataPropertyType type, object? value) =>
        (Key, Type, Value) = (key, type, value);
}
