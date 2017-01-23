﻿namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.Attributes
{
    using System;
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Helpers;
    using Xunit;
    using Xunit.Abstractions;

    public class AutoMoqDataAttributeExample : BaseAttributeExample
    {
        public AutoMoqDataAttributeExample(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [AutoMoqData]
        public void SimpleTypeGeneration(string name, int number)
        {
            Output.WriteLine($"Name: {name}");
            Output.WriteLine($"Number: {number}");
        }

        [Theory]
        [AutoMoqData]
        public void ComplexTypeGeneration(SampleClass testObject)
        {
            Output.WriteLine($"Not Virtual Property: {testObject.NotVirtualProperty}");

            testObject.VirtualProperty.Should().NotBeNull();
            Output.WriteLine($"Virtual Property: {testObject.VirtualProperty}");
        }

        [Theory]
        [AutoMoqData(IgnoreVirtualMembers = true)]
        public void ComplexTypeGenerationWithIgnoreVirtualMembers(SampleClass testObject)
        {
            Output.WriteLine($"Not Virtual Property: {testObject.NotVirtualProperty}");

            testObject.VirtualProperty.Should().BeNull();
            Output.WriteLine($"Virtual Property: {testObject.VirtualProperty}");
        }
    }
}
