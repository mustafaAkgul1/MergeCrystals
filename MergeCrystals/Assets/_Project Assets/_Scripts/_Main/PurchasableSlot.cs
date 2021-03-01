using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchasableSlot : MonoBehaviour
{
    public int purchaseCost;
    public Slot toBeActivatedSlotScript;
    public GameObject toBeDeactivatedGoldIcon;
    public SpriteRenderer spriteRenderer;
    Vector2 checkArea = new Vector2(0.25f, 0.25f);
    bool purchased = false;

    public GameObject costTextObj;

    void Start()
    {
        costTextObj.GetComponent<TextMeshPro>().text = purchaseCost.ToString();

    } // Start()

    private void OnMouseDown()
    {
        if (!purchased)
        {
            if (GameManager._instance.PurchaseNewSlot(purchaseCost))
            {
                purchased = true;
                toBeActivatedSlotScript.enabled = true;
                spriteRenderer.color = Color.white;
                Destroy(toBeDeactivatedGoldIcon);
                GameManager._instance.slots.Add(gameObject);
                gameObject.layer = LayerMask.NameToLayer("Slot");
                CreatePurchasableSlotsAround();
                Destroy(costTextObj);
                GameManager._instance.purchasableSlots.Remove(gameObject);
                GameManager._instance.AddSlotDataToSave(transform.position, toBeActivatedSlotScript.activeOrb);
                Destroy(this);
            }
            else
            {
                //Debug.Log("yetersiz para");
            }
        }

    } // OnMouseDown()

    void CreatePurchasableSlotsAround()
    {
        //Left Check
        Collider2D leftSlotCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(-1f, 0f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("Slot"));
        if (leftSlotCheckColl == null)
        {
            Collider2D leftCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(-1f, 0f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("PurchasableSlot"));
            if (leftCheckColl == null)
            {
                GameObject tmp1 = Instantiate(GameManager._instance.purchasableSlotPrefab, transform.position + new Vector3(-1f, 0f, 0f), Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                int tmpCost = SlotCostListCheck(tmp1.transform);
                tmp1.GetComponent<PurchasableSlot>().purchaseCost = tmpCost;
                tmp1.GetComponent<PurchasableSlot>().costTextObj.GetComponent<TextMeshPro>().text = tmpCost.ToString();
                //Debug.Log("fiyat Sol : " + tmpCost.ToString());

                GameManager._instance.purchasableSlots.Add(tmp1);
                GameManager._instance.AddPurchasableSlotDataToSave(tmp1.transform.position, tmpCost); // test
            }
            else
            {
                //Debug.Log("sol tarafı zaten dolu");
            }
        }
        else
        {
            //Debug.Log("sol tarafta slot var");
        }

        //Right Check
        Collider2D rightSlotCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(1f, 0f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("Slot"));
        if (rightSlotCheckColl == null)
        {
            Collider2D rightCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(1f, 0f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("PurchasableSlot"));
            if (rightCheckColl == null)
            {
                GameObject tmp2 = Instantiate(GameManager._instance.purchasableSlotPrefab, transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                int tmpCost2 = SlotCostListCheck(tmp2.transform);
                tmp2.GetComponent<PurchasableSlot>().purchaseCost = tmpCost2;
                tmp2.GetComponent<PurchasableSlot>().costTextObj.GetComponent<TextMeshPro>().text = tmpCost2.ToString();
                //Debug.Log("fiyat Sağ : " + tmpCost2.ToString());

                GameManager._instance.purchasableSlots.Add(tmp2);
                GameManager._instance.AddPurchasableSlotDataToSave(tmp2.transform.position, tmpCost2); // test
            }
            else
            {
                //Debug.Log("sağ tarafı zaten dolu");
            }
        }
        else
        {
            //Debug.Log("sağ tarafta slot var");
        }

        //Upper Check
        Collider2D upperBoundryCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(0f, 1f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("UpperSlotBoundry"));
        if (upperBoundryCheckColl == null)
        {
            Collider2D upperSlotCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(0f, 1f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("Slot"));
            if (upperSlotCheckColl == null)
            {
                Collider2D upperCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(0f, 1f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("PurchasableSlot"));
                if (upperCheckColl == null)
                {
                    GameObject tmp3 = Instantiate(GameManager._instance.purchasableSlotPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                    int tmpCost3 = SlotCostListCheck(tmp3.transform);
                    tmp3.GetComponent<PurchasableSlot>().purchaseCost = tmpCost3;
                    tmp3.GetComponent<PurchasableSlot>().costTextObj.GetComponent<TextMeshPro>().text = tmpCost3.ToString();
                    //Debug.Log("fiyat yukarı : " + tmpCost3.ToString());

                    GameManager._instance.purchasableSlots.Add(tmp3);
                    GameManager._instance.AddPurchasableSlotDataToSave(tmp3.transform.position, tmpCost3); // test
                }
                else
                {
                    //Debug.Log("yukarı tarafı zaten dolu");
                }
            }
            else
            {
                //Debug.Log("yukarı tarafta slot var");
            }
        }
        else
        {
            //Debug.Log("üst sınıra ulaşıldı");
        }

        //Lower Check
        Collider2D lowerBoundryCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(0f, -1f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("LowerSlotBoundry"));
        if (lowerBoundryCheckColl == null)
        {
            Collider2D lowerSlotCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(0f, -1f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("Slot"));
            if (lowerSlotCheckColl == null)
            {
                Collider2D lowerCheckColl = Physics2D.OverlapBox(transform.position + new Vector3(0f, -1f, 0f), checkArea, 0f, 1 << LayerMask.NameToLayer("PurchasableSlot"));
                if (lowerCheckColl == null)
                {
                    GameObject tmp4 = Instantiate(GameManager._instance.purchasableSlotPrefab, transform.position + new Vector3(0f, -1f, 0f), Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                    int tmpCost4 = SlotCostListCheck(tmp4.transform);
                    tmp4.GetComponent<PurchasableSlot>().purchaseCost = tmpCost4;
                    tmp4.GetComponent<PurchasableSlot>().costTextObj.GetComponent<TextMeshPro>().text = tmpCost4.ToString();
                    //Debug.Log("fiyat yukarı : " + tmpCost4.ToString());

                    GameManager._instance.purchasableSlots.Add(tmp4);
                    GameManager._instance.AddPurchasableSlotDataToSave(tmp4.transform.position, tmpCost4); // test
                }
                else
                {
                    //Debug.Log("aşağı tarafı zaten dolu");
                }
            }
            else
            {
                //Debug.Log("aşağı tarafta slot var");
            }
        }
        else
        {
            //Debug.Log("alt sınıra ulaşıldı");
        }

    } // CreatePurchasableSlotsAround()

    int SlotCostListCheck(Transform _transform)
    {
        int _value = 0;
        foreach (KeyValuePair<Vector2,int> item in GameManager._instance.slotCostList)
        {
            Vector2 tmpPos = new Vector2(_transform.position.x, _transform.position.y);
            //Debug.Log("tmpPos : " + tmpPos.ToString() + " item.Key : " + item.Key.ToString() + " item.Value : " + item.Value.ToString());
            if (tmpPos == item.Key)
            {
                _value = item.Value;
                return _value;
            }
        }
        return _value;

    } // SlotCostListCheck()

} // class
