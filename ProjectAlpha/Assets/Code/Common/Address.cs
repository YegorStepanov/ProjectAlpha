using UnityEngine;

namespace Code;

public readonly record struct Address<T>(string Key) where T : Object;
