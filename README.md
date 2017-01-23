# Objectivity.AutoFixture.XUnit2.AutoMoq
Autofixture auto-mocking for [XUnit](http://xunit.github.io/) using [Moq](https://github.com/moq/moq4).
It provides 3 attriubutes:
- AutoMoqData
- InlineAutoMoqData
- MemberAutoMoqData

##AutoMoqData
Provides auto-generated data specimens generated by [AutoFixture](https://github.com/AutoFixture/AutoFixture) with [Moq](https://github.com/moq/moq4) as an extension to xUnit.net's Theory attribute.

#####Arguments:
- IgnoreVirtualMembers - disables generation members marked as `virtual`; by defaul set to `false`

#####Example:
```csharp
        [Theory]
        [AutoMoqData(IgnoreVirtualMembers = true)]
        public void ComplexTypeGenerationWithIgnoreVirtualMembers(Sample testObject)
        {
            output.WriteLine($"Not Virtual Property: {testObject.NotVirtualProperty}");

            testObject.VirtualProperty.Should().BeNull();
            output.WriteLine($"Virtual Property: {testObject.VirtualProperty}");
        }
```

##InlineAutoMoqData
#####Arguments:

#####Example:
```csharp
...
```

##MemberAutoMoqData
#####Arguments:

#####Example:
```csharp
...
```