using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirbelaTest
{
    public class Bot : MovableObject
    {
        [SerializeField] private Renderer myRenderer;
        
        public void SetColor(Color newColor)
        {
            myRenderer.material.color = newColor;
        }
        
        void Awake()
        {
            Manager.Instance.RegisterBot(this);
        }
    }
}
