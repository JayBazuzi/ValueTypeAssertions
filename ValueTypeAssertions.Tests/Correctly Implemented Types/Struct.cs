// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueTypeAssertions.Tests
{
	[TestClass]
	public class CorrectlyImplementedCustomStruct
	{
		[TestMethod]
		public void EqualValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AStruct(1), new AStruct(1)))).ShouldNotThrow();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(1), new AStruct(1)))).ShouldThrow<AssertFailedException>();

			((Action) (() => ValueTypeAssertions.HasValueEquality(new AStruct(0), new AStruct()))).ShouldNotThrow();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(0), new AStruct()))).ShouldThrow<AssertFailedException>();
		}

		[TestMethod]
		public void DifferentValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AStruct(1), new AStruct(2)))).ShouldThrow<AssertFailedException>();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(1), new AStruct(2)))).ShouldNotThrow();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(1), new AStruct()))).ShouldNotThrow();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(), new AStruct(2)))).ShouldNotThrow();
		}

		private struct AStruct
		{
			public readonly int X;

			public AStruct(int x)
			{
				this.X = x;
			}

			public static bool operator ==(AStruct left, AStruct right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(AStruct left, AStruct right)
			{
				return !left.Equals(right);
			}
		}
	}
}
