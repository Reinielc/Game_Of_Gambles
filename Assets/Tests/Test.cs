using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test
{
    [Test]
    public void TestSimplePasses()
    {
        string str = "aaaaaaaaaaaaaa";
        int result = GameUtils.GetTextLength(str);
        Assert.AreEqual(2, result);
    }
}