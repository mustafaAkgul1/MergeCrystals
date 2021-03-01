using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public enum SlotType
    {
        Empty,
        Active
    }
    public SlotType slotType;
    public OrbClass activeOrb;  

    public bool CheckDroppedOrbSimilarity(GameObject _droppedOrb)
    {
        if (_droppedOrb.GetComponent<OrbClass>().orbType == activeOrb.orbType && _droppedOrb.GetComponent<OrbClass>().orbLevel == activeOrb.orbLevel)
        {
            activeOrb.anim.SetTrigger("RightMerged");

            if (activeOrb.createdFirstProductionVFXObj != null || activeOrb.createdFullProductionVFXObj != null)
            {
                Destroy(activeOrb.createdFirstProductionVFXObj);
                Destroy(activeOrb.createdFullProductionVFXObj);
                activeOrb.createdFirstProductionVFXObj = null;
                activeOrb.createdFullProductionVFXObj = null;
            }

            activeOrb.orbLevel++;
            GameManager._instance.ChangeOrbLevelAfterMerge(activeOrb.orbLevel, activeOrb.orbType, transform);
            return true;
        }
        else
        {
            return false;
        }

    } // CheckDroppedOrbSimilarity()

} // sınıf
