using UnityEngine;
using System.Collections;

public class UnitStatus : MonoBehaviour
{
    public Unit owner;

    public int hp;
    [SerializeField]
    int currentHp;
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
