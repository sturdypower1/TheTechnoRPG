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

    bool isInBattleCamera;

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
    private void Start()
    {
        overworldCamera.Follow = Technoblade.instance.transform;
        
    }

    public void ToBattleCamera()
    {
        if (!isInBattleCamera)
        {
            battleCamera.Priority = 11;
            battleCamera.transform.position = Camera.main.transform.position;
            animator.SetTrigger("OnBattleSetup");

            isInBattleCamera = true;
        }
    }

    public void ToOverworldCamera()
    {
        isInBattleCamera = false;
        battleCamera.Priority = 9;
        animator.SetTrigger("OnBattleEnd");
    }
}
