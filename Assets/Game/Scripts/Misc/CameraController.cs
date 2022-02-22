using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Animator animator;

    [SerializeField]private CinemachineVirtualCamera followCamera;
    [SerializeField]private CinemachineVirtualCamera stillCamera;

    private bool isInStillCamera;

    
    public void SwitchToStillCamera()
    {
        if (!isInStillCamera)
        {
            stillCamera.Priority = 11;
            stillCamera.transform.position = Camera.main.transform.position;
            animator.SetTrigger("OnBattleSetup");

            isInStillCamera = true;
        }
    }

    public void SwitchToFollowCamera()
    {
        isInStillCamera = false;
        stillCamera.Priority = 9;
        animator.SetTrigger("OnBattleEnd");
    }
    private void Awake()
    {
        if (instance == null)
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
        followCamera.Follow = Technoblade.instance.transform;

    }

}
