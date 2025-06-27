using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HideText : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject targetObject;
    public InputField inputField;

    private void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (targetObject != null)
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                targetObject.SetActive(false);
            }
        }
    }
}
