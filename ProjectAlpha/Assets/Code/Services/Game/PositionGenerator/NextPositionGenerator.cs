using System.ComponentModel;
using UnityEngine;

namespace Code.Services;

public abstract class PositionGenerator : ScriptableObject
{
    [SerializeField] private Mode _mode;

    [SerializeField] private float _leftOffset;
    [SerializeField] private float _rightOffset;

    public float NextPosition(float minPosX, float maxPosX, float width)
    {
        float halfWidth = width / 2f;

        return _mode switch
        {
            Mode.Leftmost => Leftmost(),
            Mode.Rightmost => Rightmost(),
            Mode.Between => Random.Range(Leftmost(), Rightmost()),
            _ => throw new InvalidEnumArgumentException(nameof(_mode), (int)_mode, typeof(Mode))
        };


        float Leftmost() => minPosX + halfWidth + _leftOffset;
        float Rightmost() => maxPosX - halfWidth - _rightOffset;
    }

    private enum Mode
    {
        Between,
        Leftmost,
        Rightmost,
    }
}