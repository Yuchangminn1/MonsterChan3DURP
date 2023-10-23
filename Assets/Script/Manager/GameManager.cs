using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }
    private static GameManager m_instance;

    GameObject player;
    PlayerController playerScript;
    void Awake()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public float DistanceToPlayer(Vector3 _position)
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = 0;
        _position.y = 0;
        return Vector3.Distance(playerPos, _position);
    }
}
