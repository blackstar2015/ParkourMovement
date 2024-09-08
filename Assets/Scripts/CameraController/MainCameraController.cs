using Unity.Cinemachine;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public CinemachineCamera vcam;
    public float rotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Get the current Camera state
        var state = vcam.State;

        //Extract the roation quaternion from the state
        var rotation = state.GetFinalOrientation();

        //convert the rotation to Euler angles
        var euler = rotation.eulerAngles;

        //Get the y-axis value from the Euler angles
        rotationY = euler.y;

        //round the rotation y value to the nearest integer
        var roundedRotationY = Mathf.RoundToInt(rotationY);
    }

    public Quaternion flatRotation => Quaternion.Euler(0,rotationY,0);
}
