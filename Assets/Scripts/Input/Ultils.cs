using UnityEngine;

public class Ultils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.WorldToScreenPoint(position);
    }
}
