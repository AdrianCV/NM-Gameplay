using UnityEngine;
using UnityEngine.UI;

public class ColorWheelController : MonoBehaviour
{
    public Animator anim;
    public bool colorWheelSelected = false;
    public static int colorID;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            colorWheelSelected = !colorWheelSelected;
        }

        if (colorWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
        }

        switch (colorID)
        {
            case 0: //nothing is selected
                break;
            case 1: //Red
                //Call red
                break;
            case 2: //Blue
                // Call blue
                break;
            case 3: //Green
                // Call green
                break;
        }
    }
}
