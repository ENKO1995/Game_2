using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollider : MonoBehaviour
{
    public  TextMeshProUGUI  CoinText;

    public int CoinCounter;
    // Start is called before the first frame update
    void Start()
    {
        CoinCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //OnTriggerEnter();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Coin")
        {
            //SoundManager.PlaySound("PickUp");

            CoinCounter++;
            CoinText.text = CoinCounter.ToString();
            Debug.Log("Coins: " + CoinCounter);
            Destroy(collision.gameObject);
        }
        //else if (collision.tag == "Teleporter" && CoinCounter == 3)
        //{
        //    Debug.Log("Win");
        //    //SceneManager.LoadScene(3);
        //}
    }
}
