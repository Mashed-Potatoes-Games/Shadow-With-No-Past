using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingMenu : MonoBehaviour
{
    private float defaultY;
    private float slideY = 10f;
    private LinearAnimation runningAnimation;

    public void Start()
    {
        defaultY = transform.position.y;
        var pos = transform.position;
        pos.y += slideY;
        transform.position = pos;
        SlideDown(2);
    }
    public void Update()
    {
        if(runningAnimation != null && runningAnimation.ContinueAnimation())
        {
            runningAnimation = null;
        }
    }

    public IEnumerator WaitBeforeStart(float waitSeconds, float slideSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        SlideDown(slideSeconds);
    }

    public void SlideDown(float duration)
    {
        var endPos = transform.position;
        endPos.y = defaultY;
        runningAnimation = new LinearAnimation(gameObject, duration, endPos);
    }

    public void SlideUp(float duration)
    {
        var endPos = transform.position;
        endPos.y = defaultY + slideY;
        runningAnimation = new LinearAnimation(gameObject, duration, endPos);
    }
}
