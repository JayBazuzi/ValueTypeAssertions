// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueTypeAssertions.Tests
{
	[TestClass]
	public class BuiltinTypes
	{
		[TestMethod]
		public void Int()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(1, 1))).ShouldNotThrow();
			((Action) (() => ValueTypeAssertions.HasValueEquality(1, 2))).ShouldThrow<AssertFailedException>();
			((Action) (() => ValueTypeAssertions.HasValueInequality(2, 1))).ShouldNotThrow();
			((Action) (() => ValueTypeAssertions.HasValueInequality(2, 2))).ShouldThrow<AssertFailedException>();
		}

		[TestMethod]
		public void String()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality("", ""))).ShouldNotThrow();
			((Action) (() => ValueTypeAssertions.HasValueEquality("foo", "foo"))).ShouldNotThrow();
			((Action) (() => ValueTypeAssertions.HasValueEquality("foo", "bar"))).ShouldThrow<AssertFailedException>();
			((Action) (() => ValueTypeAssertions.HasValueInequality("foo", "foo"))).ShouldThrow<AssertFailedException>();
		}
	}
}
