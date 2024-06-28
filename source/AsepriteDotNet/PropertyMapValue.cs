// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

public sealed class PropertyMapValue
{
    private PropertyMapValue[]? _children;

    public string Key { get; }

    public object? Value { get; }

    public bool HasChildren =>
        _children is { Length: > 0 };

    public ReadOnlySpan<PropertyMapValue> Children =>
        _children ?? ReadOnlySpan<PropertyMapValue>.Empty;

    public PropertyMapValue? this[string key] =>
        _children?.FirstOrDefault(p => p.Key == key);

    public PropertyMapValue this[int index] =>
        (_children ?? Array.Empty<PropertyMapValue>()) [index];

    internal PropertyMapValue(PropertyMapValue[]? children, string key, object? value) =>
        (_children, Key, Value) = (children, key, value);
}
