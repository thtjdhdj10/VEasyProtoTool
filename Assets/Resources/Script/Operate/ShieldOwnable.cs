using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOwnable : Operable
{
    public bool enableShield = true;

    public int maxShieldCount = 1;
    public int ShieldCount
    {
        get
        {
            return shieldCount;
        }
        set
        {
            shieldCount = value;
            if (shieldCount > 0) enableShield = true;
            else enableShield = false;
        }
    }
    [SerializeField]
    private int shieldCount = 1;

    public float shieldRegenDelay;
    private float regenProgress;

    private List<GameObject> shieldList = new List<GameObject>();

    //

    protected override void Awake()
    {
        base.Awake();

        enableShield = false;

        for (int i = 0; i < ShieldCount; ++i)
        {
            GameObject shield = Instantiate(ResourcesManager<GameObject>.LoadResource(
                ResourcesManager<GameObject>.ResourceName.Effect_Shield));
            shield.transform.position = transform.position;
            shield.transform.parent = transform;
            shieldList.Add(shield);

            enableShield = true;
        }
    }

    private void FixedUpdate()
    {
        if(shieldCount < maxShieldCount)
        {
            if(regenProgress < shieldRegenDelay)
            {
                regenProgress += Time.fixedDeltaTime;
            }
            else
            {
                regenProgress = 0f;
                ++shieldCount;

                GameObject shield = Instantiate(ResourcesManager<GameObject>.LoadResource(
                    ResourcesManager<GameObject>.ResourceName.Effect_Shield));
                shield.transform.position = transform.position;
                shield.transform.parent = transform;
                shieldList.Add(shield);
            }
        }
    }

    public bool ShieldBreak()
    {
        if (shieldCount > 0)
        {
            --shieldCount;

            GameObject shield = shieldList[shieldList.Count - 1];
            shieldList.Remove(shield);
            Destroy(shield);

            return true;
        }
        else return false;
    }
}
