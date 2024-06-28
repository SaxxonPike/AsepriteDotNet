// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Types;

namespace AsepriteDotNet.Aseprite;

/// <summary>
/// Determines the type of value stored in a user data property.
/// </summary>
public enum AsepriteUserDataPropertyType
{
    /// <summary>
    /// The value is a boolean.
    /// </summary>
    Boolean = 1,

    /// <summary>
    /// The value is a signed 8-bit integer.
    /// </summary>
    SByte = 2,

    /// <summary>
    /// The value is an unsigned 8-bit integer.
    /// </summary>
    Byte = 3,

    /// <summary>
    /// The value is a signed 16-bit integer.
    /// </summary>
    Short = 4,

    /// <summary>
    /// The value is an unsigned 16-bit integer.
    /// </summary>
    UShort = 5,

    /// <summary>
    /// The value is a signed 32-bit integer.
    /// </summary>
    Int = 6,

    /// <summary>
    /// The value is an unsigned 32-bit integer.
    /// </summary>
    UInt = 7,

    /// <summary>
    /// The value is a signed 64-bit integer.
    /// </summary>
    Long = 8,

    /// <summary>
    /// The value is an unsigned 64-bit integer.
    /// </summary>
    ULong = 9,

    /// <summary>
    /// The value is a fixed-point number.
    /// </summary>
    Fixed = 10,

    /// <summary>
    /// The value is a 32-bit floating-point number.
    /// </summary>
    Float = 11,

    /// <summary>
    /// The value is a 64-bit floating-point number.
    /// </summary>
    Double = 12,

    /// <summary>
    /// The value is a string.
    /// </summary>
    String = 13,

    /// <summary>
    /// The value is a <see cref="Point"/> structure.
    /// </summary>
    Point = 14,

    /// <summary>
    /// The value is a <see cref="Size"/> structure.
    /// </summary>
    Size = 15,

    /// <summary>
    /// The value is a <see cref="Rectangle"/> structure.
    /// </summary>
    Rectangle = 16,

    /// <summary>
    /// The value is an array.
    /// </summary>
    Vector = 17,

    /// <summary>
    /// The value is a nested property set.
    /// </summary>
    Properties = 18,

    /// <summary>
    /// The value is a unique 128-bit ID.
    /// </summary>
    Guid = 19
}
