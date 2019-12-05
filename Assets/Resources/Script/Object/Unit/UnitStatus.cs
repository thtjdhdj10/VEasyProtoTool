using UnityEngine;
using System.Collections;

public class UnitStatus : MonoBehaviour
{
    public Unit owner;

    public int hp;
    [SerializeField]
    private int currentHp;
    public int CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
            if (currentHp <= 0)
            {
                Destroy(gameObject);
            }

            vital = GetVitalSign();
            if(enableVitalColor)
            {
                sprite.color = GetVitalColor();
            }
        }
    }

    public bool enableVitalColor; // 남은체력에따라 컬러변경

    public VitalSign vital;
    public SpriteRenderer sprite;

    public enum VitalSign
    {
        RED, // 100%~80%
        ORANGE, // 80%~50%
        YELLOW, // 50%~20%
        WHITE, // 20%~0%
    }

    void Awake()
    {
        // TODO 에디터에서 찾게 수정

        if(owner == null) owner = gameObject.GetComponent<Unit>();
        owner.unitStatus = this;

        if (enableVitalColor && sprite == null)
            sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        CurrentHp = hp;
        if(CurrentHp <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public Color GetVitalColor()
    {
        Color ret = sprite.color;

        switch (vital)
        {
            case VitalSign.RED:
                ret.r = 1f;
                ret.g = 0f;
                ret.b = 0f;
                break;
            case VitalSign.ORANGE:
                ret.r = 1f;
                ret.g = 0.4f;
                ret.b = 0f;
                break;
            case VitalSign.YELLOW:
                ret.r = 1f;
                ret.g = 0.85f;
                ret.b = 0f;
                break;
            case VitalSign.WHITE:
                ret.r = 1f;
                ret.g = 1f;
                ret.b = 1f;
                break;
        }

        return ret;
    }

    public VitalSign GetVitalSign()
    {
        float remainHpPercentage = (float)currentHp / hp;
        if (remainHpPercentage > 0.8f)
            return VitalSign.RED;
        else if (remainHpPercentage > 0.5f)
            return VitalSign.ORANGE;
        else if (remainHpPercentage > 0.2f)
            return VitalSign.YELLOW;
        return VitalSign.WHITE;
    }


}
