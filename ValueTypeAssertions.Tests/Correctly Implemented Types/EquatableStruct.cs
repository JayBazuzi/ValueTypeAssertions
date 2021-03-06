﻿// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Correctly_Implemented_Types
{
	[TestClass]
	public class CorrectlyImplementedEquatableCustomStruct
	{
		[TestMethod]
		public void EqualValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AStruct(1), new AStruct(1)))).Should().NotThrow();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(1), new AStruct(1)))).Should().Throw<AssertFailedException>();
		}

		[TestMethod]
		public void DifferentValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AStruct(1), new AStruct(2)))).Should().Throw<AssertFailedException>();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(1), new AStruct(2)))).Should().NotThrow();
		}

		private struct AStruct : IEquatable<AStruct>
		{
			public readonly int X;

			public AStruct(int x)
			{
				this.X = x;
			}

			bool IEquatable<AStruct>.Equals(AStruct other) => Equals(other);

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
