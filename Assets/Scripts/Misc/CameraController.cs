using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Animator animator;

    public CinemachineVirtualCamera overworldCamera;
    public CinemachineVirtualCamera battleCamera;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ToBattleCamera()
    {
        battleCamera.transform.position = overworldCamera.transform.position;
        animator.SetTrigger("OnBattleSetup");
    }
}
