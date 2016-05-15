﻿// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueTypeAssertions.Tests
{
	[TestClass]
	public class CaseInsensitiveStringWithCaseSensitiveToString
	{
		[TestMethod]
		public void EqualValues()
		{
			((Action) (() => ValueTypeAssertions.HasValueEquality(new C("foo"), new C("FOO"))))
				.ShouldThrow<AssertFailedException>()
				.And.Message.Should().Contain("ToString()");
		}

		private class C 
		{
			public C(string value)
			{
				Value = value;
			}

			public readonly string Value;

			protected bool Equals(C other)
			{
				return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != this.GetType()) return false;
				return Equals((C) obj);
			}

			public override int GetHashCode()
			{
				return (Value != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Value) : 0);
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
				return Value;
			}
		}
	}
}