using UnityEngine;

[CreateAssetMenu(menuName = "Parkour Menu/Create New Parkour Action")]
public class NewParkourAction : ScriptableObject
{
    [SerializeField] private string _animationName;
    [SerializeField] private float _minimumHeight;
    [SerializeField] private float _maximumHeight;

    public string AnimationName => _animationName;

    public bool CheckIfAvailable(ObstacleInfo hitInfo,Transform player)
    {
        float checkHeight = hitInfo.HeightInfo.point.y - player.position.y;
        if(checkHeight < _minimumHeight || checkHeight > _maximumHeight) return false;
        else return true;
    }
}
