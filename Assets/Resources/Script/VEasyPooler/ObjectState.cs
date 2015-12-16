using UnityEngine;
using System.Collections;

public class ObjectState : MonoBehaviour
{

    bool isDying = false;
    float lifeTime = 0.0f;

    public void SetReleaseTimer(float time)
    {
        isDying = true;
        lifeTime = time;
    }

    public void AddReleaseTimer(float time)
    {
        lifeTime += time;
    }

    public void StopReleaseTimer()
    {
        isDying = false;
    }

    public void StartReleaseTimer()
    {
        isDying = true;
    }

    public float GetReleaseTime()
    {
        return lifeTime;
    }

    //

    //bool isMoving = false;
    //bool useLookAtDirection = false;

    //float speedDotPerFrame = 0.0f;

    //Vector3 direction = new Vector3();
    //Vector3 toPosition = new Vector3();

    //

    void Update()
    {
        if (isDying == true)
        {
            if (lifeTime > 0.0f)
            {
                lifeTime -= Time.deltaTime;
            }
            else
            {
                VEasyPoolerManager.ReleaseObjectRequest(gameObject);
            }
        }
    }

    //

    [System.NonSerialized]
    public int indexOfPool = -1;
    
    public string originalName = null;

    private bool isUse;
    public bool IsUse
    {
        get
        {
            return isUse;
        }
        set
        {
            gameObject.SetActive(value);

            MonoBehaviour[] c = gameObject.GetComponents<MonoBehaviour>();
            for(int i = 0; i < c.Length; ++i)
            {
                c[i].enabled = value;
            }

            isUse = value;
        }
    }


}
