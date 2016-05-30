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
		public void String()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality("foo", "bar"))).ShouldThrow<AssertFailedException>();
			((Action) (() => ValueTypeAssertions.HasValueInequality("foo", "foo"))).ShouldThrow<AssertFailedException>();
		}
	}
}
