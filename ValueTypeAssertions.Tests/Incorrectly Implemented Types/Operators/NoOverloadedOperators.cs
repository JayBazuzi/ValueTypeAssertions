﻿// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Incorrectly_Implemented_Types.Operators
{
	[TestClass]
	public class NoOverloadedOperators
	{
		[TestMethod]
		public void NoEqualityOperatorFound()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new AClass(1), new AClass(1))))
				.ShouldThrow<Exception>()
				.And.Message.Should().Contain("No op_Equality found.");
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
				if (obj.GetType() != GetType()) return false;
				return this.X == ((AClass) obj).X;
			}

			public override int GetHashCode()
			{
				return this.X;
			}
		}
	}
}
