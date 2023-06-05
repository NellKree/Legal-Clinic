using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeType : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_InputField.ContentType ContentTypeHide;
    public TMP_InputField.ContentType ContentTypeOpen;

    public Texture SpriteHide;
    public Texture SpriteOpen;
    public RawImage ButtonImage;

    public void SetContentType()
    {
        if (inputField.contentType == ContentTypeOpen)
        {
            inputField.contentType = ContentTypeHide;
            ButtonImage.texture = SpriteHide;
        }
        else
        {
            inputField.contentType = ContentTypeOpen;
            ButtonImage.texture = SpriteOpen;

        }
        inputField.ForceLabelUpdate();
    }
    public void HideContent()
    {
        inputField.contentType = ContentTypeHide;
        ButtonImage.texture = SpriteHide;
        inputField.ForceLabelUpdate();
    }
}
