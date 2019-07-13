// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Correctly_Implemented_Types
{
	[TestClass]
	public class OperatorEqualsFromBaseClass
	{
		[TestMethod]
		public void EqualValues()
		{
			(new AClass(1) == new AClass(1)).Should().BeTrue();
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AClass(1), new AClass(1)))).Should().NotThrow();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AClass(1), new AClass(1)))).Should().Throw<AssertFailedException>();
		}

		[TestMethod]
		public void DifferentValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AClass(1), new AClass(2)))).Should().Throw<AssertFailedException>();
			((Action) (() => ValueTypeAssertions.HasValueInequality(new AClass(1), new AClass(2)))).Should().NotThrow();
		}

		private class AClass : BClass
		{
			public AClass(int x) : base(x) {}
		}

		private abstract class CClass
		{
			public static bool operator ==(CClass left, CClass right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(CClass left, CClass right)
			{
				return !Equals(left, right);
			}
		}

		private abstract class BClass : CClass
		{
			protected BClass(int x)
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

			public static bool operator ==(BClass left, BClass right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(BClass left, BClass right)
			{
				return !Equals(left, right);
			}

			public override string ToString()
			{
				return this.X.ToString();
			}

			protected bool Equals(AClass other)
			{
				return this.X == other.X;
			}
		}
	}
}
