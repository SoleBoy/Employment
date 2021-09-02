using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public Sprite pickSprite;
    public Sprite norSprite;

    public Color pickColor;
    public Color norColor;

    private Text inputText;
    private Image spriteImage;
    private Button clickBtn;

    private void Awake()
    {
        inputText = transform.Find("Text").GetComponent<Text>();
        spriteImage = gameObject.GetComponent<Image>();
        clickBtn = gameObject.GetComponent<Button>();
    }
}
