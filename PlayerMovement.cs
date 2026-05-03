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

/*
Çox yaxşı sualdı 👍 bunu başa düşsən Unity movement-in 70%-ni çözmüş olursan.
Sənə sadə qayda ilə izah edim:

🔥 1. World space (dünyaya görə)
Vector3 move = new Vector3(horizontal, 0, vertical);
✔ Nə deməkdir?


Sağ = həmişə dünya sağ


İrəli = həmişə dünya forward


✔ Nə vaxt istifadə olunur?
👉 2D / 2.5D oyunlarda
👉 Kamera sabitdirsə
👉 Sadə movement istəyirsənsə
✔ Sənin case:
✔ Sən bunu istifadə etməlisən

🔥 2. Local space (özünə görə)
Vector3 move = transform.right * horizontal + transform.forward * vertical;
✔ Nə deməkdir?


“irəli” = character hara baxırsa ora


“sağ” = character-in sağı


✔ Nə vaxt istifadə olunur?
👉 FPS / TPS oyunlarda
👉 Kamera ilə birlikdə hərəkət edəndə
👉 PUBG, GTA, shooter sistemlərdə

⚠ Sənin niyə bug oldu?
Sən bunu etdin:
move = transform.forward ...transform.forward = direction;
👉 Yəni:


movement → rotation-a bağlı


rotation → movement-ə bağlı


💥 nəticə: loop + bug

✔ Qızıl qayda (yadda saxla)
👉 Əgər:


özün rotation verirsənsə
✔ world space istifadə et


👉 Əgər:


rotation artıq var (kamera/character)
✔ local space istifadə et



🔥 Sadə müqayisə
SistemMovementRotation2.5D (sənin oyun)WorldÖzün verirsənFPS / TPSLocalKamera verir

💡 Bonus (pro tip)
Əgər gələcəkdə belə sistem edəcəksənsə:
👉 Mouse ilə kamera
👉 Player kameranın baxdığı yerə getsin
onda belə olacaq:
move = camera.forward * vertical + camera.right * horizontal;

🔚 Nəticə
👉 Sadə oyun → world space
👉 Kamera-based oyun → local space

İstəsən sənə:
👉 2.5D üçün tam hazır movement sistemi
👉 jump + gravity + animasiya
tam “clean” kod kimi yığım verim 👊

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControlScript : MonoBehaviour
{

    CharacterController Player;
    Animator anim;

    float Speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Vertical"); 
        float z = Input.GetAxis("Horizontal");

     // Vector3 move = transform.right * x + transform.forward * z;  / oz kordinatina gore 
        Vector3 move = new Vector3(x, 0, z);  // dunya kordinatina gore

        Player.Move(move * Time.deltaTime * Speed);

        if (move != Vector3.zero)
        {
            transform.forward = move;
            anim.SetBool("isRuning", true);
        }
        else
        {
            anim.SetBool("isRuning", false);
        }
    }
}

*/
