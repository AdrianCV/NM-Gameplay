using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorWheelButtonController : MonoBehaviour
{
    public int id;
    private Animator anim;
    private bool selected = false;
    public GameObject colorSelected;
    private Image image;
    public Color thisColor;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        thisColor = GetComponent<Image>().color;
        image = colorSelected.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            image.color = thisColor;
        }
    }

    public void Selected()
    {
        selected = true;
        ColorWheelController.colorID = id;
        // Debug.Log(id);
    }

    public void DeSelected()
    {
        selected = false;
        ColorWheelController.colorID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
    }
}
