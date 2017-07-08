using System;
using Shouldly;
using Xunit;namespace Structura.Shared.Utilities.Tests
{
	public class InMemoryCacheTests : IClassFixture<InMemoryCache>
	{
		private InMemoryCache _implementation;
		private readonly object _someValue = new object();
		private string _someKey = "Akey";
		public void SetFixture(InMemoryCache data)
		{
			_implementation = data;
		}		[Fact]
		public void InMemoryCache_SetOrAdd_WhenKeyIsNull_ThenShouldThrowArgumentNullException()
		{
			Should.Throw<ArgumentNullException>(() => _implementation.SetOrAdd(null, _someValue));
		}		[Fact]
		public void InMemoryCache_SetOrAdd_WhenValueIsNull_ThenGetShouldReturnNull()
		{			_implementation.SetOrAdd(_someKey, null);			_implementation.Get<object>(_someKey).ShouldBeNull();
		}
		[Fact]
		public void InMemoryCache_Get_WhenValueIsNull_And_Requesting_DifferentType_ThenGetShouldReturnNull()
		{			_implementation.SetOrAdd(_someKey, (Uri)null);			_implementation.Get<string>(_someKey).ShouldBeNull();
		}		[Fact]
		public void InMemoryCache_Get_WhenValueIsAdded_And_Requesting_ParentType_ThenGetShouldInstance()
		{			_implementation.SetOrAdd(_someKey, new MySubClass());			_implementation.Get<MyClass>(_someKey).ShouldNotBeNull();
		}
		[Fact]
		public void InMemoryCache_SetOrAdd_WhenValueIsAnObject_ThenGetShouldReturnSameObject()
		{			var someObject = new object();
			_implementation.SetOrAdd(_someKey, someObject);			_implementation.Get<object>(_someKey).ShouldBe(someObject);
		}
		[Fact]
		public void InMemoryCache_SetOrAdd_WhenValueIsAnString_ThenGetShouldReturnSameObject()
		{			var someObject = "eens proberen";
			_implementation.SetOrAdd(_someKey, someObject);
			_implementation.Get<string>(_someKey).ShouldBe(someObject);
		}
		[Fact]
		public void InMemoryCache_SetOrAdd_WhenValueIsAnInt_ThenGetShouldReturnSameObject()
		{			var someObject = 10;
			_implementation.SetOrAdd(_someKey, someObject);			_implementation.Get<int>(_someKey).ShouldBe(someObject);
		}
		[Fact]
		public void InMemoryCache_ContainsKey_WhenKeyIsNull_ThenShouldThrowArgumentNullException()
		{
			Should.Throw<ArgumentNullException>(() => _implementation.ContainsKey(null));
		}
		[Fact]
		public void InMemoryCache_Get_WhenKeyIsNull_ThenShouldThrowArgumentNullException()
		{
			Should.Throw<ArgumentNullException>(() => _implementation.Get<object>(null));
		}
		[Fact]
		public void InMemoryCache_ContainsKey_WhenAnValueWasAdded_ThenShouldReturnTrue()
		{			_implementation.SetOrAdd(_someKey, _someValue);			_implementation.ContainsKey(_someKey).ShouldBeTrue();
		}
		[Fact]
		public void InMemoryCache_ContainsKey_WhenAnValueWasNotAdded_ThenShouldReturnFalse()
		{			bool computed = _implementation.ContainsKey("NotExistingKey");			computed.ShouldBeFalse();
		}
		[Fact]
		public void InMemoryCache_Get_WhenAnValueWasNotAdded_ThenShouldThrowArgumentException()
		{
			Should.Throw<ArgumentException>(() => _implementation.Get<object>("NotExistingKey"));
		}
		[Fact]
		public void InMemoryCache_Get_WhenAnValueOfDifferentTypeWasAdded_ThenShouldThrowException()
		{			_implementation.SetOrAdd(_someKey, new SomeObject());			Should
				.Throw<ArgumentException>(
					() => _implementation.Get<OtherObject>(_someKey))
				.Message.StartsWith($"Aan InMemoryCache is gevraagd om key {_someKey} op te vragen met type OtherObject");
		}
		[Fact]
		public void InMemoryCache_Get_WhenValueIsAString_And_RequestingUri_ThenShouldThrowException()
		{			_implementation.SetOrAdd(_someKey, "");
			Should
				.Throw<ArgumentException>(
					() => _implementation.Get<Uri>(_someKey))
				.Message.StartsWith($"Aan InMemoryCache is gevraagd om key {_someKey} op te vragen met type ");
		}

		[Fact]
		public void InMemoryCache_Get_WhenAnValueOfDifferentTypeWasAdded_ThenShouldThrow()
		{			_implementation.SetOrAdd(_someKey, "hallo");			Should
				.Throw<ArgumentException>(() => _implementation.Get<Uri>(_someKey))
				.Message.StartsWith($"Aan InMemoryCache is gevraagd om key {_someKey} op te vragen met type ");
		}		private class MyClass
		{		}		private class MySubClass : MyClass
		{		}		private class SomeObject
		{
		}		private class OtherObject
		{		}
	}
}