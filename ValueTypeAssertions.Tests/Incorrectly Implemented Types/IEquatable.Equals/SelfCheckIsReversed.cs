﻿// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Incorrectly_Implemented_Types.IEquatable.Equals
{
	[TestClass]
	public class SelfCheckIsReversed
	{
		[TestMethod]
		public void EqualValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new C(1), new C(1))))
				.Should().Throw<AssertFailedException>()
				.And.Message.Should().Contain("IEquatable<>.Equals(self)");
		}

		private class C : IEquatable<C>
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
				return this.X;
			}

			public static bool operator ==(C left, C right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(C left, C right)
			{
				return !Equals(left, right);
			}

			bool IEquatable<C>.Equals(C other)
			{
				if (ReferenceEquals(null, other)) return false;

				// Oops!
				if (ReferenceEquals(this, other)) return false;
				return this.X == other.X;
			}
		}
	}
}
