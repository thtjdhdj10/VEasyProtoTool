using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class Unit : Actor
    {
        public static List<Unit> unitList = new List<Unit>();

        [System.NonSerialized]
        public UnitStatus unitStatus;

        //

        protected override void OnEnable()
        {
            base.OnEnable();

            unitList.Add(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            unitList.Remove(this);
        }
    }
}