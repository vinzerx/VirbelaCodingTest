using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirbelaTest
{
    public class Player : MovableObject
    {
        private void Awake()
        {
            currentPosition = transform.position;
            Manager.Instance.RegisterPlayer(this);
        }
    }
}
