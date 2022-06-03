using System.ComponentModel;
using UnityEngine;

namespace Code.Services;

[CreateAssetMenu(menuName = "Data/Next Position Generator", fileName = "Next Position Generator")]
public sealed class NextPositionGenerator : ScriptableObject
{
    [SerializeField] private Mode _mode;

    [SerializeField] private float _leftOffset;
    [SerializeField] private float _rightOffset;
    
    public float NextPosition(IPlatform currentPlatform, IPlatform nextPlatform, float cameraDestinationRightCorner)
    {
        float halfWidth = nextPlatform.Borders.HalfWidth;

        return _mode switch
        {
            Mode.Leftmost => Leftmost(),
            Mode.Rightmost => Rightmost(),
            Mode.Between => Random.Range(Leftmost(), Rightmost()),
            _ => throw new InvalidEnumArgumentException(nameof(_mode), (int)_mode, typeof(Mode))
        };

        float Leftmost() => currentPlatform.Borders.Right + halfWidth + _leftOffset;
        float Rightmost() => cameraDestinationRightCorner - halfWidth - _rightOffset;
    }
    
    private enum Mode
    {
        Between,
        Leftmost,
        Rightmost,
    }
}