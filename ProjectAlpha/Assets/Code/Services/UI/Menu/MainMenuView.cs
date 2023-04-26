using UnityEngine;

namespace Code.Services.UI
{
    public sealed class MainMenuView : MonoBehaviour, IMainMenuView
    {
        [SerializeField] private GameObject _soundOn;
        [SerializeField] private GameObject _soundOff;

        private void Awake() =>
            EnableSound();

        public void ToggleSound()
        {
            _soundOn.SetActive(!_soundOn.activeSelf);
            _soundOff.SetActive(!_soundOff.activeSelf);
        }

        private void EnableSound()
        {
            _soundOn.SetActive(true);
            _soundOff.SetActive(false);
        }
    }
}
