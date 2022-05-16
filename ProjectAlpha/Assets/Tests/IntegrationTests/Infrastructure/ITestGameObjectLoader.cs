using UnityEngine;

namespace Tests;

public interface ITestGameObjectLoader<T> : ITestComponentLoader<T>, ITestAssetLoader<T> where T : Object { }