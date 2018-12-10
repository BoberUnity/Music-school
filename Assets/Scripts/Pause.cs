using System;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
  public event Action<bool> OnPause;
   
  public void ValueChange()
  {
    var handler = OnPause;
    if (handler != null)
      handler(GetComponent<Toggle>().isOn);
  }  
}
