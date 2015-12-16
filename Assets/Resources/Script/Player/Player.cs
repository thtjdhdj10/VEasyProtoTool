using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // 각 플레이어가 다른 모든 플레이어와의 동맹 여부를 가지고 있도록

    public int playerNumber = 0;

    // Dictionary<int,PlayerType>

    public enum PlayerType
    {
        NONE = 0,

        NEUTRAL_PLAYER = 1,
        NEUTRAL_AI = 2,

        ME = 0 + 16,
        ALLY_PLAYER = 1 + 16,
        ALLY_AI = 2 + 16,

        ENEMY_PLAYER = 1 + 32,
        ENEMY_AI = 2 + 32,
    }

    public enum Relations
    {
        NONE = 0,
        ALLY = 1,
        ENEMY = 2,
    }

    public static Relations TypeToRelations(PlayerType target)
    {
        return (Relations)((int)target / 16);
    }
}
