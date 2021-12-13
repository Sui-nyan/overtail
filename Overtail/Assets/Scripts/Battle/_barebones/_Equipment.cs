using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Entity;


namespace Overtail.Deprecated
{
    public class _Equipment : ScriptableObject
    {
        public Stats stats;
    }


    public class _Weapon : _Equipment
    {

    }

    public class _Armor : _Equipment
    {

    }

    public class _Headgear : _Equipment { }
}