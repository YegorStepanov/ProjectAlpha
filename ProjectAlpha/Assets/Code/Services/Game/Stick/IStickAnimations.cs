﻿using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public interface IStickAnimations
{
    UniTask Rotate(Transform transform, Vector3 endRotation, float speed, float delay, CancellationToken token);
    UniTask StartIncreasing(Transform transform, float speed, CancellationToken token);
}