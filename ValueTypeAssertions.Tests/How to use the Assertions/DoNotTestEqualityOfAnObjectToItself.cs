// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.How_to_use_the_Assertions
{
	[TestClass]
	public class DoNotTestEqualityOfAnObjectToItself
	{
		[TestMethod]
		public void Passing_the_same_object_twice_is_an_error()
		{
			var anObject = new C(42);
			((Action) (() => ValueTypeAssertions.HasValueEquality(anObject, anObject)))
				.ShouldThrow<AssertFailedException>()
				.And.Message.Should().Contain("Pass two different references to compare.");
		}

		private class C
		{
			public C(int value)
			{
				this._value = value;
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals((C) obj);
			}

			public override int GetHashCode()
			{
				return this._value;
			}

			public static bool operator ==(C left, C right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(C left, C right)
			{
				return !Equals(left, right);
			}

			protected bool Equals(C other)
			{
				return this._value == other._value;
			}

			private readonly int _value;
		}
	}
}
