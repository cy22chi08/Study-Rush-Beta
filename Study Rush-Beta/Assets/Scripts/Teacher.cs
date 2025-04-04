using UnityEngine;
using UnityEngine.SceneManagement;

public class Teacher : MonoBehaviour
{
  public Transform playerTarget;
  public float currentSpeed;
  quizV2 chaser;

    void Start()
    {
        chaser = FindAnyObjectByType<quizV2>();
    }

    // Update is called once per frame
    void Update()
    {
         currentSpeed = chaser.speedChase;

        transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, currentSpeed * Time.deltaTime);
    }

     void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
