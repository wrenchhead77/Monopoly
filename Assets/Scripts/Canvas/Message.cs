using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]

public class Popup
{
    public GameObject sender;
    public string confirmAction; // Name of the method for the confirm button
    public string cancelAction;  // Name of the method for the cancel button
    public string title;
    public string message;
    public string confirmText;
    public string cancelText;

    public Popup(GameObject _sender, string _confirmAction, string _cancelAction, string _title, string _message, string _confirmText, string _cancelText)
    {
        sender = _sender;
        confirmAction = _confirmAction;
        cancelAction = _cancelAction;
        title = _title;
        message = _message;
        confirmText = _confirmText;
        cancelText = _cancelText;
    }
}
public class Message : MonoBehaviour
{
    [SerializeField] private Popup popup;
    [SerializeField] private TMP_Text title;
    [SerializeField] private Image property;
    [SerializeField] private TMP_Text message;
    [SerializeField] private TMP_Text confirm;
    [SerializeField] private TMP_Text cancel;
    [SerializeField] private TMP_InputField inputField; // Add this field to hold the InputField reference
    [SerializeField] private GameObject confirmObj;
    [SerializeField] private GameObject cancelObj;

    public void InitCanvas(Popup _popup, Sprite propertyImage = null)
    {
        popup = _popup;

        // Set the UI text fields
        title.text = popup.title;
        message.text = popup.message;
        confirm.text = popup.confirmText;
        cancel.text = popup.cancelText;

        // Show or hide confirm/cancel buttons as needed
        confirmObj.SetActive(!string.IsNullOrEmpty(popup.confirmText));
        cancelObj.SetActive(!string.IsNullOrEmpty(popup.cancelText));
        // If a property image is provided, set it; otherwise hide the image
        if (propertyImage != null)
        {
            property.sprite = propertyImage;
            property.gameObject.SetActive(true);
        }
        else
        {
            property.gameObject.SetActive(false);
        }
    }

    public void OnConfirmPressed()
    {
        string playerName = inputField != null ? inputField.text : popup.confirmText;
        popup.sender.SendMessage(popup.confirmAction, playerName, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
    }
    public void OnCancelPressed()
    {
        popup.sender.SendMessage(popup.cancelAction, popup.cancelText, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
    }
}
