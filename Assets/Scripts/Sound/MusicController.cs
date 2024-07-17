using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Image _muteIcon;
    [SerializeField] Sprite _muteSprite;
    [SerializeField] Sprite _unmuteSprite;
    
    public void Mute()
    {
        _isMuted = !_isMuted;
        _audioSource.mute = _isMuted;
        _muteIcon.sprite = _isMuted ? _unmuteSprite : _muteSprite;
    }
    
    bool _isMuted;
}
