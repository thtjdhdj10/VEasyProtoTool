using UnityEngine;

public class RefValue<T> where T : struct
{
    public RefValue(T _value){ value = _value; }
    public T value;
}

// 제네릭 클래스는 직렬화할 수 없음
// 직렬화를 위하여 파생 클래스 사용
[System.Serializable]
public class RefBoolean : RefValue<bool>
{
    public RefBoolean(bool _value)
        : base(_value) { }
}