using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public int nbPlayer = 1;
    public bool keyboarded = true;

    Vector3 movement;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    //Transform thistransform;
    Animator anim;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();
        this.transform.Rotate(Vector3.up * -90);
        //thistransform = transform;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float h = 0;
        float v = 0;

        //Debug.Log("nbPLayer = " + nbPlayer + " nb Controller = " + nbController);
      
        if (!keyboarded)
        {
            h = Input.GetAxisRaw("Player" + nbPlayer + "LeftJoystickHorizontal");
            v = Input.GetAxisRaw("Player" + nbPlayer + "LeftJoystickVertical");
        }
        else
        {
            h = Input.GetAxis("HorizontalKeyboard");
            v = Input.GetAxis("VerticalKeyboard");
        }

        h = (h < 0.02 && h > -0.02) ? 0 : h;
        v = (v < 0.02 && v > -0.02) ? 0 : v;
        move(h, v);

        //si le controler du player est co
        if (!keyboarded)
        {
            turningWithController();
        }
        else
        {
            //si le controlleur du player n'est pas co
            turning();
        }

        animating(h, v);
    }

    private void move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void turningWithController()
    {
        Vector3 NextDir = new Vector3(Input.GetAxisRaw("Player" + nbPlayer + "RightJoystickHorizontal"), 0, Input.GetAxisRaw("Player" + nbPlayer + "RightJoystickVertical"));
        if (NextDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(NextDir);
        //CharCtrl.Move(NextDir / 8);
    }

    void turning()
    {
        Ray cameray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorhit;

        if (Physics.Raycast(cameray, out floorhit, camRayLength, floorMask))
        {
            Vector3 playertoMouse = floorhit.point - transform.position;
            playertoMouse.y = 0;

            Quaternion rotation = Quaternion.LookRotation(playertoMouse);

            playerRigidbody.MoveRotation(rotation);
        }
    }

    void animating(float h, float v)
    {
        bool isrunning;
        if (h != 0 || v != 0)
            isrunning = true;
        else
            isrunning = false;
        //Debug.Log("isRunning = " + isrunning);
        anim.SetBool("isRunning", isrunning);
    }
   
}
