﻿// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueTypeAssertions.Tests.Incorrectly_Implemented_Types
{
	[TestClass]
	public class EqualsUsesHashCode
	{
		[TestMethod]
		public void EqualValues()
		{
			// Let's make a hash collision!
			((Action) (() => ValueTypeAssertions.HasValueInequality(new C(0, 33), new C(1, 0))))
				.ShouldThrow<AssertFailedException>()
				.And.Message.Should().Contain("Equals(object)");
		}

		private class C
		{
			public C(int x, int y)
			{
				X = x;
				Y = y;
			}

			public readonly int X;
			public readonly int Y;

			public override int GetHashCode()
			{
				// This uses (((h1 << 5) + h1) ^ h2);
				// http://referencesource.microsoft.com/#mscorlib/system/array.cs,87d117c8cc772cca
				return Tuple.Create(X, Y).GetHashCode();
			}

			public static bool operator ==(C left, C right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(C left, C right)
			{
				return !Equals(left, right);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				// Oops!
				return GetHashCode() == obj.GetHashCode();
			}
		}
	}
}