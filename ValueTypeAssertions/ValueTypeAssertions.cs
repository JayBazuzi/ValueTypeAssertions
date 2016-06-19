// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using System.Reflection;
using FluentAssertions;

namespace Bazuzi.ValueTypeAssertions
{
	public static class ValueTypeAssertions
	{
		public static void HasValueEquality<T>(T item, T equalItem)
		{
			ReferenceEquals(item, equalItem).Should().BeFalse("Pass two different references to compare.");

			if (item is IEquatable<T>)
			{
				var equatable = (IEquatable<T>) item;
				equatable.Equals(equalItem).Should().BeTrue("IEquatable<>.Equals");
				equatable.Equals(item).Should().BeTrue("IEquatable<>.Equals(self)");
				equatable.Invoking(_ => _.Equals(default(T))).ShouldNotThrow<NullReferenceException>("IEquatable<>.Equals(null)");
				equatable.Equals(default(T)).Should().BeFalse("IEquatable<>.Equals(null)");
			}

			item.Equals(equalItem).Should().BeTrue("Equals(object)");
			item.Equals(item).Should().BeTrue("Equals(self)");
			item.Invoking(_ => _.Equals(null)).ShouldNotThrow<NullReferenceException>("Equals(object null)");
			item.Equals(null).Should().BeFalse("Equals(object null)");
			item.Invoking(_ => _.Equals(new object())).ShouldNotThrow<InvalidCastException>("compare to other type");
			item.Equals(new object()).Should().BeFalse("compare to other type");

			item.ToString().ToUpperInvariant().Should().Be(equalItem.ToString().ToUpperInvariant(), "ToString()");

			item.GetHashCode().Should().Be(equalItem.GetHashCode(), "GetHashCode()");

			CallComparisonOperator(item, equalItem, Operator.Equality).Should().BeTrue("operator ==");
			CallComparisonOperator(item, equalItem, Operator.Inequality).Should().BeFalse("operator !=");
		}

		public static void HasValueInequality<T>(T item, T differentItem)
		{
			if (item is IEquatable<T>)
			{
				var equatable = (IEquatable<T>) item;
				equatable.Equals(differentItem).Should().BeFalse("IEquatable<>.Equals");
			}

			item.Equals(differentItem).Should().BeFalse("Equals(object)");

			if (item.ToString() != item.GetType().FullName) { item.ToString().Should().NotBe(differentItem.ToString(), "ToString()"); }

			// Two unequal objects do not necessarily need to have differnet hash codes, but in practice
			// the chances of a hash collision are rare enough that this is a good test assertion.
			item.GetHashCode().Should().NotBe(differentItem.GetHashCode(), "GetHashCode()");

			CallComparisonOperator(item, differentItem, Operator.Equality).Should().BeFalse("operator ==");
			CallComparisonOperator(item, differentItem, Operator.Inequality).Should().BeTrue("operator !=");
		}

		private static bool CallComparisonOperator<T>(T item1, T item2, Operator @operator)
		{
			return GetOverloadedComparisonOperator(item1, item2, @operator) ?? GetBuiltinComparisonOperator(item1, item2, @operator);
		}

		private static bool GetBuiltinComparisonOperator<T>(T item1, T item2, Operator @operator)
		{
			switch (@operator)
			{
				case Operator.Equality:
					return (dynamic) item1 == (dynamic) item2;
				case Operator.Inequality:
					return (dynamic) item1 != (dynamic) item2;
			}
			throw new InvalidOperationException();
		}

		private static bool? GetOverloadedComparisonOperator<T>(T item1, T item2, Operator @operator)
		{
			var methodInfo = typeof(T).GetMethod(
				"op_" + @operator,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy,
				null,
				new[] {item1.GetType(), item2.GetType()},
				null);

			return (bool?) methodInfo?.Invoke(null, new object[] {item1, item2});
		}

		private enum Operator
		{
			Equality,
			Inequality
		}
	}
}
