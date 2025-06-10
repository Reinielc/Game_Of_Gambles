using UnityEngine;
using UnityEngine.UI;

public class ItemPiece : MonoBehaviour
{
    private Image Icon;

    public void Init(Item item)
    {
        if (item != null && item.Icon != null)
        {
            Icon = GetComponent<Image>();
            Icon.sprite = item.Icon;
        }
        else
        {
            Debug.LogError("Item or Item Icon is null, can't set icon!");
            // 可选择设置一个默认图标
            // Icon.sprite = defaultIconSprite;
        }
    }
}
