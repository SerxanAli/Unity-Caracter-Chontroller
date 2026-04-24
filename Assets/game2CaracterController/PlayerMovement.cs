using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;  // Character Controller-i buraya sürükləyəcəyik
    public float speed = 12f;               // Hərəkət sürəti
    Vector3 velocity;                       // Yerçəkimi (qravitasiya) üçün sürət vektoru
    public float gravity = -9.81f;          // Yerçəkimi dərəcəsi

    public float jumpHeight = 3f;

    // Gravitasya ve Tullanma
    public float groundDistance = 0.4f;

    Transform groundCheck; // elle atmiyim deye publicu silib startda tanitdiq
    LayerMask groundMask;  // elle atmiyim deye publicu silib startda tanitdiq

    bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        groundCheck = transform.Find("GroundCheck");

        groundMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {

        ///////  Yerçəkimi  
        // -s - Burda klavyaturadan oxlar ve asdw den gelen 1 ve -1 deyerini aliriq
        float x = Input.GetAxis("Horizontal"); // Sağ-sol hərəkəti (A, D və ya Oxlar)
        float z = Input.GetAxis("Vertical");   // İrəli-geri hərəkəti (W, S və ya Oxlar)

        // -s- burda ise dunya yox oz etrafinda donsun deye transiform automatik scriptin atildigi
        //     obje ye teyin edilib deye Vektor 3 den yaratdigimiz deyiskenin  raytini aliriq ki ( -1 vurulandda olacaq left)
        //     forvarda onu kimi. obsum 3 oxlu deyisken yaradib move ye teyin edirik ama
        Vector3 move = transform.right * x + transform.forward * z; // Bele yazanda persanaj hara baxirsa ora gedir


        controller.Move(move * speed * Time.deltaTime);


        // Yerçəkimi sürətini zamanla artırırıq
        velocity.y += gravity * Time.deltaTime;  // velocity.y = velocity.y + gravity * Time.deltaTime / yeni her update framede
        // bunun ustune -9.81 gelir oda asagi salir altinda Colidder yoxdusa asagi dusecek sohbet y deyiseninden gedir

          // -s- yaratdigimiz y deyiseni controller adini verdiyimiz eslinde playere aid olan yere Move funkun icinde
          // teyin edirik
        // Hesablanmış düşmə sürətini player-ə tətbiq edirik   
        controller.Move(velocity * Time.deltaTime);

      //  controller.Move((move * speed + velocity) * Time.deltaTime); Bu setir 2 dene MOVE ni birlesdirmek ucun numunedi

        ///// Yerdeyikmi  -s- Bu funksiyanin icinde yoxlama gedirki
        /// groundDistance radiusli bir sarikin icinde groundMask adli (layerli) bir obje ile ici ice kecibmi
        /// oda true ve ya false qaytarir 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // -s- Burda TEMA o duku durdugu yerde altinda yer(plane) objesi olsa bele gravity (-9.81) her updatede anu asagi
        // salir bizse onu -2 ye qaytariri ki sora Jump basanda -2 + JumpHight qeder oppansin Yoxsa adi halda graviti
        // toplanib -500 falan olanda ne qeder Jumpu bassanda -490 gelecek player opbanmiyacaq
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Tam 0 etmirik ki, yerə tam yapışsın
        }


        ///  Tullanma 
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Tullanma düsturu: v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        


      

        
    }
}
