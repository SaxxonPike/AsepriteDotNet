// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a set of key/value pairs with arbitrary value types.
/// This class cannot be inherited.
/// </summary>
public sealed class PropertyMap
{
    private readonly PropertyMapValue[] _properties;

    /// <summary>
    /// Unique key associated with the property map.
    /// This will be zero for generic user data, but Aseprite extensions can
    /// specify their own IDs to keep their data separate.
    /// </summary>
    public int Key { get; }

    /// <summary>
    /// All properties stored in the root level of the property map.
    /// </summary>
    public ReadOnlySpan<PropertyMapValue> Properties => _properties;

    /// <summary>
    /// Retrieve a property by key.
    /// </summary>
    /// <param name="key">
    /// Property key to match.
    /// </param>
    /// <returns>
    /// If the key was found, the corresponding key/value pair found first will
    /// be returned. Otherwise, returns null.
    /// </returns>
    public PropertyMapValue? this[string key] =>
        _properties.FirstOrDefault(p => p.Key == key);

    /// <summary>
    /// Retrieve a property by index.
    /// </summary>
    /// <param name="index">
    /// Index to retrieve.
    /// </param>
    /// <returns>
    /// The key/value pair at the specified index.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the given index is out of range.
    /// </exception>
    public PropertyMapValue this[int index] =>
        _properties[index];

    internal PropertyMap(uint key, PropertyMapValue[] properties) =>
        (Key, _properties) = (unchecked((int)key), properties);
}
