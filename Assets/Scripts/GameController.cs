using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public SongPlace[] SongPlaces = new SongPlace[4];
  public SongPlace[] SitPlaces = new SongPlace[4];
  [SerializeField] private Toggle pauseToggle = null;

  public void CheckAllSlots()
  {
    bool hasFreeSlots = false;
    foreach (var songPlace in SongPlaces)
    {
      if (songPlace.Pit == null)
        hasFreeSlots = true;
    }
    pauseToggle.gameObject.SetActive(!hasFreeSlots);
  }	
}
