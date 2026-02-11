using H_Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElement : MonoBehaviour
{
    public int ID;

    [SerializeField] GameObject _frameLock;

    public void Init(int id)
    {
        ID = id;
        CheckUnlock();
    }

    private void OnEnable()
    {
        CheckUnlock();
    }

    void CheckUnlock()
    {
        if (GameDatas.LevelUnlock >= ID)
            Unlock();
        else
            Lock();
    }

    void Unlock()
    {
        _frameLock.SetActive(false);    
    }

    void Lock()
    {
       _frameLock.SetActive(true);
    }
}
