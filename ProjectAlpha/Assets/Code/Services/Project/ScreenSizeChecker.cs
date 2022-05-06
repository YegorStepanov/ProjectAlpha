using System;
using UnityEngine;
using VContainer.Unity;

namespace Code.Services;

//TODO: unused
public sealed class ScreenSizeChecker : ITickable
{
    public Size ScreenSize { get; private set; }

    public void Tick()
    {
        if (ScreenSize.Width == Screen.width) return;
        if (ScreenSize.Height == Screen.height) return;

        Debug.Log("Update ScreenSize" + ": " + Time.frameCount);

        ScreenSize = new Size(Screen.width, Screen.height);
        OnScreenResized?.Invoke(ScreenSize);
    }

    public event Action<Size> OnScreenResized;
}