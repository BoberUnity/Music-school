using UnityEngine;

public class SongPlace : MonoBehaviour
{
  [SerializeField] private GameObject arrow = null;  
  public bool IsChear = false;

  [SerializeField] private GameObject pit = null;
  public GameObject Pit
  {
    get { return pit;}
    set
    {
      pit = value;
      if(arrow != null)
        arrow.SetActive(pit == null);
    }
  }
}
