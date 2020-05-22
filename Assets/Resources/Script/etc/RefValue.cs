

public class RefValue<T> where T : struct
{
    public RefValue(T _value){ value = _value; }
    public T value;
}
