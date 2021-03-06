﻿// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Incorrectly_Implemented_Types.Object.Equals
{
	[TestClass]
	public class DefaultObjectEquals
	{
		[TestMethod]
		public void EqualValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new C(1), new C(1))))
				.Should().Throw<AssertFailedException>()
				.And.Message.Should().Contain("Equals(object)");
		}

		private class C
		{
			public C(int x)
			{
				this.X = x;
			}

			public readonly int X;

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

			//	if (ReferenceEquals(this, obj)) return true;
			//	if (ReferenceEquals(null, obj)) return false;
			//{
			//public override bool Equals(object obj)

			// Oops!
			//	if (obj.GetType() != GetType()) return false;
			//	return Equals((C)obj);
			//}
		}
	}
}
