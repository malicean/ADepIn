using System.Diagnostics.CodeAnalysis;
using Moq;
using Xunit;

namespace Atlas.Fluent.Impl.Tests
{
	public class SingletonServiceBindingTests
	{
		[Fact]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Ctor()
		{
			new ConstantServiceBinding<bool>(true);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Get(bool value)
		{
			var mockResolver = new Mock<IServiceResolver>();
			var resolver = mockResolver.Object;
			var result = value;
			var binding = new SingletonServiceBinding<bool>(new FunctionalServiceBinding<bool>(() =>
			{
				var copy = result;
				result = !result;
				return copy;
			}));

			var ret1 = binding.Get(resolver);
			var ret2 = binding.Get(resolver);

			Assert.Equal(value, ret1);
			Assert.Equal(value, ret2);
			// Invert value because the function (which inverts result) is called once)
			Assert.Equal(!value, result);
		}
	}
}