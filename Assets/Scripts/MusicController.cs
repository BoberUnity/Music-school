using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
  [SerializeField] private Pause pause = null;
  [SerializeField] private Image songBarProgress = null;
  [SerializeField] private Text textDown = null;
  [SerializeField] private Text textUp = null;
  [SerializeField] private float textBorder = 0.15f;
  [SerializeField] private EllipsoidParticleEmitter stars = null;
  [SerializeField] private Transform left = null;
  [SerializeField] private Transform right = null;

  private AudioSource audioSource = null;
  private float songTime = 0;
  private bool isPlaying = false;
  private float textSpeed = 0;

  void Update()
  {
    if (isPlaying)
      songTime += Time.deltaTime;
    float songProgress = songTime / audioSource.clip.length;
    songBarProgress.fillAmount = songProgress;
    if (songProgress > textBorder && songProgress < 1 - textBorder)
    {      
      int lastSymbol = (int)(textDown.text.Length * (songProgress - textBorder) * textSpeed);
      textUp.text = textDown.text.Substring(0, lastSymbol);
    }
    stars.transform.position = Vector3.Lerp(left.position, right.position, songProgress);
  }

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    pause.OnPause += OnPause;
    textSpeed = 1 / (1 - textBorder * 2);
  }

  void OnDestroy()
  {
    pause.OnPause -= OnPause;
  }

  void Play()
  {
    audioSource.Play();
    isPlaying = true;
    stars.emit = true;
  }
	
	void Pause ()
  {
    audioSource.Pause();
    isPlaying = false;
    stars.emit = false;
  }

  void OnPause(bool isOn)
  {
    if (isOn)
      Pause();
    else
      Play();
  }
}
