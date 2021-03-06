﻿// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Incorrectly_Implemented_Types.Object.Equals
{
	[TestClass]
	public class TypeCheckIsReversed
	{
		[TestMethod]
		public void EqualValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AClass(1), new AClass(1))))
				.Should().Throw<AssertFailedException>()
				.And.Message.Should().Contain("compare to other type");
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

				// Oops!
				if (obj.GetType() != GetType()) return true;
				return Equals((AClass) obj);
			}

			public override int GetHashCode()
			{
				return this.X;
			}

			public static bool operator ==(AClass left, AClass right)
			{
				return Equals(left, right);
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
