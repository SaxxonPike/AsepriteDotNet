// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.Processors;

namespace AsepriteDotNet.Tests.Processors;

public sealed class PropertyMapProcessorTestFixture
{
    public const int LayerExtensionId = 1;
    public const int CelExtensionId = 2;
    public const int TagExtensionId = 3;
    public const int SliceExtensionId = 4;
    public const int FileExtensionId = 5;
    public const int UserDataExtensionId = 6;

    public string Name { get; } = "property-map-processor-test";
    public AsepriteFile AsepriteFile { get; }
    public AsepriteCel AsepriteCel { get; }
    public AsepriteLayer AsepriteLayer { get; }
    public AsepriteSlice AsepriteSlice { get; }
    public AsepriteTag AsepriteTag { get; }
    public AsepriteUserData AsepriteUserData { get; }

    public Guid Guid { get; } = Guid.NewGuid();

    private AsepriteUserDataPropertyMap CreateTestPropertyMap(int vendorId, string id) =>
        new(unchecked((uint)vendorId), [
            new AsepriteUserDataProperty("id", AsepriteUserDataPropertyType.String, id),
            new AsepriteUserDataProperty("sbyte", AsepriteUserDataPropertyType.SByte, sbyte.MaxValue),
            new AsepriteUserDataProperty("byte", AsepriteUserDataPropertyType.Byte, byte.MaxValue),
            new AsepriteUserDataProperty("short", AsepriteUserDataPropertyType.Short, short.MaxValue),
            new AsepriteUserDataProperty("ushort", AsepriteUserDataPropertyType.UShort, ushort.MaxValue),
            new AsepriteUserDataProperty("int", AsepriteUserDataPropertyType.Int, int.MaxValue),
            new AsepriteUserDataProperty("uint", AsepriteUserDataPropertyType.UInt, uint.MaxValue),
            new AsepriteUserDataProperty("long", AsepriteUserDataPropertyType.Long, long.MaxValue),
            new AsepriteUserDataProperty("ulong", AsepriteUserDataPropertyType.ULong, ulong.MaxValue),
            new AsepriteUserDataProperty("string", AsepriteUserDataPropertyType.String, "hello"),
            new AsepriteUserDataProperty("fixed", AsepriteUserDataPropertyType.Fixed, int.MaxValue / 65536.0f),
            new AsepriteUserDataProperty("float", AsepriteUserDataPropertyType.Float, float.MaxValue),
            new AsepriteUserDataProperty("double", AsepriteUserDataPropertyType.Double, double.MaxValue),
            new AsepriteUserDataProperty("point", AsepriteUserDataPropertyType.Point,
                new Point(int.MinValue, int.MaxValue)),
            new AsepriteUserDataProperty("size", AsepriteUserDataPropertyType.Size,
                new Size(int.MinValue, int.MaxValue)),
            new AsepriteUserDataProperty("rect", AsepriteUserDataPropertyType.Rectangle,
                new Rectangle(int.MinValue, int.MaxValue, int.MaxValue, int.MinValue)),
            new AsepriteUserDataProperty("vector", AsepriteUserDataPropertyType.Vector,
                new object?[] { "greetings", int.MaxValue }),
            new AsepriteUserDataProperty("props", AsepriteUserDataPropertyType.Properties,
                new AsepriteUserDataPropertyMap(0, [
                    new AsepriteUserDataProperty("howdy", AsepriteUserDataPropertyType.String, "hiya"),
                    new AsepriteUserDataProperty("bye", AsepriteUserDataPropertyType.Int, int.MaxValue)
                ])),
            new AsepriteUserDataProperty("uuid", AsepriteUserDataPropertyType.Guid, Guid)
        ]);

    private AsepriteUserDataPropertyMap[] CreateTestPropertyMaps(int vendorId, string id) =>
    [
        CreateTestPropertyMap(0, id),
        CreateTestPropertyMap(vendorId, id)
    ];

    public PropertyMapProcessorTestFixture()
    {
        const int width = 100;

        const int height = 100;

        var pixels = new Rgba32[width * height];

        AsepriteLayer = new AsepriteImageLayer(
            new AsepriteLayerProperties(),
            "test-layer") { UserData = { PropertyMaps = CreateTestPropertyMaps(LayerExtensionId, "layer") } };

        AsepriteCel = new AsepriteImageCel(
            new AsepriteCelProperties(),
            AsepriteLayer,
            new AsepriteImageCelProperties(),
            pixels) { UserData = { PropertyMaps = CreateTestPropertyMaps(CelExtensionId, "cel") } };

        var frame = new AsepriteFrame(
            "test-frame",
            100,
            100,
            100,
            [AsepriteCel]);

        AsepriteTag = new AsepriteTag(
            new AsepriteTagProperties(),
            "test-tag") { UserData = { PropertyMaps = CreateTestPropertyMaps(TagExtensionId, "tag") } };

        AsepriteSlice = new AsepriteSlice(
            "test-slice",
            false,
            false,
            []) { UserData = { PropertyMaps = CreateTestPropertyMaps(SliceExtensionId, "slice") } };

        AsepriteUserData = new AsepriteUserData
        {
            PropertyMaps = CreateTestPropertyMaps(UserDataExtensionId, "userdata")
        };

        AsepriteFile = new AsepriteFile(
            "property-map-file",
            new AsepritePalette(0),
            100,
            100,
            AsepriteColorDepth.RGBA,
            [frame],
            [AsepriteLayer],
            [AsepriteTag],
            [AsepriteSlice],
            [],
            new AsepriteUserData { PropertyMaps = CreateTestPropertyMaps(FileExtensionId, "file") },
            []);
    }
}

public class PropertyMapProcessorTests : IClassFixture<PropertyMapProcessorTestFixture>
{
    private readonly PropertyMapProcessorTestFixture _fixture;

    public PropertyMapProcessorTests(PropertyMapProcessorTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void Process_File_UsesFileUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.FileExtensionId;
        const string id = "file";

        PropertyMap propertyMap = PropertyMapProcessor.Process(_fixture.AsepriteFile, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_Cel_UsesCelUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.CelExtensionId;
        const string id = "cel";

        PropertyMap propertyMap = PropertyMapProcessor.Process(_fixture.AsepriteCel, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_Layer_UsesLayerUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.LayerExtensionId;
        const string id = "layer";

        PropertyMap propertyMap = PropertyMapProcessor.Process(_fixture.AsepriteLayer, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_Slice_UsesSliceUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.SliceExtensionId;
        const string id = "slice";

        PropertyMap propertyMap = PropertyMapProcessor.Process(_fixture.AsepriteSlice, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_Tag_UsesTagUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.TagExtensionId;
        const string id = "tag";

        PropertyMap propertyMap = PropertyMapProcessor.Process(_fixture.AsepriteTag, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_UserData_ReturnsEmptyMap_WhenInputIsEmpty()
    {
        PropertyMap propertyMap = PropertyMapProcessor.Process(_fixture.AsepriteUserData, int.MinValue);

        Assert.Equal(int.MinValue, propertyMap.Key);
        Assert.Equal(0, propertyMap.Properties.Length);
    }

    [Fact]
    public void Process_All_Throws_WhenInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => PropertyMapProcessor.Process((AsepriteFile)null!));
        Assert.Throws<ArgumentNullException>(() => PropertyMapProcessor.Process((AsepriteCel)null!));
        Assert.Throws<ArgumentNullException>(() => PropertyMapProcessor.Process((AsepriteLayer)null!));
        Assert.Throws<ArgumentNullException>(() => PropertyMapProcessor.Process((AsepriteTag)null!));
        Assert.Throws<ArgumentNullException>(() => PropertyMapProcessor.Process((AsepriteSlice)null!));
        Assert.Throws<ArgumentNullException>(() => PropertyMapProcessor.Process((AsepriteUserData)null!));
    }

    [Fact]
    public void Process_UserData_ProcessesPropertyMap()
    {
        const int extension = PropertyMapProcessorTestFixture.UserDataExtensionId;
        const string id = "userdata";

        PropertyMap propertyMap = PropertyMapProcessor.Process(_fixture.AsepriteUserData, extension);

        // Sanity checks.

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
        Assert.Equal(19, propertyMap.Properties.Length);

        // Bad input data assertions.

        Assert.Equal(propertyMap["invalid"]?.Value, null);

        // Single value assertions.

        var sByteProp = propertyMap["sbyte"];
        Assert.Equal(propertyMap[1], sByteProp);
        Assert.Equal(sByteProp?.Key, "sbyte");
        Assert.Equal(sByteProp?.HasChildren, false);
        Assert.Equal(sByteProp?.Value, (int)sbyte.MaxValue);

        var byteProp = propertyMap["byte"];
        Assert.Equal(propertyMap[2], byteProp);
        Assert.Equal(byteProp?.Key, "byte");
        Assert.Equal(byteProp?.HasChildren, false);
        Assert.Equal(byteProp?.Value, (int)byte.MaxValue);

        var shortProp = propertyMap["short"];
        Assert.Equal(propertyMap[3], shortProp);
        Assert.Equal(shortProp?.Key, "short");
        Assert.Equal(shortProp?.HasChildren, false);
        Assert.Equal(shortProp?.Value, (int)short.MaxValue);

        var uShortProp = propertyMap["ushort"];
        Assert.Equal(propertyMap[4], uShortProp);
        Assert.Equal(uShortProp?.Key, "ushort");
        Assert.Equal(uShortProp?.HasChildren, false);
        Assert.Equal(uShortProp?.Value, (int)ushort.MaxValue);

        var intProp = propertyMap["int"];
        Assert.Equal(propertyMap[5], intProp);
        Assert.Equal(intProp?.Key, "int");
        Assert.Equal(intProp?.HasChildren, false);
        Assert.Equal(intProp?.Value, int.MaxValue);

        var uIntProp = propertyMap["uint"];
        Assert.Equal(propertyMap[6], uIntProp);
        Assert.Equal(uIntProp?.Key, "uint");
        Assert.Equal(uIntProp?.HasChildren, false);
        Assert.Equal(uIntProp?.Value, (long)uint.MaxValue);

        var longProp = propertyMap["long"];
        Assert.Equal(propertyMap[7], longProp);
        Assert.Equal(longProp?.Key, "long");
        Assert.Equal(longProp?.HasChildren, false);
        Assert.Equal(longProp?.Value, long.MaxValue);

        var uLongProp = propertyMap["ulong"];
        Assert.Equal(propertyMap[8], uLongProp);
        Assert.Equal(uLongProp?.Key, "ulong");
        Assert.Equal(uLongProp?.HasChildren, false);
        Assert.Equal(uLongProp?.Value, ulong.MaxValue);

        var fixedProp = propertyMap["fixed"];
        Assert.Equal(propertyMap[10], fixedProp);
        Assert.Equal(fixedProp?.Key, "fixed");
        Assert.Equal(fixedProp?.HasChildren, false);
        Assert.Equal(fixedProp?.Value, int.MaxValue / 65536.0f);

        var floatProp = propertyMap["float"];
        Assert.Equal(propertyMap[11], floatProp);
        Assert.Equal(floatProp?.Key, "float");
        Assert.Equal(floatProp?.HasChildren, false);
        Assert.Equal(floatProp?.Value, float.MaxValue);

        var doubleProp = propertyMap["double"];
        Assert.Equal(propertyMap[12], doubleProp);
        Assert.Equal(doubleProp?.Key, "double");
        Assert.Equal(doubleProp?.HasChildren, false);
        Assert.Equal(doubleProp?.Value, double.MaxValue);

        var stringProp = propertyMap["string"];
        Assert.Equal(propertyMap[9], stringProp);
        Assert.Equal(stringProp?.Key, "string");
        Assert.Equal(stringProp?.HasChildren, false);
        Assert.Equal(stringProp?.Value, "hello");

        var pointProp = propertyMap["point"];
        Assert.Equal(propertyMap[13], pointProp);
        Assert.Equal(pointProp?.Key, "point");
        Assert.Equal(pointProp?.HasChildren, false);
        Assert.Equal(pointProp?.Value, new Point(int.MinValue, int.MaxValue));

        var sizeProp = propertyMap["size"];
        Assert.Equal(propertyMap[14], sizeProp);
        Assert.Equal(sizeProp?.Key, "size");
        Assert.Equal(sizeProp?.HasChildren, false);
        Assert.Equal(sizeProp?.Value, new Size(int.MinValue, int.MaxValue));

        var rectProp = propertyMap["rect"];
        Assert.Equal(propertyMap[15], rectProp);
        Assert.Equal(rectProp?.Key, "rect");
        Assert.Equal(rectProp?.HasChildren, false);
        Assert.Equal(rectProp?.Value, new Rectangle(int.MinValue, int.MaxValue, int.MaxValue, int.MinValue));

        var uuidProp = propertyMap["uuid"];
        Assert.Equal(propertyMap[18], uuidProp);
        Assert.Equal(uuidProp?.Key, "uuid");
        Assert.Equal(uuidProp?.HasChildren, false);
        Assert.Equal(uuidProp?.Value, _fixture.Guid);

        // Vector type assertions.

        var vector = propertyMap["vector"]!;
        Assert.Equal(propertyMap[16], vector);
        Assert.Equal(vector.Key, "vector");
        Assert.Equal(vector.HasChildren, true);
        Assert.Equal(vector.Children.Length, 2);

        Assert.Equal(vector[0].Value, "greetings");
        Assert.Equal(vector[0].HasChildren, false);
        Assert.Equal(vector[0].Key, string.Empty);
        Assert.Equal(vector[1].Value, int.MaxValue);
        Assert.Equal(vector[1].HasChildren, false);
        Assert.Equal(vector[1].Key, string.Empty);

        // Property set type assertions.

        var nestProps = propertyMap["props"]!;
        Assert.Equal(propertyMap[17], nestProps);
        Assert.Equal(nestProps.Key, "props");
        Assert.Equal(nestProps.HasChildren, true);

        Assert.Equal(nestProps["howdy"]?.Value, "hiya");
        Assert.Equal(nestProps["bye"]?.Value, int.MaxValue);
    }

    [Fact]
    public void Process_OutputsNull_WhenInputValueIsNull()
    {
        var value = PropertyMapProcessor.Process("any key", null);
        Assert.Null(value);
    }
}
