using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISaveable : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    public virtual void Start()
    {
        FileSaveManager.instance.OnStartSave += Save;
        Load();
    }
    public virtual void OnDestroy()
    {
        FileSaveManager.instance.OnStartSave -= Save;
    }

    public abstract void Save(int saveFileNumber);
    public abstract void Load();
}
