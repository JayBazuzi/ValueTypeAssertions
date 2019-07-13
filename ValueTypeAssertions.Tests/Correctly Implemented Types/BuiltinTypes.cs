// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Correctly_Implemented_Types
{
	[TestClass]
	public class BuiltinTypes
	{
		[TestMethod]
		public void Int()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(1, 1))).Should().NotThrow();
			((Action) (() => ValueTypeAssertions.HasValueEquality(1, 2))).Should().Throw<AssertFailedException>();
			((Action) (() => ValueTypeAssertions.HasValueInequality(2, 1))).Should().NotThrow();
			((Action) (() => ValueTypeAssertions.HasValueInequality(2, 2))).Should().Throw<AssertFailedException>();
		}

		[TestMethod]
		public void String()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality("foo", "bar"))).Should().Throw<AssertFailedException>();
			((Action) (() => ValueTypeAssertions.HasValueInequality("foo", "foo"))).Should().Throw<AssertFailedException>();
		}
	}
}
