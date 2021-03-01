using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbClass : MonoBehaviour
{
    public int orbLevel = 1;
    [HideInInspector] public Vector2 previousPosition;
    bool isDragging = false;
    Vector2 dropCheckArea = new Vector2(0.25f, 0.25f);
    [HideInInspector] public Animator anim;
    [HideInInspector] public bool mergedToDestroy = false;

    //updated game design variables
    public float productionTime = 10f;
    float currentProductionTime;
    public int productionCapacity = 3;
    [HideInInspector] public List<GameObject> productionStorage;
    bool canHarvest = true;
    [HideInInspector] public GameObject createdFirstProductionVFXObj;
    [HideInInspector] public GameObject createdFullProductionVFXObj;
    [HideInInspector] public bool canSpawnCopy = true;
    [HideInInspector] public bool createdFromSave = false;

    public enum OrbType
    {
        Fire,
        Water,
        Air,
        Earth,
        Void,
        Poison,
        Lightning
    }
    public OrbType orbType;

    void Awake()
    {
        anim = GetComponent<Animator>();

    } // Awake()

    void Start()
    {
        previousPosition = transform.position;
        productionStorage = new List<GameObject>();

        if (createdFromSave)
        {
            CheckOfflineProduction();
        }
        else
        {
            currentProductionTime = productionTime;
        }

        StartCoroutine(StartProduction());

    } // Start()

    void CheckOfflineProduction()
    {
        float offlineSeconds = GameManager._instance.offlineControllerScript.offlineSeconds;
        bool firstVfxSpawned = false;
        bool fullVfxSpawned = false;

        if (offlineSeconds > productionTime)
        {

            while (offlineSeconds >= productionTime)
            {
                offlineSeconds -= productionTime;
                if (productionStorage.Count < productionCapacity)
                {
                    switch (orbType)
                    {
                        case OrbType.Fire:
                            productionStorage.Add(GameManager._instance.projectileList[0]);
                            break;
                        case OrbType.Water:
                            productionStorage.Add(GameManager._instance.projectileList[1]);
                            break;
                        case OrbType.Air:
                            productionStorage.Add(GameManager._instance.projectileList[2]);
                            break;
                        case OrbType.Earth:
                            productionStorage.Add(GameManager._instance.projectileList[3]);
                            break;
                        case OrbType.Void:
                            productionStorage.Add(GameManager._instance.projectileList[4]);
                            break;
                        case OrbType.Poison:
                            productionStorage.Add(GameManager._instance.projectileList[5]);
                            break;
                        case OrbType.Lightning:
                            productionStorage.Add(GameManager._instance.projectileList[6]);
                            break;
                        default:
                            break;

                    } // switch

                    if (!firstVfxSpawned)
                    {
                        firstVfxSpawned = true;
                        if (createdFirstProductionVFXObj == null)
                        {
                            createdFirstProductionVFXObj = VFXManager._instance.SpawnCrystalFirstProductionVFX(this.gameObject.transform, orbType);
                        }
                    }
                }
                else
                {
                    currentProductionTime = 0f;
                    if (createdFirstProductionVFXObj != null)
                    {
                        Destroy(createdFirstProductionVFXObj);
                        createdFirstProductionVFXObj = null;
                    }
                    if (!fullVfxSpawned)
                    {
                        if (createdFullProductionVFXObj == null)
                        {
                            createdFullProductionVFXObj = VFXManager._instance.SpawnCrystalFullProductionVFX(this.gameObject.transform, orbType);
                        }
                    }
                    break;
                }

            } // while offline seconds

            if (productionStorage.Count < productionCapacity)
            {
                currentProductionTime = productionTime - offlineSeconds;
            }
            else
            {
                if (createdFirstProductionVFXObj != null)
                {
                    Destroy(createdFirstProductionVFXObj);
                    createdFirstProductionVFXObj = null;
                }
                if (!fullVfxSpawned)
                {
                    if (createdFullProductionVFXObj == null)
                    {
                        createdFullProductionVFXObj = VFXManager._instance.SpawnCrystalFullProductionVFX(this.gameObject.transform, orbType);
                    }
                }
            }
        }
        else
        {
            currentProductionTime = productionTime - offlineSeconds;
        }

    } // CheckOfflineProduction()

    IEnumerator StartProduction()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            currentProductionTime--;
            if (currentProductionTime <= 0f && productionStorage.Count < productionCapacity)
            {
                switch (orbType)
                {
                    case OrbType.Fire:
                        productionStorage.Add(GameManager._instance.projectileList[0]);
                        //Debug.Log("üretildi Fire");
                        break;
                    case OrbType.Water:
                        productionStorage.Add(GameManager._instance.projectileList[1]);
                        //Debug.Log("üretildi Water");
                        break;
                    case OrbType.Air:
                        productionStorage.Add(GameManager._instance.projectileList[2]);
                        //Debug.Log("üretildi Air");
                        break;
                    case OrbType.Earth:
                        productionStorage.Add(GameManager._instance.projectileList[3]);
                        //Debug.Log("üretildi Earth");
                        break;
                    case OrbType.Void:
                        productionStorage.Add(GameManager._instance.projectileList[4]);
                        //Debug.Log("üretildi Void");
                        break;
                    case OrbType.Poison:
                        productionStorage.Add(GameManager._instance.projectileList[5]);
                        //Debug.Log("üretildi Poison");
                        break;
                    case OrbType.Lightning:
                        productionStorage.Add(GameManager._instance.projectileList[6]);
                        //Debug.Log("üretildi Lightning");
                        break;
                    default:
                        break;

                } // switch

                if (productionStorage.Count < productionCapacity)
                {
                    currentProductionTime = productionTime;
                }
                else
                {
                    currentProductionTime = 0f;
                }

                if (productionStorage.Count < productionCapacity && createdFirstProductionVFXObj == null)
                {
                    createdFirstProductionVFXObj = VFXManager._instance.SpawnCrystalFirstProductionVFX(this.gameObject.transform, orbType);
                }
                else if (productionStorage.Count == productionCapacity && createdFullProductionVFXObj == null)
                {
                    createdFullProductionVFXObj = VFXManager._instance.SpawnCrystalFullProductionVFX(this.gameObject.transform, orbType);
                    if (createdFirstProductionVFXObj != null)
                    {
                        Destroy(createdFirstProductionVFXObj);
                        createdFirstProductionVFXObj = null;
                    }
                }

            }

        }

    } // StartProduction()

    private void OnMouseDown()
    {
        isDragging = true;
        Collider2D droppedColl = Physics2D.OverlapBox(transform.position, dropCheckArea, 0f, 1 << LayerMask.NameToLayer("Slot"));
        droppedColl.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Empty;
        droppedColl.gameObject.GetComponent<Slot>().activeOrb = null;
        GameManager._instance.heldOrbEndOfMergePhase = this;
        GameManager._instance.AddSlotDataToSave(droppedColl.gameObject.transform.position, droppedColl.gameObject.GetComponent<Slot>().activeOrb); //Test Save

    } // OnMouseDown()

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }

    } // OnMouseDrag()

    private void OnMouseUp()
    {
        isDragging = false;
        Vector2 mousePoint = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Collider2D droppedColl = Physics2D.OverlapBox(mousePoint, dropCheckArea, 0f, 1 << LayerMask.NameToLayer("Slot"));

        if (droppedColl == null)
        {
            transform.position = previousPosition;
            Collider2D prevDroppedColl1 = Physics2D.OverlapBox(previousPosition, dropCheckArea, 0f, 1 << LayerMask.NameToLayer("Slot"));
            prevDroppedColl1.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
            prevDroppedColl1.gameObject.GetComponent<Slot>().activeOrb = this;
            GameManager._instance.heldOrbEndOfMergePhase = null;
            GameManager._instance.AddSlotDataToSave(prevDroppedColl1.gameObject.transform.position, prevDroppedColl1.gameObject.GetComponent<Slot>().activeOrb); //Test Save
        }

        if (droppedColl != null && droppedColl.gameObject.CompareTag("Slot"))
        {
            switch (droppedColl.gameObject.GetComponent<Slot>().slotType)
            {
                case Slot.SlotType.Empty:

                    Vector3 tmpPos1 = new Vector3(droppedColl.gameObject.transform.position.x, droppedColl.gameObject.transform.position.y, 0f);
                    transform.position = tmpPos1;
                    previousPosition = transform.position;
                    droppedColl.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                    droppedColl.gameObject.GetComponent<Slot>().activeOrb = this;
                    GameManager._instance.heldOrbEndOfMergePhase = null;
                    GameManager._instance.AddSlotDataToSave(droppedColl.gameObject.transform.position, droppedColl.gameObject.GetComponent<Slot>().activeOrb); //Test Save
                    AudioManager._instance.PlaySFX(AudioManager._instance.crystalDropSFXindex);

                    break;

                case Slot.SlotType.Active:

                    if (droppedColl.gameObject.GetComponent<Slot>().CheckDroppedOrbSimilarity(this.gameObject))
                    {
                        // merge başarılı 
                        GameManager._instance.heldOrbEndOfMergePhase = null;

                        Vector3 tmpPoss = new Vector3(droppedColl.gameObject.GetComponent<Slot>().transform.position.x, droppedColl.gameObject.GetComponent<Slot>().transform.position.y, 0f);
                        transform.position = tmpPoss;
                        anim.SetTrigger("LeftMerged");

                        if (createdFirstProductionVFXObj != null || createdFullProductionVFXObj != null)
                        {
                            Destroy(createdFirstProductionVFXObj);
                            Destroy(createdFullProductionVFXObj);
                            createdFirstProductionVFXObj = null;
                            createdFullProductionVFXObj = null;
                        }
                    }
                    else
                    {
                        Collider2D droppedCrystalColl = null;
                        Collider2D prevDroppedColl1 = Physics2D.OverlapBox(previousPosition, dropCheckArea, 0f, 1 << LayerMask.NameToLayer("Slot"));
                        droppedCrystalColl = droppedColl.GetComponent<Slot>().activeOrb.gameObject.GetComponent<BoxCollider2D>();
                        Vector3 tmpPosCur = transform.position;
                        Vector3 tmpPos2 = new Vector3(droppedColl.gameObject.transform.position.x, droppedColl.gameObject.transform.position.y, 0f);
                        transform.position = tmpPos2;            
                        previousPosition = transform.position;  
                        droppedColl.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                        droppedColl.gameObject.GetComponent<Slot>().activeOrb = this;
                        Vector3 tmpPos3 = new Vector3(prevDroppedColl1.gameObject.transform.position.x, prevDroppedColl1.gameObject.transform.position.y, 0f);
                        droppedCrystalColl.gameObject.transform.position = tmpPos3;
                        droppedCrystalColl.gameObject.GetComponent<OrbClass>().previousPosition = tmpPos3;                  
                        prevDroppedColl1.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                        prevDroppedColl1.gameObject.GetComponent<Slot>().activeOrb = droppedCrystalColl.gameObject.GetComponent<OrbClass>();
                        GameManager._instance.heldOrbEndOfMergePhase = null;
                        GameManager._instance.AddSlotDataToSave(prevDroppedColl1.gameObject.transform.position, prevDroppedColl1.gameObject.GetComponent<Slot>().activeOrb); //Test Save
                        GameManager._instance.AddSlotDataToSave(droppedColl.gameObject.transform.position, droppedColl.gameObject.GetComponent<Slot>().activeOrb); //Test Save
                        AudioManager._instance.PlaySFX(AudioManager._instance.crystalDropSFXindex);
                    }

                    break;

                default:
                    break;

            } // droppedSlotType

        } // if null

    } // OnMouseUp()

    public IEnumerator HarvestCrystal()
    {
        if (canHarvest == true)
        {
            canHarvest = false;

            if (productionStorage.Count > 0)
            {
                if (productionStorage != null)
                {

                    for (int i = 0; i < productionStorage.Count; i++)
                    {
                        ProjectileStorage._instance.AddProjectileToStorage(orbType, productionStorage[i]);

                        if (productionStorage.Count < GameManager._instance.HarvestedProjectileShownGap)
                        {
                            switch (orbType)
                            {
                                case OrbType.Fire:
                                    GameObject tmp1 = Instantiate(GameManager._instance.projectileList[0], transform.position, Quaternion.identity);
                                    tmp1.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                    break;
                                case OrbType.Water:
                                    GameObject tmp2 = Instantiate(GameManager._instance.projectileList[1], transform.position, Quaternion.identity);
                                    tmp2.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                    break;
                                case OrbType.Air:
                                    GameObject tmp3 = Instantiate(GameManager._instance.projectileList[2], transform.position, Quaternion.identity);
                                    tmp3.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                    break;
                                case OrbType.Earth:
                                    GameObject tmp4 = Instantiate(GameManager._instance.projectileList[3], transform.position, Quaternion.identity);
                                    tmp4.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                    break;
                                case OrbType.Void:
                                    GameObject tmp5 = Instantiate(GameManager._instance.projectileList[4], transform.position, Quaternion.identity);
                                    tmp5.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                    break;
                                case OrbType.Poison:
                                    GameObject tmp6 = Instantiate(GameManager._instance.projectileList[5], transform.position, Quaternion.identity);
                                    tmp6.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                    break;
                                case OrbType.Lightning:
                                    GameObject tmp7 = Instantiate(GameManager._instance.projectileList[6], transform.position, Quaternion.identity);
                                    tmp7.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                    break;
                                default:
                                    break;

                            } // switch (orbType)
                        }
                        else if (canSpawnCopy)
                        {
                            canSpawnCopy = false;

                            for (int k = 0; k < GameManager._instance.HarvestedProjectileShownGap; k++)
                            {
                                switch (orbType)
                                {
                                    case OrbType.Fire:
                                        GameObject tmp1 = Instantiate(GameManager._instance.projectileList[0], transform.position, Quaternion.identity);
                                        tmp1.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                        break;
                                    case OrbType.Water:
                                        GameObject tmp2 = Instantiate(GameManager._instance.projectileList[1], transform.position, Quaternion.identity);
                                        tmp2.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                        break;
                                    case OrbType.Air:
                                        GameObject tmp3 = Instantiate(GameManager._instance.projectileList[2], transform.position, Quaternion.identity);
                                        tmp3.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                        break;
                                    case OrbType.Earth:
                                        GameObject tmp4 = Instantiate(GameManager._instance.projectileList[3], transform.position, Quaternion.identity);
                                        tmp4.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                        break;
                                    case OrbType.Void:
                                        GameObject tmp5 = Instantiate(GameManager._instance.projectileList[4], transform.position, Quaternion.identity);
                                        tmp5.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                        break;
                                    case OrbType.Poison:
                                        GameObject tmp6 = Instantiate(GameManager._instance.projectileList[5], transform.position, Quaternion.identity);
                                        tmp6.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                        break;
                                    case OrbType.Lightning:
                                        GameObject tmp7 = Instantiate(GameManager._instance.projectileList[6], transform.position, Quaternion.identity);
                                        tmp7.GetComponent<OrbProjectile>().anim.SetTrigger("Harvested");
                                        break;
                                    default:
                                        break;

                                } // switch (orbType)

                                yield return new WaitForSeconds(0.05f);

                            } // for

                        } // else 

                        //ui da belki gösteririz üretilen orb kapasitesini güncellenmesi

                        yield return new WaitForSeconds(0.05f);

                    } // en dış for

                    yield return new WaitForSeconds(0.05f);
                    productionStorage.Clear();

                } // nullcheck

                canHarvest = true;
                canSpawnCopy = true;

                if (mergedToDestroy)
                {
                    if (gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                canHarvest = true;
                canSpawnCopy = true;
                //Debug.Log("hasat edecek malzeme yok");
            }
        }
        else
        {
            //Debug.Log("hasat edilemez");
        }

    } // HarvestCrystal()

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CrystalHarvester"))
        {
            if (createdFirstProductionVFXObj != null || createdFullProductionVFXObj != null)
            {
                Destroy(createdFirstProductionVFXObj);
                Destroy(createdFullProductionVFXObj);
                createdFirstProductionVFXObj = null;
                createdFullProductionVFXObj = null;

            }
            if (productionStorage.Count == productionCapacity) // üretim zamanını sıfırlamak için
            {
                currentProductionTime = productionTime;
            }
            StartCoroutine(HarvestCrystal());
        }

    } // OnTriggerEnter2D()

    public void MergeAnimationEndEventTrigger()
    {
        AudioManager._instance.PlaySFX(AudioManager._instance.crystalMergeSFXindex);

        if (productionStorage.Count <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                //Debug.Log("transform.Child(0) : " + transform.GetChild(0).name);
                //Debug.Log("transform.Child(0).Child(i) : " + transform.GetChild(0).GetChild(i).name);
                //Debug.Log("transform.Child(0).Child(i).transform.rotation.y : " + transform.GetChild(0).GetChild(i).transform.rotation.y.ToString());
                if (transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                }
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }

            mergedToDestroy = true;
            StartCoroutine(HarvestCrystal());
        }

    } // MergeAnimationEndEventTrigger()

    public void MergeAnimationEndForVFXEvent()
    {
        switch (orbType)
        {
            case OrbType.Fire:
                VFXManager._instance.SpawnFireOrbMergeVFX(transform);
                break;
            case OrbType.Water:
                VFXManager._instance.SpawnWaterOrbMergeVFX(transform);
                break;
            case OrbType.Air:
                VFXManager._instance.SpawnAirOrbMergeVFX(transform);
                break;
            case OrbType.Earth:
                VFXManager._instance.SpawnEarthOrbMergeVFX(transform);
                break;
            case OrbType.Void:
                VFXManager._instance.SpawnVoidOrbMergeVFX(transform);
                break;
            case OrbType.Poison:
                VFXManager._instance.SpawnPoisonOrbMergeVFX(transform);
                break;
            case OrbType.Lightning:
                VFXManager._instance.SpawnLightningOrbMergeVFX(transform);
                break;
            default:
                break;
        }

    } // MergeAnimationEndForVFXEvent()

} // class
