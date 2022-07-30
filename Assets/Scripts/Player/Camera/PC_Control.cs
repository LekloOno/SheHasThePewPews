using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Control : MonoBehaviour
{
    public float X_Sens;
    public float Y_Sens;

    [SerializeField] Transform cameraPosition;
    [SerializeField] Transform playerDir;
    [SerializeField] PIS_Combat inputHandler;
    [SerializeField] float globalSensAdjustment;

    float xRotation;
    float yRotation;

    void Update()
    {
        OnLook();
        transform.position = cameraPosition.position;
        transform.rotation = cameraPosition.rotation;
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook()
    {
        float lookX = inputHandler.playerInputActions.Arena.Look.ReadValue<Vector2>().x*globalSensAdjustment*X_Sens;
        float lookY = inputHandler.playerInputActions.Arena.Look.ReadValue<Vector2>().y*globalSensAdjustment*Y_Sens;

        xRotation = Mathf.Clamp(xRotation-lookY, -90f, 90f);

        yRotation += lookX;

        cameraPosition.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerDir.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
