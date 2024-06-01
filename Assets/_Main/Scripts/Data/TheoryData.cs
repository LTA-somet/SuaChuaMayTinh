using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/Theory", fileName = "Theory")]
public class TheoryData : ScriptableObject
{
    public List<Muc3Data> Muc3Datas;
    [ContextMenu("Update Order")]
    void UpdateOrder()
    {
        for(int i = 0; i < Muc3Datas.Count; i++) 
        {
            var muc3 = Muc3Datas[i];
            for(int j = 0; j < muc3.ChildrenContent.Count; j++)
            {
                var muc3Children = muc3.ChildrenContent[j];
                muc3Children.Order = j;
                for (int k = 0; k < muc3Children.ChildrenContent.Count; k++)
                {
                    var muc4 = muc3Children.ChildrenContent[k];
                    muc4.Order = k;
                    for (int h = 0; h < muc3Children.ChildrenContent.Count; h++)
                    {
                        var muc5 = muc4.ChildrenData[h];
                        muc5.Order = h;
                    }
                }
            }
            muc3.Order = i;
        }
    }
}

[System.Serializable]
public class Muc3Data
{
    public string uID;
    [Multiline] public string Title;
    public List<Muc3ChildData> ChildrenContent;
    public int Order;
}
[System.Serializable]
public class Muc3ChildData
{
    public string uID;
    [Multiline] public string Title;
    public string DemoMediaPath;
    public List<Muc4Data> ChildrenContent = new List<Muc4Data>();
    public int Order;
    public void Destroy()
    {
        StreamingAssetHelper.DeleteFile(DemoMediaPath);
        for (int i = 0; i < ChildrenContent.Count; i++)
        {
            ChildrenContent[i].Destroy();
        }
    }
}
[System.Serializable]
public class Muc4Data
{
    public string uID;
    [Multiline] public string Title;
    public List<ContentData> ChildrenData = new List<ContentData>();
    public int Order;
    public void Destroy()
    {
        if(ChildrenData != null)
        {
            for (int i = 0; i < ChildrenData.Count; i++)
            {
                ChildrenData[i].Destroy();
            }
        }    
    }
    public string GetTitleWithRomanIndex()
    {
        return NumbersToRoman.IntToRoman(Order+1) + ". " + Title;
    }
}

public enum ContentType : byte
{
    Image = 0,
    Video = 1,
    Animation = 2,
}
[System.Serializable]
public class ContentData
{
    public string uID;
    [Multiline] public string Title;
    [Multiline] public string Content;
    public string MediaPath;
    public int Order;
    public List<SlideData> ChildrenContent;

    public void Destroy()
    {
        StreamingAssetHelper.DeleteFile(MediaPath);
        for (int i = 0; i < ChildrenContent.Count; i++)
        {
            ChildrenContent[i].Destroy();
        }
    }
}
[System.Serializable]
public class SlideData
{
    public string uID;
    [Multiline] public string Title;
    [Multiline] public string Content;
    public string MediaPath;
    public ContentType Type;
    public int Order;
    public void Destroy()
    {
        StreamingAssetHelper.DeleteFile(MediaPath);
    }
}