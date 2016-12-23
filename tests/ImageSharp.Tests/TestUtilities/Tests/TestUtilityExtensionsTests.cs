﻿// <copyright file="FlagsHelper.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

// ReSharper disable InconsistentNaming
namespace ImageSharp.Tests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Reflection;

    using Xunit;
    using Xunit.Abstractions;

    public class TestUtilityExtensionsTests
    {
        public TestUtilityExtensionsTests(ITestOutputHelper output)
        {
            this.Output = output;
        }

        private ITestOutputHelper Output { get; }

        public static Image<TColor> CreateTestImage<TColor>(GenericFactory<TColor> factory)
            where TColor : struct, IPackedPixel, IEquatable<TColor>
        {
            Image<TColor> image = factory.CreateImage(10, 10);

            using (var pixels = image.Lock())
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Vector4 v = new Vector4(i, j, 0, 1);
                        v /= 10;

                        TColor color = default(TColor);
                        color.PackFromVector4(v);

                        pixels[i, j] = color;
                    }
                }
            }

            return image;
        }

        [Fact]
        public void Baz()
        {
            var type = typeof(Color).GetTypeInfo().Assembly.GetType("ImageSharp.Color");
            this.Output.WriteLine(type.ToString());

            var fake = typeof(Color).GetTypeInfo().Assembly.GetType("ImageSharp.dsaada_DASqewrr");
            Assert.Null(fake);
        }

        [Fact]
        public void GetPackedType()
        {
            Type shouldBeUIint32 = TestUtilityExtensions.GetPackedType(typeof(Color));

            Assert.Equal(shouldBeUIint32, typeof(uint));
        }

        [Theory]
        [WithFile(TestImages.Bmp.Car, PixelTypes.Color)]
        public void IsEquivalentTo_WhenFalse<TColor>(TestImageProvider<TColor> provider)
            where TColor : struct, IPackedPixel, IEquatable<TColor>
        {
            var a = provider.GetImage();
            var b = provider.GetImage();
            b = b.OilPaint(3, 2);

            Assert.False(a.IsEquivalentTo(b));
        }

        [Theory]
        [WithMemberFactory(nameof(CreateTestImage), PixelTypes.Color | PixelTypes.Bgr565)]
        public void IsEquivalentTo_WhenTrue<TColor>(TestImageProvider<TColor> provider)
            where TColor : struct, IPackedPixel, IEquatable<TColor>
        {
            var a = provider.GetImage();
            var b = provider.GetImage();

            Assert.True(a.IsEquivalentTo(b));
        }

        [Theory]
        [InlineData(PixelTypes.Color, typeof(Color))]
        [InlineData(PixelTypes.Argb, typeof(Argb))]
        [InlineData(PixelTypes.HalfVector4, typeof(HalfVector4))]
        [InlineData(PixelTypes.ColorWithDefaultImageClass, typeof(Color))]
        public void ToType(PixelTypes pt, Type expectedType)
        {
            Assert.Equal(pt.ToType(), expectedType);
        }

        [Theory]
        [InlineData(typeof(Color), PixelTypes.Color)]
        [InlineData(typeof(Argb), PixelTypes.Argb)]
        public void GetPixelType(Type clrType, PixelTypes expectedPixelType)
        {
            Assert.Equal(expectedPixelType, clrType.GetPixelType());
        }

        private static void AssertContainsPixelType<T>(
            PixelTypes pt,
            IEnumerable<KeyValuePair<PixelTypes, Type>> pixelTypesExp)
        {
            Assert.Contains(new KeyValuePair<PixelTypes, Type>(pt, typeof(T)), pixelTypesExp);
            
        }

        [Fact]
        public void ToTypes()
        {
            PixelTypes pixelTypes = PixelTypes.Alpha8 | PixelTypes.Bgr565 | PixelTypes.Color | PixelTypes.HalfVector2 | PixelTypes.ColorWithDefaultImageClass;

            var expanded = pixelTypes.ExpandAllTypes();

            Assert.Equal(expanded.Count(), 5);
            
            AssertContainsPixelType<Alpha8>(PixelTypes.Alpha8, expanded);
            AssertContainsPixelType<Bgr565>(PixelTypes.Bgr565, expanded);
            AssertContainsPixelType<Color>(PixelTypes.Color, expanded);
            AssertContainsPixelType<HalfVector2>(PixelTypes.HalfVector2, expanded);
            AssertContainsPixelType<Color>(PixelTypes.ColorWithDefaultImageClass, expanded);
        }

        [Fact]
        public void ToTypes_All()
        {
            var expanded = PixelTypes.All.ExpandAllTypes().ToArray();

            Assert.True(expanded.Length >= FlagsHelper<PixelTypes>.GetSortedValues().Length - 2);
            AssertContainsPixelType<Color>(PixelTypes.Color, expanded);
            AssertContainsPixelType<Color>(PixelTypes.ColorWithDefaultImageClass, expanded);
        }
    }
}