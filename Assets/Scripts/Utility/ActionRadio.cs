using System;
using UnityEngine;

namespace MiniJam167.Utility
{
    [CreateAssetMenu(fileName = "Rd_Action", menuName = "MiniJam167/Radio/Action")]
    public class ActionRadio : ScriptableObject
    {
        public Action Callback;

    }
}
