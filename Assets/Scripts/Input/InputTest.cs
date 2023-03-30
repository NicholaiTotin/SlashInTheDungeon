using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;

public class InputTest : MonoBehaviour
{
    [SerializeField] SwipeListener swipeListener;
    [SerializeField] TrailRenderer trail;
    [SerializeField] Animator anim;
    [SerializeField] GameObject UI;
    [SerializeField] AudioSource wrongeMove;
    
    Queue<string> keyQueue;
    int randomValue;

    [HideInInspector] public bool enemyInFront;
    public int valueEnemy;


    private void Awake()
    {
        keyQueue = new Queue<string>();
    }

    private void OnEnable()
    {
        swipeListener.OnSwipe.AddListener(OnSwipe);
    }

    private void OnSwipe(string swipe)
    {
        Debug.Log(swipe);
        switch (swipe)
        {
            case "Down":
                keyQueue.Enqueue(swipe);
                Invoke("RemoveAction", 0.15f);
                anim.SetFloat("leftSpeed", 0f);
                anim.SetFloat("rightSpeed", 0f);
                randomValue = Random.Range(0, 5);
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hall_Left_Corner") &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.92f ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("Hall_Right_Corner") &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.92f)
                {
                    wrongeMove.Play();
                }
                break;
            case "Right":
                keyQueue.Enqueue(swipe);
                Invoke("RemoveAction", 0.15f);
                anim.SetFloat("rightSpeed", 0f);
                anim.SetFloat("speed", 0f);

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Hall_Left_Corner") &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.92f ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("Hall_Forward"))
                {
                    wrongeMove.Play();
                }
                break;
            case "Left":
                keyQueue.Enqueue(swipe);
                Invoke("RemoveAction", 0.15f);
                anim.SetFloat("leftSpeed", 0f);
                anim.SetFloat("speed", 0f);

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Hall_Right_Corner") &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.92f ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("Hall_Forward"))
                {
                    wrongeMove.Play();
                }
                break;
            default:
                anim.SetFloat("leftSpeed", 0f);
                anim.SetFloat("rightSpeed", 0f);
                anim.SetFloat("speed", 0f);
                break;

        }
    }

    private void OnDisable()
    {
        swipeListener.OnSwipe.RemoveListener(OnSwipe);
    }

    private void Update()
    {
        anim.SetInteger("random", randomValue);

        if (keyQueue.Count > 0 && !enemyInFront)
        {
            if(keyQueue.Peek() == "Down")
            {
                anim.SetFloat("speed", 1f);
            }

            if (keyQueue.Peek() == "Right")
            {
                anim.SetFloat("leftSpeed", 1f);
            }

            if (keyQueue.Peek() == "Left")
            {
                anim.SetFloat("rightSpeed", 1f);
            }
        }

        if(valueEnemy > 3)
        {
            valueEnemy = 0;
        }

        if (Input.GetMouseButton(0))
        {
            trail.enabled = true;
        }
        else
        {
            trail.enabled = false;
        }

        if (enemyInFront)
        {
            anim.speed = 0;
        }
        else
        {
            anim.speed = 1;
            UI.SetActive(false);
        }
    }

    public void RemoveAction()
    {
        if(keyQueue.Count > 0)
        {
            keyQueue.Dequeue();
        }
        return;
    }

    public void ValueEnemyCount()
    {
        valueEnemy++;
    }

    public void RemoveEnemy()
    {
        enemyInFront = false;
    }
}
