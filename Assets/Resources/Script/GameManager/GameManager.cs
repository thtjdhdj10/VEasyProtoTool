using UnityEngine;

namespace VEPT
{
    public class GameManager : SingletonComponent<GameManager>
    {
        public bool isQuitting = false;

        private void FixedUpdate()
        {
            Operable.GetOperableList<Collidable>()?.ForEach(o => o.isCollisionInFrame = false);
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
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