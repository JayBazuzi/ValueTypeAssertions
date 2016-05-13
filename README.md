# ValueTypeAssertions

By "value type", I mean "a type that represents a value in some domain."

To correctly implement this in .Net is tricky. There's `object.Equals()` and `operator ==` and `GetHashCode()` and more. 

These are assertions that ensure that you have checked all the checkboxes in implementing equality. You can use it like this:

```
class NtfsPath
{
  // implementation here
}

[Test]
public void NtfsPathHasValueEquality()
{
  ValueTypeAssertions.HasValueEquality(new NtfsPath("foo.txt"), new NtfsPath("foo.txt"));
  ValueTypeAssertions.HasValueEquality(new NtfsPath("foo.txt"), new NtfsPath("bar.txt"));
}

[Test]
public void NtfsPathIsCaseInsensitive()
{
  ValueTypeAssertions.HasValueEquality(new NtfsPath("foo.txt"), new NtfsPath("FOO.TXT"));
}
```
