using UnityEngine;

public class Hammer : MonoBehaviour
{
    public SHM shm;
    public GameObject ps;
    Move_Player Player;
    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Move_Player>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.tag == "Player")
        {
            GameObject gmm = Instantiate(ps, contact.point, Quaternion.Euler(Vector3.zero));
            gmm.GetComponent<ParticleSystem>().Play();
            Player.Dead(true);
        }
        else if(collision.gameObject.tag == "Platform" || collision.gameObject.name == "Arena_A")
            Instantiate(ps, contact.point, Quaternion.Euler(Vector3.zero));
        var player = GameObject.Find("Player");
        if (Mathf.Abs(Vector3.Distance(player.transform.position, contact.point)) < 5f)
        {
            player.GetComponent<Rigidbody>().AddForce((player.transform.position - contact.point).normalized * 800f);
            Player.Dead(true);
        }
    }
}
