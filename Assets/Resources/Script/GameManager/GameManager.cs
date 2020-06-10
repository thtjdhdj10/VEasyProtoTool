using UnityEngine;

namespace VEPT
{
    public class GameManager : SingletonComponent<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();

            // vsync(수직동기화) 꺼야 정상 적용됨
            Application.targetFrameRate = 60;
        }

        private void FixedUpdate()
        {
            Operable.GetOperableList<Collidable>()?.ForEach(o => o.isCollisionInFrame = false);
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Mouse0))
        //    {
        //        GameObject shock = Instantiate(ResourcesManager<GameObject>.LoadResource(
        //            ResourcesManager<GameObject>.ResourceName.Effect_Shockwave));
        //        shock.transform.position = VEasyCalculator.ScreenToWorldPos(Input.mousePosition);
        //    }
        //}
    }
}