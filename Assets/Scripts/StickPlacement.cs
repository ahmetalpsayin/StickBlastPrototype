using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlacement : MonoBehaviour
{
    public GridManager gridManager;
    public StickSpawner stickSpawner;

    private Camera cam;
    private GameObject selectedStick;
    private Vector3 offset;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.CompareTag("Stick"))
            {
                Stick stickScript = hit.GetComponent<Stick>();

                if (stickScript != null && stickScript.isPlaced)
                {
                    Debug.Log("[StickPlacement] Bu çubuk zaten yerleştirilmiş, tekrar seçilemez.");
                    return; //   seçilmesine izin verme
                }

                selectedStick = hit.gameObject;
                offset = selectedStick.transform.position - (Vector3)mousePos;
            }
        }

        if (Input.GetMouseButton(0) && selectedStick != null)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            selectedStick.transform.position = mousePos + (Vector2)offset;
        }

        if (Input.GetMouseButtonUp(0) && selectedStick != null)
        {

            Stick stickScript = selectedStick.GetComponent<Stick>();

            //  Eğer bu çubuk zaten yerleştirildiyse tekrar bırakılmasın
            if (stickScript != null && stickScript.isPlaced)
            {
                Debug.Log("[StickPlacement] Zaten yerleştirilmiş çubuğu tekrar bırakmak istedin.");
                selectedStick = null;
                return;
            }

            Vector3 snapped = gridManager.GetNearestGridPosition(selectedStick.transform.position);
            bool success = gridManager.PlaceStick(selectedStick, snapped);

            if (success)
            {
                Debug.Log("çubukları yerleştirmede sen bir harikasın!");
                selectedStick.transform.position = snapped;
                stickScript.isPlaced = true; //  Yerleştirildiğini burada işaretle
                stickSpawner.OnStickPlaced(selectedStick); 

                // BONUS: Tekrar seçilemesin diye collider'ı kapat
                selectedStick.GetComponent<Collider2D>().enabled = false;
                selectedStick.tag = "Untagged";
            }
            else
            {
                //Destroy(selectedStick); // Geçersiz konumda bırakıldıysa sil

                // Veya alternatif olarak: uyarı ver, silme
                // Seçim alanındaysa, orijinal pozisyona geri gönder 
                Debug.Log("o da nesi?? Çubukları ızgaranın içine bırakmalısın!");

                // Geri dönmesi için başlangıç pozisyonunu saklamamız lazım! 
                // Yerleştirme başarısızsa geri seçim alanına gönder
                selectedStick.transform.position = stickScript.startPosition;
            }

            selectedStick = null;
        }
    }
}


