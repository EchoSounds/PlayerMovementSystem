using UnityEngine;

public class playerOrientation : MonoBehaviour
{
    [SerializeField]private GameObject targetObject;

    // Update is called once per frame
    void Update()
    {
        // Get the target object's y rotation
        Quaternion targetRotation = targetObject.transform.rotation;

        // Set the current object's y rotation to match the target object's y rotation
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles.y = targetRotation.eulerAngles.y;
        eulerAngles.z = targetRotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
}