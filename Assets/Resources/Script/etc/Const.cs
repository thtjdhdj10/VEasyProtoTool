namespace VEPT
{
    public enum EResourceName
    {
        NONE = 0,

        // prefab

        Bullet_Laser,
        Bullet_Slayer_1,
        Bullet_Slayer_2,
        Bullet_Straight_1,
        Bullet_Straight_2,

        Effect_Bullet,
        Effect_LaserBody,
        Effect_LaserRoot,
        Effect_Shield,
        Effect_ShieldBreak,
        Effect_Shockwave,

        Enemy_Wing,
        EnemyBoss_Slayer,

        Player,

        // sprite

        Player_Damaged_strip5,

        // controller

        Effect_Laser_Column_Controller,
        Effect_Laser_Root__Controller,

        EnemyBoss_Slayer__Controller,

        Player_Damaged_Controller,
    }

    // 순서를 바꾸지 말 것.
    // index 로 사용할 수 있도록 NONE 을 맨 뒤로 했음
    public enum EDirection
    {
        LEFT = 0,
        RIGHT,
        UP,
        DOWN,

        FRONT,
        BACK,

        NONE,
    }
}