using System;
using Code.Common;
using UnityEngine;
using Zenject;

namespace Code.Project
{
    //TODO: unused
    public sealed class ScreenSizeChecker : ITickable
    {
        public event Action<Size> OnScreenResized;

        public Size ScreenSize { get; private set; }

        public void Tick()
        {
            if (ScreenSize.Width == Screen.width) return;
            if (ScreenSize.Height == Screen.height) return;

            Debug.Log("Update ScreenSize" + ": " + Time.frameCount);

            ScreenSize = new Size(Screen.width, Screen.height);
            OnScreenResized?.Invoke(ScreenSize);
        }
    }
}