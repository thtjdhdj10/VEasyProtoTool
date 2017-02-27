using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionVectorMoveUnit : Action
{
    public float speed;

    bool[] moveDir = new bool[4];

    Dictionary<GameManager.Direction, KeyManager.KeyCommand> dirKeyDic =
        new Dictionary<GameManager.Direction, KeyManager.KeyCommand>();
    Dictionary<GameManager.Direction, GameManager.Direction> dirRevdirDic =
        new Dictionary<GameManager.Direction, GameManager.Direction>();

    public ActionVectorMoveUnit()
    {
        dirKeyDic[GameManager.Direction.LEFT] = KeyManager.KeyCommand.MOVE_LEFT;
        dirKeyDic[GameManager.Direction.RIGHT] = KeyManager.KeyCommand.MOVE_RIGHT;
        dirKeyDic[GameManager.Direction.UP] = KeyManager.KeyCommand.MOVE_UP;
        dirKeyDic[GameManager.Direction.DOWN] = KeyManager.KeyCommand.MOVE_DOWN;

        dirRevdirDic[GameManager.Direction.LEFT] = GameManager.Direction.RIGHT;
        dirRevdirDic[GameManager.Direction.RIGHT] = GameManager.Direction.LEFT;
        dirRevdirDic[GameManager.Direction.UP] = GameManager.Direction.DOWN;
        dirRevdirDic[GameManager.Direction.DOWN] = GameManager.Direction.UP;
    }

    public override void Activate(Trigger trigger)
    {
        if (trigger.GetType() != typeof(TriggerForKeyInputs))
        {
            return;
        }

        TriggerForKeyInputs triggerKeyInputs = (TriggerForKeyInputs)trigger;

        UpdateMoveState(triggerKeyInputs.command, triggerKeyInputs.pressType);

        VectorMovable vm = triggerKeyInputs.target.GetComponent<VectorMovable>();

        if (vm == null)
        {
            vm = triggerKeyInputs.target.gameObject.AddComponent<VectorMovable>();
            vm.Init(speed, Movable.BounceType.NONE);
        }

        vm.speed = speed;
        vm.moveDir = moveDir;
    }

    void UpdateMoveState(KeyManager.KeyCommand command, KeyManager.KeyPressType type)
    {
        for (int d = 0; d < 4; ++d)
        {
            GameManager.Direction dir = (GameManager.Direction)d;
            if (command == dirKeyDic[dir])
            {
                if (type == KeyManager.KeyPressType.DOWN)
                {
                    moveDir[(int)dir] = true;

                    moveDir[(int)dirRevdirDic[dir]] = false;
                }
                else if (type == KeyManager.KeyPressType.PRESS)
                {
                    if (moveDir[(int)dirRevdirDic[dir]] == false)
                    {
                        moveDir[(int)dir] = true;
                    }
                }
                else if (type == KeyManager.KeyPressType.UP)
                {
                    moveDir[(int)dir] = false;
                }

                return;
            }
        }
    }
}
