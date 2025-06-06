using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public RectTransform interactionTooltip;

    private void Awake()
    {
        SetInteractionTooltip(false);
    }

    public void SetInteractionTooltip(bool active) => interactionTooltip.gameObject.SetActive(active);
}
