using UnityEngine;

namespace Code.Services;

public sealed class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _soundOn;
    [SerializeField] private GameObject _soundOff;

    private void Awake() =>
        EnableSound();

    private void EnableSound()
    {
        _soundOn.SetActive(true);
        _soundOff.SetActive(false);
    }

    public void ToggleSound()
    {
        _soundOn.SetActive(!_soundOn.activeSelf);
        _soundOff.SetActive(!_soundOff.activeSelf);
    }
}