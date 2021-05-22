using System;
using UnityEngine;

namespace ShadowWithNoPast.GameProcess
{
    [RequireComponent(typeof(Animator))]
    public class TransitionObject : MonoBehaviour
    {
        private Animator animator;
        private Action whenOver;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void FadeOut(Action whenOver)
        {
            animator.SetTrigger("ExitScene");
            animator.fireEvents = true;
            this.whenOver = whenOver;
        }

        public void FadeIsOver()
        {
            if(whenOver != null)
            {
                whenOver.Invoke();
                whenOver = null;
            }
        }
    }
}