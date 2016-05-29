// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using System.Linq.Expressions;
using FluentAssertions;

namespace Bazuzi.ValueTypeAssertions
{
	public static class ValueTypeAssertions
	{
		public static void HasValueEquality<T>(T item, T equalItem)
		{
			object.ReferenceEquals(item, equalItem).Should().BeFalse("Pass two different references to compare.");

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

			((object) item).Equals(equalItem).Should().BeTrue("object.Equals()");

			item.ToString().ToUpperInvariant().Should().Be(equalItem.ToString().ToUpperInvariant(), "ToString()");

			item.GetHashCode().Should().Be(equalItem.GetHashCode(), "GetHashCode()");

			CallBinaryOperator<T, bool>(item, equalItem, Expression.Equal).Should().BeTrue("operator ==");
			CallBinaryOperator<T, bool>(item, equalItem, Expression.NotEqual).Should().BeFalse("operator !=");
		}

		public static void HasValueInequality<T>(T item, T differentItem)
		{
			if (item is IEquatable<T>)
			{
				var equatable = (IEquatable<T>) item;
				equatable.Equals(differentItem).Should().BeFalse("IEquatable<>.Equals");
			}

			item.Equals(differentItem).Should().BeFalse("Equals(object)");

			((object) item).Equals(differentItem).Should().BeFalse("object.Equals()");

			if (item.ToString() != item.GetType().FullName) { item.ToString().Should().NotBe(differentItem.ToString(), "ToString()"); }

			// Two unequal objects do not necessarily need to have differnet hash codes, but in practice
			// the chances of a hash collision are rare enough that this is a good test assertion.
			item.GetHashCode().Should().NotBe(differentItem.GetHashCode(), "GetHashCode()");

			CallBinaryOperator<T, bool>(item, differentItem, Expression.Equal).Should().BeFalse("operator ==");
			CallBinaryOperator<T, bool>(item, differentItem, Expression.NotEqual).Should().BeTrue("operator !=");
		}

		private static TResult CallBinaryOperator<T, TResult>(T item1,
			T item2,
			Func<Expression, Expression, BinaryExpression> @operator)
		{
			return Expression.Lambda<Func<TResult>>(@operator(Expression.Constant(item1), Expression.Constant(item2)))
				.Compile()();
		}
	}
}
