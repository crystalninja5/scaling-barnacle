# Objectivity.AutoFixture.XUnit2.AutoMoq
Autofixture auto-mocking for [XUnit](http://xunit.github.io/) using [Moq](https://github.com/moq/moq4).
It provides 3 attributes:
- AutoMoqData
- InlineAutoMoqData
- MemberAutoMoqData

#Attributes
##AutoMoqData
Provides auto-generated data specimens generated by [AutoFixture](https://github.com/AutoFixture/AutoFixture) with [Moq](https://github.com/moq/moq4) as an extension to xUnit.net's Theory attribute.

#####Arguments:
- IgnoreVirtualMembers - disables generation members marked as `virtual`; by default set to `false`

#####Example:
```csharp
[Theory]
[AutoMoqData]
public void GivenCurrencyConverter_WhenConvertToPln_ThenMustReturnCorrectConvertedAmmount(
	string testCurrencySymbol,
    [Frozen] ICurrencyExchangeProvider currencyProvider,
    CurrencyConverter currencyConverter)
{
	// Arrange
    Mock.Get(currencyProvider).Setup(cp => cp.GetCurrencyExchangeRate(testCurrencySymbol)).Returns(100M);

    // Act 
    decimal result = currencyConverter.ConvertToPln(testCurrencySymbol, 100M);

	// assert 
    Assert.Equal(10000M, result);
}
```

##InlineAutoMoqData
Provides a data source for a data theory, with the data coming from inline values combined with auto-generated data specimens generated by [AutoFixture](https://github.com/AutoFixture/AutoFixture) with [Moq](https://github.com/moq/moq4).
#####Arguments:
- IgnoreVirtualMembers - disables generation members marked as `virtual`; by default set to `false`

#####Example:
```csharp
[Theory]
[InlineAutoMoqData("objectId1", "objectId2", true)]
[InlineAutoMoqData("objectId1", "objectId1", false)]
public void GivenGuildService_WhenGetGuildWhichNotExistsOrAreNotEnabled_ThenReturnNull(
	string objectId, 
    string searchByobjectId, 
    bool isEnabled, 
    Guild expectedGuild, 
    [Frozen] IGenericRepository<Guild> guildRepository, 
    GuildService guildService)
{
	// Arrange
    expectedGuild.ObjectId = objectId;
    expectedGuild.IsEnabled = isEnabled;
    Mock.Get(guildRepository).Setup(r => r.GetAll()).Returns(new[] { expectedGuild }.AsQueryable());

    // Act
    Guild guild = guildService.GetFirstOrDefaultEnabledGuild(new[] { searchByobjectId });

    // Assert
    Assert.Null(guild);
}
```

##MemberAutoMoqData
Provides a data source for a data theory, with the data coming from one of the following sources:
- A static property
- A static field
- A static method (with parameters)

combined with auto-generated data specimens generated by [AutoFixture](https://github.com/AutoFixture/AutoFixture) with [Moq](https://github.com/moq/moq4).

The member must return something compatible with `Enumerable<object[]>` with the test data.
**Caution:** The property is completely enumerated by .ToList() before any test is run. Hence it should return independent object sets.
#####Arguments:
- IgnoreVirtualMembers - disables generation members marked as `virtual`; by default set to `false`
- ShareFixture - indicates whether to share a `fixture` across all data items should be used or new one; by default set to `true`

#####Example:
```csharp
public class GuildServiceFixture
{
	public static IEnumerable<object[]> GuildsWhichNotExistsOrAreNotEnabled()
    {
    	return new List<object[]>()
        {
        	new object[] { "objectId1", "objectId2", true },
            new object[] { "objectId1", "objectId1", false },
        };
    }
}

...

[Theory]
[MemberAutoMoqData("GuildsWhichNotExistsOrAreNotEnabled", MemberType = typeof(GuildServiceFixture))]
public void GivenGuildService_WhenGetGuildWhichNotExistsOrIsNotEnabled_ThenReturnNull(
	string objectId,
    string searchByobjectId,
    bool isEnabled,
    Guild expectedGuild,
    [Frozen] IGenericRepository<Guild> guildRepository,
    GuildService guildService)
{
	// Arrange
    expectedGuild.ObjectId = objectId;
    expectedGuild.IsEnabled = isEnabled;
    Mock.Get(guildRepository).Setup(r => r.GetAll()).Returns(new[] { expectedGuild }.AsQueryable());

    // Act
    Guild guild = guildService.GetFirstOrDefaultEnabledGuild(new[] { searchByobjectId });

    // Assert
    Assert.Null(guild);
}
```

#Tips and tricks
##Fixture injection
You can inject same instance of `IFixture` to a test method by adding mentioned interface as an argument of test method.
```csharp
[Theory]
[AutoMoqData]
public void FixtureInjection(IFixture fixture)
{
	Assert.NotNull(fixture);
}
```

## IgnoreVirtualMembers issue
You should be aware that the *CLR* requires that interface methods be marked as virtual. Please look at the following example:
```csharp
public interface IUser
{
	string Name { get; set; }
	User Substitute { get; set; }
	string Surname { get; set; }
}

public class User : IUser
{
	public string Name { get; set; }
	public virtual User Substitute { get; set; }
	public string Surname { get; set; }
}
```
You can see than only `Substitute` property has been explicitly marked as `virtual`. In such situation *the compiler* will mark other properties as `virtual` and `sealed`. And finally *AutoFixture* will assign `null` value to those properties when option `IgnoreVirtualMembers` will be set to `true`.

```csharp
[Theory]
[AutoMoqData(IgnoreVirtualMembers = true)]
public void IssueWithClassThatImplementsInterface(User user)
{
	Assert.Null(user.Name);
    Assert.Null(user.Surname);
    Assert.Null(user.Substitute);
}
```