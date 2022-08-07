﻿using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using VContainer;

namespace Code.Services.Entities;

public sealed class Cherry : SimpleSpriteEntity, ICherry
{
    private ICherryAnimations _animations;
    private ICherryPickHandler _pickHandler;
    private Settings _settings;

    [Inject, UsedImplicitly]
    private void Construct(ICherryAnimations animations, ICherryPickHandler pickHandler, Settings settings)
    {
        _animations = animations;
        _pickHandler = pickHandler;
        _settings = settings;
    }

    public UniTask MoveAsync(float destinationX) =>
        _animations.Move(transform, destinationX, _settings.MovementSpeed, DestroyToken);

    public void PickUp()
    {
        _pickHandler.OnCherryPicked(this);
        //show super effect
    }

    [System.Serializable]
    public class Settings
    {
        public float MovementSpeed = 10f;
    }
}