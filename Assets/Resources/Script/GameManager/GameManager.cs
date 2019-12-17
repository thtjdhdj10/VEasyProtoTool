using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private ResourcesManager<GameObject> prefabmanager;
    private ResourcesManager<Sprite> prefabManager;
    private ResourcesManager<RuntimeAnimatorController> controllerManager;

    // 순서를 바꾸지 말 것.
    // index 로 사용할 수 있도록 NONE 을 맨 뒤로 했음
    public enum Direction
    {
        LEFT = 0,
        RIGHT,
        UP,
        DOWN,

        FRONT,
        BACK,

        NONE,
    }

    private void Awake()
    {
        gm = this;

        prefabmanager = new ResourcesManager<GameObject>();
        prefabManager = new ResourcesManager<Sprite>();
        controllerManager = new ResourcesManager<RuntimeAnimatorController>();

        pivotType = PivotType.RANDOM;
        List<int> numberList = new List<int>() { 2, 6, 4, 7, 12, 4, 7, 8, 23, 425, 23, 78, 8, 3, 2, 76 };
        QuickRecursion(0, numberList.Count - 1, ref numberList);
        Debug.Log(numberList);
    }

    private void QuickRecursion(int left, int right, ref List<int> numbers)
    {
        if (left >= right) return;

        int pivot = QuickFunc(left, right, ref numbers);

        QuickRecursion(left, pivot - 1, ref numbers);
        QuickRecursion(pivot + 1, right, ref numbers);
    }

    private int QuickFunc(int left, int right, ref List<int> numbers)
    {
        int pivot = left;
        switch (pivotType)
        {
            case PivotType.RANDOM:
                pivot = Random.Range(left, right + 1);
                break;
        }

        int j = left;
        for (int i = j + 1; i <= right; ++i)
        {
            if (j < i && i < pivot)
            {
                if (j == pivot) ++j;
                Swap(i, j, ref numbers);
            }
        }

        Swap(pivot, j, ref numbers);
        pivot = j;

        return pivot;
    }

    private void Swap(int a, int b, ref List<int> numbers)
    {
        int t = numbers[a];
        numbers[a] = numbers[b];
        numbers[b] = numbers[t];
    }

    private PivotType pivotType;

    enum PivotType
    {
        RANDOM,
        FIRST,
        CENTER,
        LAST,
    }



    private void FixedUpdate()
    {
        foreach(var o in Operable.GetOperableList<Collidable>())
        {
            Collidable col = o as Collidable;
            if (col != null) col.isCollisionInFrame = false;
        }
    }


    private void Start()
    {
        //TriggerForUnits tu = gameObject.AddComponent<TriggerForUnits>();
        //tu.type = TriggerForUnits.Type.CREATE_UNIT;
        //tu.target = typeof(Unit);

        //ActionCreateObject action = new ActionCreateObject();
        
        //tu.actionList.Add()

        //TriggerKeyInput tk = gameObject.AddComponent<TriggerKeyInput>();
        //tk.Init(false, false, true,
        //    this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        //ActionLog al = new ActionLog();
        //al.text = "ATTACK!";
        //tk.actionList.Add(al);

        //if(Controlable.controlableList.Count > 1)
        //{
        //    TriggerKeyInputs tks = gameObject.AddComponent<TriggerKeyInputs>();
        //    tks.Init(false, false, true,
        //        Controlable.controlableList[1].owner);

        //    ActionVectorMoveUnit move = new ActionVectorMoveUnit();
        //    move.speed = 2f;
        //    tks.actionList.Add(move);
        //}

        //for(int i = 0; i < 20; ++i)
        //{
        //    int[] ii = new int[3];
        //    ii[0] = 2;
        //    ii[1] = 10;
        //    ii[2] = 4;
        //    Debug.Log(CustomRandGen.Rand(ii));
        //}


    }

}
