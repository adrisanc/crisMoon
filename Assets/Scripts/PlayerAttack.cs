using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform ballonPoint;
    [SerializeField] private GameObject[] ballons;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown)//if mouse botton is press
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        ballons[FindBallon()].transform.position = ballonPoint.position;
        ballons[FindBallon()].GetComponent<BallonPlayer>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindBallon()
    {
        for (int i = 0; i < ballons.Length; i++)
        {
            if (!ballons[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
