using UnityEngine;

public class ButtonPit : MonoBehaviour
{
  [SerializeField]  [Tooltip("prefab of animal")]  GameObject pitAnimPrefab = null;
  [SerializeField]  [Tooltip("Time in seconds")]  private float dragInterval = 0.2f;
  [SerializeField]  private Collider2D myPlace = null;
  [SerializeField]  private Pause pause = null;
  private GameObject pitAnimObj = null;
  private bool isDragging = false;
  private bool isPressed = false;
  private float pressTime = 0;
  private Vector3 mousePositionPrevious = Vector3.zero;
  private float screenScaleFactor = 0.0f;  
  private Collider2D currentSongPlaceCollider = null;
  private GameController gameController = null;

  void Start()
  {
    pause.OnPause += OnPause;
    gameController = FindObjectOfType<GameController>();
    screenScaleFactor = 8 / (float)Screen.height;
    myPlace.GetComponent<SongPlace>().Pit = gameObject;
  }

  void Update()
  {
    if (isPressed)
      pressTime += Time.deltaTime;
    if (isPressed && pressTime > dragInterval)
      isDragging = true;
    if (isDragging)
    {
      transform.position += (Input.mousePosition - mousePositionPrevious) * screenScaleFactor;
      if (pitAnimObj != null)
        pitAnimObj.transform.position += (Input.mousePosition - mousePositionPrevious) * screenScaleFactor;
    }
    mousePositionPrevious = Input.mousePosition;
  }

  void OnMouseDown()
  {
    isPressed = true;    
  }

  void OnMouseUp()
  {
    if (!isDragging)
    {
      //Нажатие
      if (myPlace.GetComponent<SongPlace>().IsChear)      
        GoToFreeSongPlace();        
      else      
        GoToFreeSitPlace();        
    }
    else
    {
      Drop();
    }
    isPressed = false;
    pressTime = 0;
  }

  void OnTriggerEnter2D(Collider2D other2d)
  {
    SongPlace songPlace = other2d.GetComponent<SongPlace>();
    if (songPlace != null && songPlace.Pit == null)
    {
      currentSongPlaceCollider = other2d;      
    }
  }

  void OnTriggerExit2D(Collider2D other2d)
  {
    SongPlace songPlace = other2d.GetComponent<SongPlace>();
    if (songPlace != null && other2d == currentSongPlaceCollider)
      currentSongPlaceCollider = null;
  }

  void Drop()
  {
    if (currentSongPlaceCollider != null)
    {
      SitOnPlace();
    }
    else
    {
      transform.position = myPlace.transform.position;
      if (pitAnimObj != null)
        pitAnimObj.transform.position = myPlace.transform.position;
    }
    isDragging = false;
  }

  void SitOnPlace()
  {    
    if (currentSongPlaceCollider.GetComponent<SongPlace>().IsChear)
    {
      if (pitAnimObj != null)
        Destroy(pitAnimObj.gameObject);
      GetComponent<SpriteRenderer>().enabled = true;      
    }
    else
    {
      if (pitAnimObj == null)
        pitAnimObj = (GameObject)Instantiate(pitAnimPrefab, currentSongPlaceCollider.transform.position, Quaternion.identity);
      else
        pitAnimObj.transform.position = currentSongPlaceCollider.transform.position;
      GetComponent<SpriteRenderer>().enabled = false;
      currentSongPlaceCollider.GetComponent<SongPlace>().Pit = gameObject;  
    }    
    myPlace.GetComponent<SongPlace>().Pit = null;
    myPlace = currentSongPlaceCollider;
    myPlace.GetComponent<SongPlace>().Pit = gameObject;
    transform.position = myPlace.transform.position;
    currentSongPlaceCollider = null;
    gameController.CheckAllSlots();
  }

  void GoToFreeSongPlace()
  {
    foreach (var songPlace in gameController.SongPlaces)
    {
      if (songPlace.Pit == null)
      {
        currentSongPlaceCollider = songPlace.GetComponent<Collider2D>();
        SitOnPlace();
        break;
      }
    }    
  }

  void GoToFreeSitPlace()
  {
    foreach (var sitPlace in gameController.SitPlaces)
    {
      if (sitPlace.Pit == null)
      {
        currentSongPlaceCollider = sitPlace.GetComponent<Collider2D>();
        SitOnPlace();
        break;
      }
    }    
  }

  void OnPause(bool isOn)
  {
    if (isOn)
    {
      if (pitAnimObj != null)
        pitAnimObj.GetComponentInChildren<Animator>().SetTrigger("Stay");
    }
    else
    {
      if (pitAnimObj != null)
        pitAnimObj.GetComponentInChildren<Animator>().SetTrigger("Sing");
    }
  }  
}
