using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISaveable : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    public virtual void Start()
    {
        SaveAndLoadManager.instance.OnStartSave += Save;
        Load();
    }
    public virtual void OnDestroy()
    {
        SaveAndLoadManager.instance.OnStartSave -= Save;
    }

    public abstract void Save(int saveFileNumber);
    public abstract void Load();
}
