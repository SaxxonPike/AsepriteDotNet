// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing user data property maps from an <see cref="AsepriteFile"/>.
/// </summary>
public static class PropertyMapProcessor
{
    internal static PropertyMapValue? Process(string key, object? input)
    {
        if (input == null)
            return null;

        PropertyMapValue[]? children = null;
        object? value;

        switch (input)
        {
            // Some integer conversions are performed, aligned to
            // the integer implicit cast rules of C#:
            case sbyte sByteVal:
                value = (int)sByteVal;
                break;
            case byte byteVal:
                value = (int)byteVal;
                break;
            case short shortVal:
                value = (int)shortVal;
                break;
            case ushort uShortVal:
                value = (int)uShortVal;
                break;
            case uint uIntVal:
                value = (long)uIntVal;
                break;

            // Arrays are converted to values with no keys:
            case Array vectorVal:
                value = vectorVal;
                children = vectorVal
                    .Cast<object?>()
                    .Select(e => Process("", e))
                    .Where(e => e != null)
                    .Select(e => e!)
                    .ToArray();
                break;

            // Nested property maps are traversed:
            case AsepritePropertyMap mapVal:
                value = mapVal;
                children = Process(mapVal.Entries);
                break;

            // Other values are left untouched:
            default:
                value = input;
                break;
        }

        return new PropertyMapValue(children ?? Array.Empty<PropertyMapValue>(), key, value);
    }

    internal static PropertyMapValue[] Process(ReadOnlySpan<AsepritePropertyMapEntry> properties)
    {
        var result = new List<PropertyMapValue>();

        foreach (var property in properties)
        {
            if (Process(property.Key, property.Value) is not { } val)
                continue;

            result.Add(val);
        }

        return result.ToArray();
    }

    public static PropertyMap Process(AsepritePropertyMap propertyMap)
    {
        ArgumentNullException.ThrowIfNull(propertyMap);

        var values = Process(propertyMap.Entries);
        return new PropertyMap(propertyMap.ID, values);
    }

    public static PropertyMap Process(AsepriteUserData userData, int extensionId = 0)
    {
        ArgumentNullException.ThrowIfNull(userData);

        foreach (var propertyMap in userData.PropertyMaps)
        {
            if (propertyMap.ID == unchecked((uint)extensionId))
                return Process(propertyMap);
        }

        return new PropertyMap(unchecked((uint)extensionId), Array.Empty<PropertyMapValue>());
    }

    public static PropertyMap Process(AsepriteCel cel, int extensionId = 0)
    {
        ArgumentNullException.ThrowIfNull(cel);
        return Process(cel.UserData, extensionId);
    }

    public static PropertyMap Process(AsepriteLayer layer, int extensionId = 0)
    {
        ArgumentNullException.ThrowIfNull(layer);
        return Process(layer.UserData, extensionId);
    }

    public static PropertyMap Process(AsepriteSlice slice, int extensionId = 0)
    {
        ArgumentNullException.ThrowIfNull(slice);
        return Process(slice.UserData, extensionId);
    }

    public static PropertyMap Process(AsepriteTag tag, int extensionId = 0)
    {
        ArgumentNullException.ThrowIfNull(tag);
        return Process(tag.UserData, extensionId);
    }

    public static PropertyMap Process(AsepriteFile file, int extensionId = 0)
    {
        ArgumentNullException.ThrowIfNull(file);
        return Process(file.UserData, extensionId);
    }
}
