using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 老虎机每一列
/// </summary>
public class Line : MonoBehaviour
{
    public Transform Root;
    public GameObject PiecePrefab;
    private bool IsStartAni;
    private ItemPiece EndPiece;
    private Action Callback;

    /// <summary>
    /// 初始化一栏
    /// </summary>
    /// <param name="item"></param>
    /// <param name="Count"></param>
    /// <param name="Callback"></param>
    public void Create(Item item, int Count, Action Callback = null)
    {
        Root.transform.position = transform.position;
        this.Callback = Callback;
        for (int i = 0; i < Root.childCount; i++)
        {
            Destroy(Root.GetChild(i).gameObject);
        }
        for (int i = 0; i < Count; i++)
        {
            Instantiate(PiecePrefab, Root).GetComponent<ItemPiece>().Init(Manager.Instance.RandomGetItem());
        }
        EndPiece = Instantiate(PiecePrefab, Root).GetComponent<ItemPiece>();
        EndPiece.Init(item);
        StartAni();
    }

    /// <summary>
    /// 开始动画
    /// </summary>
    public void StartAni()
    {
        IsStartAni = true;
    }

    /// <summary>
    /// 帧更新，开始后就会往下移动
    /// </summary>
    private void Update()
    {
        if (IsStartAni)
        {
            Root.transform.Translate(Vector3.down * Time.deltaTime * 800, Space.World);
            if (Vector2.Distance(transform.position, EndPiece.transform.position) <= 2f)
            {
                IsStartAni = false;
                EndPiece.transform.position = transform.position;
                Callback?.Invoke();
            }
        }
    }

}

