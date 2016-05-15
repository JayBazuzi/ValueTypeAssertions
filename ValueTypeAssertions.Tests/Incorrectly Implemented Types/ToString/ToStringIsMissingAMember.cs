// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueTypeAssertions.Tests
{
	[TestClass]
	public class ToStringIsMissingAMember
	{
		[TestMethod]
		public void DifferentValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueInequality(new C("foo", "bar"), new C("foo", "qux"))))
				.ShouldThrow<AssertFailedException>()
				.And.Message.Should().Contain("ToString()");
		}

		private class C
		{
			public C(string partA, string partB)
			{
				PartA = partA;
				PartB = partB;
			}

			public readonly string PartA;
			public readonly string PartB;

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals((C) obj);
			}

			public override int GetHashCode()
			{
				unchecked { return (PartA.GetHashCode()*397) ^ PartB.GetHashCode(); }
			}

			public static bool operator ==(C left, C right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(C left, C right)
			{
				return !Equals(left, right);
			}

			public override string ToString()
			{
				//return PartA + PartB;
				return PartA; // oops!

			}

			protected bool Equals(C other)
			{
				return string.Equals(PartA, other.PartA) && string.Equals(PartB, other.PartB);
			}
		}
	}
}
