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

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _hoverSound;
    [SerializeField] AudioClip _selectSound;

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
        _audioSource.PlayOneShot(_selectSound);
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
        _audioSource.PlayOneShot(_hoverSound);
        anim.SetBool("Hover", true);
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
    }
}
