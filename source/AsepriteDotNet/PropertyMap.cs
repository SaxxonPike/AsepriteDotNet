// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

public sealed class PropertyMap
{
    private PropertyMapValue[] _properties;

    public int Key { get; }

    public ReadOnlySpan<PropertyMapValue> Properties => _properties;

    public PropertyMapValue? this[string key] =>
        _properties.FirstOrDefault(p => p.Key == key);

    public PropertyMapValue? this[int index] =>
        _properties[index];

    internal PropertyMap(uint key, PropertyMapValue[] properties) =>
        (Key, _properties) = (unchecked((int)key), properties);
}
