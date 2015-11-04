using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // 각 플레이어가 다른 모든 플레이어와의 동맹 여부를 가지고 있도록

    public enum PlayerType
    {
        NONE = 0,

        NEUTRAL_PLAYER = 1 + 16 * 0,
        NEUTRAL_AI = 2 + 16 * 0,

        ME = 0 + 16*1,
        ALLY_PLAYER = 1 + 16*1,
        ALLY_AI = 2 + 16*1,

        ENEMY_PLAYER = 1 + 16*2,
        ENEMY_AI = 2 + 16*2,
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
