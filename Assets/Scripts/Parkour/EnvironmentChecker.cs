using UnityEngine;

public class EnvironmentChecker : MonoBehaviour
{
    public Vector3 RayOffset = new Vector3(0, .2f, 0);
    public float RayLength = .9f;
    public float HeightRayLength = 6f;
    public LayerMask ObstacleLayer;

    public ObstacleInfo CheckObstacle()
    {
        ObstacleInfo hitData = new ObstacleInfo();
        Vector3 rayOrigin = transform.position + RayOffset;
        hitData.HitFound = Physics.Raycast(rayOrigin, transform.forward,out hitData.HitInfo,RayLength,ObstacleLayer);

        Debug.DrawRay(rayOrigin,transform.forward *  RayLength, (hitData.HitFound) ? Color.red : Color.green);
        
        if(hitData.HitFound)
        {
            Vector3 heightOrigin = hitData.HitInfo.point + Vector3.up * HeightRayLength;
            hitData.HeightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.HeightInfo, HeightRayLength, ObstacleLayer);

            Debug.DrawRay(heightOrigin, Vector3.down * HeightRayLength, (hitData.HeightHitFound) ? Color.blue : Color.green);

        }

        return hitData;
    }
}

public struct ObstacleInfo
{
    public bool HitFound;
    public bool HeightHitFound;
    public RaycastHit HitInfo;
    public RaycastHit HeightInfo;
}
