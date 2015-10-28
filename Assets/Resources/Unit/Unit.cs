using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    //

    public int ownerNumber = -1;

    enum RelationToPlayer
    {
        NONE = 0,
        MY,
        ALLY_PLAYER,
        ALLY_AI,
        NEUTRAL_PLAYER,
        NEUTRAL_AI,
        ENEMY_PLAYER,
        ENEMY_AI,
    }

    //

    public class UnitAbility<T>
        where T : struct
    {
        public T attackPoint;
        public T defencePoint;

        public T maxHp;
        public T currentHp;

        public T maxMp;
        public T currentMp;

    }

    public class UnitSubAbility<T>
        where T : struct
    {
        public T attackRange;
        public T searchRange;
        public T moveSpeed;
    }

    public class LogicalPosition<T>
        where T : struct
    {
        
    }

    //

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
