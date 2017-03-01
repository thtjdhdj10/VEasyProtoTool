using UnityEngine;
using System.Collections;

public class UnitStatus : MonoBehaviour
{
    public Unit owner;

    public float hp;
    [SerializeField]
    float currentHp;
    public float CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
            if (currentHp <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    void Awake()
    {
        if(owner == null)
        {
            owner = gameObject.GetComponent<Unit>();
        }

        owner.unitStatus = this;
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


}
