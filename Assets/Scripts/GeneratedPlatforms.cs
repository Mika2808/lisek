using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] public GameObject platformPrefab;
    const int PLATFORMS_NUM = 9;
    GameObject[] platforms;
    Vector3[] platformsPosition;
    [SerializeField] private float speed = 1.2f;
    bool krec_sie_mala = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i < PLATFORMS_NUM; i++)
        {
            Vector3 buff = platforms[i].transform.position;         
            if (krec_sie_mala)
            {
                
                buff.x += 2;
                if (buff.x > 69)
                {
                    krec_sie_mala = false;
                }
            }
            else
            {
                buff.x -= 2;
                if (buff.x < 60)
                {
                    krec_sie_mala = true;
                }
            }

            platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position, buff, speed * Time.deltaTime);
        }

    }
    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        platformsPosition = new Vector3[PLATFORMS_NUM];

        for(int i = 0;i < PLATFORMS_NUM; i++)
        {
            platformsPosition[i].Set(56 + (2*i)%6, 14 + 2*i, 0);            
            platforms[i] = Instantiate(platformPrefab, platformsPosition[i], Quaternion.identity);
        }
    }
}
