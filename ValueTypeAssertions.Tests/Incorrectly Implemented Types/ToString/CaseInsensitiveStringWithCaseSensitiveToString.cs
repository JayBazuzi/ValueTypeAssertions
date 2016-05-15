// Copyright (C) 2016 Jay Bazuzi - This software may be modified and distributed under the terms of the MIT license.  See the LICENSE.md file for details.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bazuzi.ValueTypeAssertions.Tests.Incorrectly_Implemented_Types.ToString
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
				this.Value = value;
			}

			public readonly string Value;

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals((C) obj);
			}

			public override int GetHashCode()
			{
				return this.Value != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Value) : 0;
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
				return this.Value;
			}

			protected bool Equals(C other)
			{
				return string.Equals(this.Value, other.Value, StringComparison.OrdinalIgnoreCase);
			}
		}
	}
}
