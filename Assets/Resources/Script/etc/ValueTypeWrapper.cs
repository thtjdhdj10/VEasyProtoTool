using UnityEngine;

namespace VEPT
{
    public class ValueTypeWrapper<T> where T : struct
    {
        public ValueTypeWrapper(T _value) { value = _value; }
        public T value;
    }

    // 제네릭 클래스는 직렬화할 수 없음
    // 직렬화를 위하여 파생 클래스 사용
    [System.Serializable]
    public class BooleanWrapper : ValueTypeWrapper<bool>
    {
        public BooleanWrapper(bool _value)
            : base(_value) { }
    }

    [System.Serializable]
    public class IntegerWrapper : ValueTypeWrapper<int>
    {
        public IntegerWrapper(int _value)
            : base(_value) { }
    }

    [System.Serializable]
    public class FloatWrapper : ValueTypeWrapper<float>
    {
        public FloatWrapper(float _value)
            : base(_value) { }
    }
}