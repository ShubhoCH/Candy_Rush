using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    Vector2 startTouch, swipedelta;
    bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private void Start()
    {
        swipeDown = swipeLeft = swipeRight = swipeUp = false;
    }
    void Update()
    {
        #region Mobile Inputs
        if (Input.touchCount > 0)
        {
            //Get the Position of the touch whenever the player touches the screen:
            if (Input.touches[0].phase == TouchPhase.Began)
                startTouch = Input.touches[0].position;
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                Reset();

            // Activate and Place the Remote in the place of start Touch: 


            //Calculate Distance from Initial Touch:
            swipedelta = Vector2.zero;
            swipedelta = Input.touches[0].position - startTouch;

            //Update the button position per frame accordingly:

        }
        #endregion

        //Did we cross the Dead-Zone Circle:
        if (swipedelta.magnitude > 80f)
        {
            //Which direction?
            float x = swipedelta.x;
            float y = swipedelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or Right:
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                //Up or Down:
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }
        }
    }
    public void Reset()
    {
        swipeDown = swipeLeft = swipeRight = swipeUp = false;
        startTouch = swipedelta = Vector2.zero;
        // Disable Remote GameObject:

    }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

}
