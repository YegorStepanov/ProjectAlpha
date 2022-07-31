﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Common;

public static class StartupInfo
{
    public static string StartupSceneName { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        StartupSceneName = SceneManager.GetActiveScene().name;
    }
}
