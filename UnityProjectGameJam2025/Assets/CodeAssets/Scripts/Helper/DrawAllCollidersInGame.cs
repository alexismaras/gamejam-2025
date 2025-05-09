using UnityEngine;

[ExecuteAlways] // Works in both Play Mode and Edit Mode
public class DrawAllCollidersInGame : MonoBehaviour
{
    public bool showColliders = true;
    public Color triggerColor = Color.green;
    public Color solidColor = Color.red;

    void OnDrawGizmos()
    {
        if (!showColliders) return;

        Collider[] allColliders = FindObjectsOfType<Collider>();
        foreach (Collider col in allColliders)
        {
            if (!col.enabled || !col.gameObject.activeInHierarchy) continue;

            Gizmos.color = col.isTrigger ? triggerColor : solidColor;
            DrawCollider(col);
        }
    }

    private void DrawCollider(Collider col)
    {
        switch (col)
        {
            case BoxCollider box:
                Gizmos.matrix = col.transform.localToWorldMatrix;
                Gizmos.DrawWireCube(box.center, box.size);
                break;

            case SphereCollider sphere:
                Vector3 sphereWorldPos = col.transform.TransformPoint(sphere.center);
                Gizmos.DrawWireSphere(sphereWorldPos, sphere.radius);
                break;

            case CapsuleCollider capsule:
                DrawWireCapsule(
                    position: col.transform.TransformPoint(capsule.center),
                    height: capsule.height,
                    radius: capsule.radius,
                    direction: capsule.direction,
                    localToWorld: col.transform.localToWorldMatrix
                );
                break;

            case MeshCollider mesh:
                if (mesh.sharedMesh != null)
                    Gizmos.DrawWireMesh(mesh.sharedMesh, col.transform.position, col.transform.rotation, col.transform.lossyScale);
                break;
        }
    }

    // Capsule drawing implementation
    private void DrawWireCapsule(Vector3 position, float height, float radius, int direction, Matrix4x4 localToWorld)
    {
        Vector3 up = Vector3.up;
        switch (direction)
        {
            case 0: up = Vector3.right; break; // X-axis
            case 1: up = Vector3.up; break;    // Y-axis
            case 2: up = Vector3.forward; break; // Z-axis
        }

        Vector3 center = position;
        Vector3 top = center + up * (height * 0.5f - radius);
        Vector3 bottom = center - up * (height * 0.5f - radius);

        // Draw spheres (top and bottom hemispheres)
        Gizmos.DrawWireSphere(top, radius);
        Gizmos.DrawWireSphere(bottom, radius);

        // Draw connecting cylinders
        Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
        Vector3 right = Vector3.Cross(up, forward).normalized * radius;

        for (int i = 0; i < 4; i++)
        {
            Vector3 sideDir = Quaternion.AngleAxis(i * 90, up) * forward;
            Gizmos.DrawLine(top + sideDir * radius, bottom + sideDir * radius);
        }
    }
}