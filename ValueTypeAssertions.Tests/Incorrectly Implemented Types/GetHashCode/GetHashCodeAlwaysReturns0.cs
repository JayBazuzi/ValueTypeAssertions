// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueTypeAssertions.Tests.Incorrectly_Implemented_Types
{
	[TestClass]
	public class GetHashCodeAlwaysReturns0
	{
		[TestMethod]
		public void ShouldFail()
		{
			((Action) (() => ValueTypeAssertions.HasValueInequality(new C(1), new C(2))))
				.ShouldThrow<AssertFailedException>()
				.And.Message.Should().Contain("GetHashCode()");
		}

		private class C
		{
			public C(int x)
			{
				this.X = x;
			}

			public readonly int X;

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals((C) obj);
			}

			public override int GetHashCode()
			{
				return 0;
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
				return this.X == other.X;
			}
		}
	}
}
