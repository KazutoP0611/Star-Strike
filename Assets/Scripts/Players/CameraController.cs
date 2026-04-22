using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string boostParameter;

    public void CameraToBoostPosition() => anim.SetBool(boostParameter, true);

    public void CameraToNormalPosition() => anim.SetBool(boostParameter, false);
}
