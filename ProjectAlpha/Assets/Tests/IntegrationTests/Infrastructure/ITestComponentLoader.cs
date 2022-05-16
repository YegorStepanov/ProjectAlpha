﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Tests;

public interface ITestComponentLoader<T>: IDisposable where T : Object
{
    UniTask<GameObject> Instantiate();
    UniTask<AsyncOperationHandle<GameObject>> InstantiateHandle();
}