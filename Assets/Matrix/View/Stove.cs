using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public GameObject ob1;
    public GameObject ob2;

    public void SetFire()
    {
        ob1.SetActive(true); 
        ob2.SetActive(true);
    }

    public void UnFire()
    {
        ob1.SetActive(false);
        ob2.SetActive(false);
    }
}
