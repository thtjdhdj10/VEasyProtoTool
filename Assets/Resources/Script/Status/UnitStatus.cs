using UnityEngine;
using System.Collections;

public class UnitStatus : MonoBehaviour
{
    [System.NonSerialized]
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
        owner = gameObject.GetComponent<Unit>();
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
