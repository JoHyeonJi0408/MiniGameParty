using Unity.VisualScripting;
using UnityEngine;

public class ObstacleAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange;
    public float speed;
    
    private enum State { Idle, Chase, Block }
    private State currentState;

    private void Start()
    {
        currentState = State.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Block:
                break;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        if (currentState != State.Chase && dist < chaseRange)
        {
            currentState = State.Chase;
        }
    }

    private void Idle()
    {

    }

    private void Chase()
    {
        MoveToward(player.position);
    }

    private void MoveToward(Vector3 target)
    {
        Vector3 dir = (target - player.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var cc = other.GetComponentInChildren<CharacterController>();
            Vector3 newPos = new Vector3(1, 0.1f, 1);

            cc.enabled = false;
            other.transform.position = newPos;
            cc.enabled = true;

            currentState = State.Idle;
        }
    }
}
