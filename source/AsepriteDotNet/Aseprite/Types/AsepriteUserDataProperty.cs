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
    ///
    /// </summary>
    public string Key { get; }

    /// <summary>
    ///
    /// </summary>
    public object? Value { get; }

    internal AsepriteUserDataProperty(string key, object? value) =>
        (Key, Value) = (key, value);
}
