using System.Collections;
using UnityEngine;
using Google.XR.ARCoreExtensions;

public class VpsAvailabilityChecker : MonoBehaviour
{
    public double latitude = 37.4219999;   // Example: Googleplex
    public double longitude = -122.0840575;

    private VpsAvailabilityPromise _promise;

    void Start()
    {
        // Kick off VPS availability check
        _promise = AREarthManager.CheckVpsAvailabilityAsync(latitude, longitude);
        StartCoroutine(CheckPromiseState());
    }

    IEnumerator CheckPromiseState()
    {
        while (_promise.State == PromiseState.Pending)
        {
            Debug.Log("VPS availability check still pending...");
            yield return null; // wait a frame
        }

        if (_promise.State == PromiseState.Done)
        {
            VpsAvailability availability = _promise.Result;
            Debug.Log("VPS availability: " + availability);

            if (availability == VpsAvailability.Available)
            {
                // VPS available — show "Enter AR" button
                Debug.Log("✅ VPS available, enable AR entry UI.");
            }
            else
            {
                Debug.Log("❌ VPS not available at this location.");
            }
        }
        else if (_promise.State == PromiseState.Cancelled)
        {
            Debug.Log("VPS check was cancelled.");
        }
    }

    public void CancelCheck()
    {
        if (_promise != null && _promise.State == PromiseState.Pending)
        {
            _promise.Cancel();
            Debug.Log("Attempted to cancel VPS check.");
        }
    }
}
