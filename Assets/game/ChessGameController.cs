using UnityEngine;

public class ChessGameController : MonoBehaviour
{
    private GameObject selectedPiece;

    // Skriptin ən yuxarısında, selectedPiece-in yanında bunu yaz:
    private bool isWhiteTurn = true;



    [Header("Ayarlar")]
    public LayerMask clickableLayer;
    public float pieceYOffset = 0.5f; // Fiqurun taxta üzərindəki hündürlüyü (Y oxu)

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, clickableLayer))
            {
                if (hit.collider.CompareTag("Piece"))
                {
                    SelectPiece(hit.collider.gameObject);
                }
                else if (selectedPiece != null && hit.collider.CompareTag("Board"))
                {
                    MovePiece(hit.point);
                }
            }
        }
    }

    void SelectPiece(GameObject piece)
    {
        selectedPiece = piece;
        Debug.Log("Seçildi: " + piece.name);
    }


    void MovePiece(Vector3 worldPoint)
    {
        float newX = Mathf.Floor(worldPoint.x / 2) * 2 + 1;
        float newZ = Mathf.Floor(worldPoint.z / 2) * 2 + 1;
        float oldX = selectedPiece.transform.position.x;
        float oldZ = selectedPiece.transform.position.z;

        // 1. Növbə Yoxlaması
        if (isWhiteTurn && !selectedPiece.name.Contains("White")) return;
        if (!isWhiteTurn && !selectedPiece.name.Contains("Black")) return;

        // 2. Piyada (Pawn) Məntiqi
        if (selectedPiece.name.Contains("Pawn"))
        {
            if (newX != oldX) { selectedPiece = null; return; } // Yan tərəfə qadağadır

            float distance = newZ - oldZ; // Gedilən məsafə

            if (isWhiteTurn) // AĞLAR (Z artır)
            {
                bool isFirstMove = (oldZ == -5f); // Sənin koordinat sistemində ağ piyadanın başlanğıcı -5-dirsə
                float maxStep = isFirstMove ? 4.1f : 2.1f; // İlk gedişdə 2 xana (4 vahid)

                if (distance <= 0 || distance > maxStep) { selectedPiece = null; return; }
            }
            else // QARALAR (Z azalır)
            {
                bool isFirstMove = (oldZ == 5f); // Qara piyadanın başlanğıcı 5-dirsə
                float maxStep = isFirstMove ? -4.1f : -2.1f;

                if (distance >= 0 || distance < maxStep) { selectedPiece = null; return; }
            }
        }

        // Hərəkəti icra et
        selectedPiece.transform.position = new Vector3(newX, 0, newZ);

        // Növbəni dəyiş
        isWhiteTurn = !isWhiteTurn;
        selectedPiece = null;
        Debug.Log(isWhiteTurn ? "Ağların növbəsidir" : "Qaraların növbəsidir");


        // At (Knight) Məntiqi
        if (selectedPiece.name.Contains("Knight"))
        {
            float dx = Mathf.Abs(newX - oldX); // X oxu üzrə məsafə
            float dz = Mathf.Abs(newZ - oldZ); // Z oxu üzrə məsafə

            // "L" hərfi: (2 xana və 1 xana) VƏ YA (1 xana və 2 xana)
            // Sənin sistemində bu (4 vahid və 2 vahid) deməkdir.
            bool isLMove = (dx == 4f && dz == 2f) || (dx == 2f && dz == 4f);

            if (!isLMove)
            {
                Debug.Log("At yalnız L formasında hərəkət edə bilər!");
                selectedPiece = null;
                return;
            }
        }


    }
}