using UnityEngine;

namespace Code.IntegrationTests;

public interface ITestGameObjectLoader<T> : ITestComponentLoader<T>, ITestAssetLoader<T> where T : Object { }
