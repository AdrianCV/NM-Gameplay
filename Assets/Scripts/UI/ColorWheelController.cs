using UnityEngine;
using UnityEngine.UI;

public class ColorWheelController : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _player;
    public Animator anim;
    public bool colorWheelSelected = false;
    public static int colorID;

    [SerializeField] Material _material1;
    [SerializeField] Material _material2;
    [SerializeField] Material _material3;

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
            _player.WheelIsOpen = true;
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
            _player.WheelIsOpen = false;
        }

        switch (colorID)
        {
            case 0: //nothing is selected
                break;
            case 1: //Blue
                //Call Blue
                _player.CurrentMaterial = _material1;
                break;
            case 2: //Green
                // Call green
                _player.CurrentMaterial = _material2;
                break;
            case 3: //Red
                // Call red
                _player.CurrentMaterial = _material3;
                break;
        }
    }
}
