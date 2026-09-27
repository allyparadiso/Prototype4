using UnityEngine;

public class SanityZone : MonoBehaviour
{
    public enum ZoneType { SafeZone, DangerZone }
    public ZoneType zoneType;
    public float changeAmount = 15f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SanityManager sanity = other.GetComponent<SanityManager>() ?? FindFirstObjectByType<SanityManager>();

            if (sanity != null)
            {
                if (zoneType == ZoneType.SafeZone)
                {
                    sanity.ChangeSanity(changeAmount);
                    sanity.SetDrainStatus(false);
                }
                else if (zoneType == ZoneType.DangerZone)
                {
                    sanity.ChangeSanity(-changeAmount);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SanityManager sanity = other.GetComponent<SanityManager>() ?? FindFirstObjectByType<SanityManager>();
            if (sanity != null && zoneType == ZoneType.SafeZone) 
            {
                sanity.SetDrainStatus(true);
            }
        }
    }
}
