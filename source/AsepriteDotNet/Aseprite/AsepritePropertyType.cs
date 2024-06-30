// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Aseprite;

/// <summary>
/// Determines the type of value stored in a user data property.
/// </summary>
public enum AsepritePropertyType
{
    /// <summary>
    /// The value is not present. The Aseprite documentation says this should
    /// not be found in any exported file.
    /// </summary>
    Null = 0,

    /// <summary>
    /// The value is a boolean.
    /// </summary>
    Bool8 = 1,

    /// <summary>
    /// The value is a signed 8-bit integer.
    /// </summary>
    I8 = 2,

    /// <summary>
    /// The value is an unsigned 8-bit integer.
    /// </summary>
    U8 = 3,

    /// <summary>
    /// The value is a signed 16-bit integer.
    /// </summary>
    I16 = 4,

    /// <summary>
    /// The value is an unsigned 16-bit integer.
    /// </summary>
    U16 = 5,

    /// <summary>
    /// The value is a signed 32-bit integer.
    /// </summary>
    I32 = 6,

    /// <summary>
    /// The value is an unsigned 32-bit integer.
    /// </summary>
    U32 = 7,

    /// <summary>
    /// The value is a signed 64-bit integer.
    /// </summary>
    I64 = 8,

    /// <summary>
    /// The value is an unsigned 64-bit integer.
    /// </summary>
    U64 = 9,

    /// <summary>
    /// The value is a fixed-point number.
    /// </summary>
    Fixed = 10,

    /// <summary>
    /// The value is a 32-bit floating-point number.
    /// </summary>
    F32 = 11,

    /// <summary>
    /// The value is a 64-bit floating-point number.
    /// </summary>
    F64 = 12,

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
    Uuid = 19
}
