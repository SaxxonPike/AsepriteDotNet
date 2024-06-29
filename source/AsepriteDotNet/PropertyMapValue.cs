// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a single key/value pair in a property map.
/// This class cannot be inherited.
/// </summary>
public sealed class PropertyMapValue
{
    private readonly PropertyMapValue[]? _children;

    /// <summary>
    /// Key of the key/value pair.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Value of the key/value pair.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Returns true if there are nested values - that is, there are
    /// other values contained within. Otherwise, returns false. Nested
    /// values can be either arrays or nested property maps.
    /// </summary>
    public bool HasChildren =>
        _children is { Length: > 0 };

    /// <summary>
    /// Returns the child key/value pairs that belong to this key/value pair.
    /// If the value cannot store other values inside, an empty collection is
    /// returned (and thus, this will not throw.)
    /// </summary>
    public ReadOnlySpan<PropertyMapValue> Children =>
        _children ?? ReadOnlySpan<PropertyMapValue>.Empty;

    /// <summary>
    /// Retrieves a nested value by key.
    /// </summary>
    /// <param name="key">
    /// Key to match.
    /// </param>
    /// <returns>
    /// The first child key/value pair that corresponds to the specified key.
    /// If the key was not found, or no nested values are present, this will
    /// return null.
    /// </returns>
    public PropertyMapValue? this[string key] =>
        _children?.FirstOrDefault(p => p.Key == key);

    /// <summary>
    /// Retrieves a nested value by index.
    /// </summary>
    /// <param name="index">
    /// Index within the nested values to retrieve.
    /// </param>
    /// <returns>
    /// The key/value pair that corresponds to the specified index.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the specified index is out of range of nested values.
    /// </exception>
    public PropertyMapValue this[int index] =>
        (_children ?? Array.Empty<PropertyMapValue>()) [index];

    internal PropertyMapValue(PropertyMapValue[]? children, string key, object? value) =>
        (_children, Key, Value) = (children, key, value);
}
