using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameUtils
{
    public static int GetTextLength(string str)
    {
        int len = 0;
        for (int i = 0; i < str.Length; i++)
        {
            byte[] byte_len = Encoding.UTF8.GetBytes(str.Substring(i, 1));
            if (byte_len.Length > 1)
                len += 2;
            else
                len += 1;
        }
        return len;
    }
}