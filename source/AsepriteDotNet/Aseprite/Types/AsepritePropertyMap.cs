// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines a property map within custom user data in an Aseprite file.
/// This class cannot be inherited.
/// </summary>
public sealed class AsepritePropertyMap
{
    private readonly ReadOnlyMemory<AsepritePropertyMapEntry> _entries;

    /// <summary>
    /// ID of the property map. Generic user properties will use a value of
    /// zero for this, but Aseprite extensions can populate their own data to
    /// keep it separate.
    /// </summary>
    public uint ID { get; }

    /// <summary>
    /// Key/value pairs in the property map.
    /// </summary>
    public ReadOnlySpan<AsepritePropertyMapEntry> Entries => _entries.Span;

    internal AsepritePropertyMap(uint id, ReadOnlyMemory<AsepritePropertyMapEntry> entries) =>
        (ID, _entries) = (id, entries);
}
