using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueTypeAssertions.Tests
{
    [TestClass]
    public class CaseInsensitiveStringWithCaseSensitiveGetHashCode
    {
        [TestMethod]
        public void Test()
        {
            ((Action) (() => ValueTypeAssertions.HasValueEquality(new C("foo"), new C("FOO"))))
                .ShouldThrow<AssertFailedException>()
                .And.Message.Should().Contain("GetHashCode");
        }

        private class C : IEquatable<C>
        {
            public readonly string Value;

            public C(string value)
            {
                Value = value;
            }

            public bool Equals(C other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((C) obj);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }

            public static bool operator ==(C left, C right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(C left, C right)
            {
                return !Equals(left, right);
            }
        }
    }
}