using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private const float YMin = -50.0f;
    private const float YMax = 50.0f;

    public Transform lookAt;
    public Transform Player;

    public float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensitivity = 100.0f;

    // Update is called once per frame
    void LateUpdate()
    {
        // Update camera rotation
        currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 direction = new Vector3(0.4f, 1.8f, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * direction;

        transform.LookAt(lookAt.position);

        // Update player rotation to match camera's horizontal rotation
        Vector3 playerRotation = new Vector3(0, currentX, 0); // Only apply horizontal rotation
        Player.rotation = Quaternion.Euler(playerRotation);
    }
}
