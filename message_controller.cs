using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class message_controller : MonoBehaviour
{
    public IEnumerator Stay_On()
    {
        for(int t = 0;t < 90; t++)
        {
            yield return null;
        }
        gameObject.SetActive(false);
        yield break;
    }
}
