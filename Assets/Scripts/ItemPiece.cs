using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 老虎机每一个格子
/// </summary>
public class ItemPiece : MonoBehaviour
{
    private Image Icon;
    public void Init(Item item)
    {
        Icon = GetComponent<Image>();
        Icon.sprite = item.Icon;
    }
}
