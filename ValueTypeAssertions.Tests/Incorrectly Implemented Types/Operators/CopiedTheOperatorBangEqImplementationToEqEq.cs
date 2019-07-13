// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Incorrectly_Implemented_Types.Operators
{
	[TestClass]
	public class CopiedTheOperatorBangEqImplementationToEqEq
	{
		[TestMethod]
		public void EqualValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AClass(1), new AClass(1))))
				.Should().Throw<AssertFailedException>()
				.And.Message.Should().Contain("operator ==");
		}

		[TestMethod]
		public void DifferentValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AClass(1), new AClass(2))))
				.Should().Throw<AssertFailedException>()
				.And.Message.Should().Contain("operator ==");
		}

		private class AClass
		{
			public AClass(int x)
			{
				this.X = x;
			}

			public readonly int X;

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals((AClass) obj);
			}

			public override int GetHashCode()
			{
				return this.X;
			}

			public static bool operator ==(AClass left, AClass right)
			{
				// oops!
				return !Equals(left, right);
			}

			public static bool operator !=(AClass left, AClass right)
			{
				return !Equals(left, right);
			}

			protected bool Equals(AClass other)
			{
				return this.X == other.X;
			}
		}
	}
}
