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

                //  Bu çubuk zaten yerleştirildiyse, seçilmesine izin verme
                if (stickScript != null && stickScript.isPlaced)
                {
                    Debug.LogWarning("[StickPlacement] This stick is already placed. Can't select.");
                    return;
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
                Debug.LogWarning("[StickPlacement] Bu çubuk zaten yerleştirilmiş!");
                selectedStick = null;
                return;
            }

            Vector3 snapped = gridManager.GetNearestGridPosition(selectedStick.transform.position);
            bool success = gridManager.PlaceStick(selectedStick, snapped);

            if (success)
            {
                Debug.Log("çubukları yerleştirmede sen bir harikasın1");
                selectedStick.transform.position = snapped;
                stickScript.isPlaced = true; //  Yerleştirildiğini burada işaretle
                stickSpawner.OnStickPlaced(selectedStick);
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


