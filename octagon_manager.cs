using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class octagon_manager : MonoBehaviour {

    public GameObject octa_prefab;

    public GameObject[,] octagons = new GameObject[19,11];

    private void Start()
    {
        Spawn_Octagons();
    }

    public void Spawn_Octagons()
    {
        for(int x = 0;x < 19; x++)
        {
            for(int y = 0;y < 11; y++)
            {
                float new_x = (-106.0f * 9.0f) + (106.0f * x);
                float new_y = (106.0f * 5.0f) - (106.0f * y);

                octagons[x, y] = Instantiate(octa_prefab);
                octagons[x, y].transform.SetParent(gameObject.transform);
                octagons[x, y].transform.localPosition = new Vector2(new_x, new_y);
            }
        }
    }
}
