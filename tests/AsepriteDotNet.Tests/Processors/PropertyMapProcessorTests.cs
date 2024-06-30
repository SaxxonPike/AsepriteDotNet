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

    public const int NumberOfTypes = 20;

    public string Name { get; } = "property-map-processor-test";
    public AsepriteFile AsepriteFile { get; }
    public AsepriteCel AsepriteCel { get; }
    public AsepriteLayer AsepriteLayer { get; }
    public AsepriteSlice AsepriteSlice { get; }
    public AsepriteTag AsepriteTag { get; }
    public AsepriteUserData AsepriteUserData { get; }

    public Guid Uuid { get; } = Guid.NewGuid();

    private AsepritePropertyMap CreateTestPropertyMap(int vendorId, string id) =>
        new(unchecked((uint)vendorId), new[]
        {
            new AsepritePropertyMapEntry("id", AsepritePropertyType.String, id),
            new AsepritePropertyMapEntry("sbyte", AsepritePropertyType.I8, sbyte.MaxValue),
            new AsepritePropertyMapEntry("byte", AsepritePropertyType.U8, byte.MaxValue),
            new AsepritePropertyMapEntry("short", AsepritePropertyType.I16, short.MaxValue),
            new AsepritePropertyMapEntry("ushort", AsepritePropertyType.U16, ushort.MaxValue),
            new AsepritePropertyMapEntry("int", AsepritePropertyType.I32, int.MaxValue),
            new AsepritePropertyMapEntry("uint", AsepritePropertyType.U32, uint.MaxValue),
            new AsepritePropertyMapEntry("long", AsepritePropertyType.I64, long.MaxValue),
            new AsepritePropertyMapEntry("ulong", AsepritePropertyType.U64, ulong.MaxValue),
            new AsepritePropertyMapEntry("string", AsepritePropertyType.String, "hello"),
            new AsepritePropertyMapEntry("fixed", AsepritePropertyType.Fixed, int.MaxValue / 65536.0f),
            new AsepritePropertyMapEntry("float", AsepritePropertyType.F32, float.MaxValue),
            new AsepritePropertyMapEntry("double", AsepritePropertyType.F64, double.MaxValue),
            new AsepritePropertyMapEntry("point", AsepritePropertyType.Point,
                new Point(int.MinValue, int.MaxValue)),
            new AsepritePropertyMapEntry("size", AsepritePropertyType.Size,
                new Size(int.MinValue, int.MaxValue)),
            new AsepritePropertyMapEntry("rect", AsepritePropertyType.Rectangle,
                new Rectangle(int.MinValue, int.MaxValue, int.MaxValue, int.MinValue)),
            new AsepritePropertyMapEntry("vector", AsepritePropertyType.Vector,
                new object?[] { "greetings", int.MaxValue }),
            new AsepritePropertyMapEntry("props", AsepritePropertyType.Properties,
                new AsepritePropertyMap(0,
                    new[]
                    {
                        new AsepritePropertyMapEntry("howdy", AsepritePropertyType.String, "hiya"),
                        new AsepritePropertyMapEntry("bye", AsepritePropertyType.I32, int.MaxValue)
                    })),
            new AsepritePropertyMapEntry("uuid", AsepritePropertyType.Uuid, Uuid),
            new AsepritePropertyMapEntry("bool", AsepritePropertyType.Bool8, true)
        });

    private AsepritePropertyMap[] CreateTestPropertyMaps(int vendorId, string id) =>
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
            "test-layer") { UserData = { PropertyMapData = CreateTestPropertyMaps(LayerExtensionId, "layer") } };

        AsepriteCel = new AsepriteImageCel(
            new AsepriteCelProperties(),
            AsepriteLayer,
            new AsepriteImageCelProperties(),
            pixels) { UserData = { PropertyMapData = CreateTestPropertyMaps(CelExtensionId, "cel") } };

        var frame = new AsepriteFrame(
            "test-frame",
            100,
            100,
            100,
            [AsepriteCel]);

        AsepriteTag = new AsepriteTag(
            new AsepriteTagProperties(),
            "test-tag") { UserData = { PropertyMapData = CreateTestPropertyMaps(TagExtensionId, "tag") } };

        AsepriteSlice = new AsepriteSlice(
            "test-slice",
            false,
            false,
            []) { UserData = { PropertyMapData = CreateTestPropertyMaps(SliceExtensionId, "slice") } };

        AsepriteUserData = new AsepriteUserData
        {
            PropertyMapData = CreateTestPropertyMaps(UserDataExtensionId, "userdata")
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
            new AsepriteUserData { PropertyMapData = CreateTestPropertyMaps(FileExtensionId, "file") },
            []);
    }
}

public class PropertyMapProcessorTests(PropertyMapProcessorTestFixture fixture)
    : IClassFixture<PropertyMapProcessorTestFixture>
{
    [Fact]
    public void Process_File_UsesFileUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.FileExtensionId;
        const string id = "file";

        PropertyMap propertyMap = PropertyMapProcessor.Process(fixture.AsepriteFile, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_Cel_UsesCelUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.CelExtensionId;
        const string id = "cel";

        PropertyMap propertyMap = PropertyMapProcessor.Process(fixture.AsepriteCel, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_Layer_UsesLayerUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.LayerExtensionId;
        const string id = "layer";

        PropertyMap propertyMap = PropertyMapProcessor.Process(fixture.AsepriteLayer, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_Slice_UsesSliceUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.SliceExtensionId;
        const string id = "slice";

        PropertyMap propertyMap = PropertyMapProcessor.Process(fixture.AsepriteSlice, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_Tag_UsesTagUserData()
    {
        const int extension = PropertyMapProcessorTestFixture.TagExtensionId;
        const string id = "tag";

        PropertyMap propertyMap = PropertyMapProcessor.Process(fixture.AsepriteTag, extension);

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
    }

    [Fact]
    public void Process_UserData_ReturnsEmptyMap_WhenInputIsEmpty()
    {
        PropertyMap propertyMap = PropertyMapProcessor.Process(fixture.AsepriteUserData, int.MinValue);

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

        PropertyMap propertyMap = PropertyMapProcessor.Process(fixture.AsepriteUserData, extension);

        // Sanity checks.

        Assert.Equal(extension, propertyMap.Key);
        Assert.Equal(id, propertyMap["id"]?.Value);
        Assert.Equal(PropertyMapProcessorTestFixture.NumberOfTypes, propertyMap.Properties.Length);

        // Bad input data assertions.

        Assert.Null(propertyMap["invalid"]?.Value);

        // Single value assertions.

        var sByteProp = propertyMap["sbyte"];
        Assert.Equal(propertyMap[1], sByteProp);
        Assert.Equal("sbyte", sByteProp?.Key);
        Assert.False(sByteProp?.HasChildren);
        Assert.Equal((int)sbyte.MaxValue, sByteProp?.Value);

        var byteProp = propertyMap["byte"];
        Assert.Equal(propertyMap[2], byteProp);
        Assert.Equal("byte", byteProp?.Key);
        Assert.False(byteProp?.HasChildren);
        Assert.Equal((int)byte.MaxValue, byteProp?.Value);

        var shortProp = propertyMap["short"];
        Assert.Equal(propertyMap[3], shortProp);
        Assert.Equal("short", shortProp?.Key);
        Assert.False(shortProp?.HasChildren);
        Assert.Equal((int)short.MaxValue, shortProp?.Value);

        var uShortProp = propertyMap["ushort"];
        Assert.Equal(propertyMap[4], uShortProp);
        Assert.Equal("ushort", uShortProp?.Key);
        Assert.False(uShortProp?.HasChildren);
        Assert.Equal((int)ushort.MaxValue, uShortProp?.Value);

        var intProp = propertyMap["int"];
        Assert.Equal(propertyMap[5], intProp);
        Assert.Equal("int", intProp?.Key);
        Assert.False(intProp?.HasChildren);
        Assert.Equal(int.MaxValue, intProp?.Value);

        var uIntProp = propertyMap["uint"];
        Assert.Equal(propertyMap[6], uIntProp);
        Assert.Equal("uint", uIntProp?.Key);
        Assert.False(uIntProp?.HasChildren);
        Assert.Equal((long)uint.MaxValue, uIntProp?.Value);

        var longProp = propertyMap["long"];
        Assert.Equal(propertyMap[7], longProp);
        Assert.Equal("long", longProp?.Key);
        Assert.False(longProp?.HasChildren);
        Assert.Equal(long.MaxValue, longProp?.Value);

        var uLongProp = propertyMap["ulong"];
        Assert.Equal(propertyMap[8], uLongProp);
        Assert.Equal("ulong", uLongProp?.Key);
        Assert.False(uLongProp?.HasChildren);
        Assert.Equal(ulong.MaxValue, uLongProp?.Value);

        var fixedProp = propertyMap["fixed"];
        Assert.Equal(propertyMap[10], fixedProp);
        Assert.Equal("fixed", fixedProp?.Key);
        Assert.False(fixedProp?.HasChildren);
        Assert.Equal(int.MaxValue / 65536.0f, fixedProp?.Value);

        var floatProp = propertyMap["float"];
        Assert.Equal(propertyMap[11], floatProp);
        Assert.Equal("float", floatProp?.Key);
        Assert.False(floatProp?.HasChildren);
        Assert.Equal(float.MaxValue, floatProp?.Value);

        var doubleProp = propertyMap["double"];
        Assert.Equal(propertyMap[12], doubleProp);
        Assert.Equal("double", doubleProp?.Key);
        Assert.False(doubleProp?.HasChildren);
        Assert.Equal(double.MaxValue, doubleProp?.Value);

        var stringProp = propertyMap["string"];
        Assert.Equal(propertyMap[9], stringProp);
        Assert.Equal("string", stringProp?.Key);
        Assert.False(stringProp?.HasChildren);
        Assert.Equal("hello", stringProp?.Value);

        var pointProp = propertyMap["point"];
        Assert.Equal(propertyMap[13], pointProp);
        Assert.Equal("point", pointProp?.Key);
        Assert.False(pointProp?.HasChildren);
        Assert.Equal(new Point(int.MinValue, int.MaxValue), pointProp?.Value);

        var sizeProp = propertyMap["size"];
        Assert.Equal(propertyMap[14], sizeProp);
        Assert.Equal("size", sizeProp?.Key);
        Assert.False(sizeProp?.HasChildren);
        Assert.Equal(new Size(int.MinValue, int.MaxValue), sizeProp?.Value);

        var rectProp = propertyMap["rect"];
        Assert.Equal(propertyMap[15], rectProp);
        Assert.Equal("rect", rectProp?.Key);
        Assert.False(rectProp?.HasChildren);
        Assert.Equal(new Rectangle(int.MinValue, int.MaxValue, int.MaxValue, int.MinValue), rectProp?.Value);

        var uuidProp = propertyMap["uuid"];
        Assert.Equal(propertyMap[18], uuidProp);
        Assert.Equal("uuid", uuidProp?.Key);
        Assert.False(uuidProp?.HasChildren);
        Assert.Equal(fixture.Uuid, uuidProp?.Value);

        var boolProp = propertyMap["bool"];
        Assert.Equal(propertyMap[19], boolProp);
        Assert.Equal("bool", boolProp?.Key);
        Assert.False(boolProp?.HasChildren);
        Assert.Equal(true, boolProp?.Value);

        // Vector type assertions.

        var vector = propertyMap["vector"]!;
        Assert.Equal(propertyMap[16], vector);
        Assert.Equal("vector", vector.Key);
        Assert.True(vector.HasChildren);
        Assert.Equal(2, vector.Children.Length);

        Assert.Equal("greetings", vector[0].Value);
        Assert.False(vector[0].HasChildren);
        Assert.Equal(string.Empty, vector[0].Key);
        Assert.Equal(int.MaxValue, vector[1].Value);
        Assert.False(vector[1].HasChildren);
        Assert.Equal(string.Empty, vector[1].Key);

        // Property set type assertions.

        var nestProps = propertyMap["props"]!;
        Assert.Equal(propertyMap[17], nestProps);
        Assert.Equal("props", nestProps.Key);
        Assert.True(nestProps.HasChildren);

        Assert.Equal("hiya", nestProps["howdy"]?.Value);
        Assert.Equal(int.MaxValue, nestProps["bye"]?.Value);
    }

    [Fact]
    public void Process_OutputsNull_WhenInputValueIsNull()
    {
        var value = PropertyMapProcessor.Process("any key", null);
        Assert.Null(value);
    }
}
