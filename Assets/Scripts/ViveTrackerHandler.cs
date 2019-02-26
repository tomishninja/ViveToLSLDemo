using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

public class ViveTrackerHandler : MonoBehaviour
{
    public SteamVR_TrackedObject[] trackers;

    /// <summary>
    /// This may not work tom
    /// </summary>
    public void Start()
    {
        AutoAssign();
    }

    public void AutoAssign()
    {
        int currentTrackerIndex = 0;
        foreach (SteamVR_TrackedObject.EIndex eIndex in Enum.GetValues(typeof(SteamVR_TrackedObject.EIndex)).Cast<SteamVR_TrackedObject.EIndex>())
        {
            string modelName = GetDeviceName(eIndex);
            if (modelName != "")
            {
                if (modelName.Contains("tracker"))
                {
                    // This is most likely the ID of a tracker
                    if (currentTrackerIndex < trackers.Length)
                    {
                        trackers[currentTrackerIndex].index = eIndex;
                        Debug.Log(string.Format("Assigned {0} to {1}", modelName, trackers[currentTrackerIndex].gameObject.name));
                    }
                    else
                    {
                        Debug.Log(
                            string.Format("[WARNING] More than {0} trackers found, tracker {1} ({2}) will not be assigned",
                                trackers.Length,
                                (uint)eIndex,
                                modelName));
                    }

                    currentTrackerIndex += 1;
                }
                else
                {
                    Debug.Log(string.Format("Not assigning {0} to a controller or tracker", modelName));
                }
            }
        }
    }

    string GetDeviceName(SteamVR_TrackedObject.EIndex index)
    {
        var system = OpenVR.System;
        if (system == null)
            return "";

        var error = ETrackedPropertyError.TrackedProp_Success;
        var capacity = system.GetStringTrackedDeviceProperty((uint)index, ETrackedDeviceProperty.Prop_RenderModelName_String, null, 0, ref error);
        if (capacity <= 1)
        {
            return "";
        }

        var buffer = new System.Text.StringBuilder((int)capacity);
        system.GetStringTrackedDeviceProperty((uint)index, ETrackedDeviceProperty.Prop_RenderModelName_String, buffer, capacity, ref error);

        string renderModelName = "";
        var s = buffer.ToString();
        if (renderModelName != s)
        {
            renderModelName = s;
        }

        return renderModelName;
    }

    public void SetTrackerEIndex(int trackerIndex, SteamVR_TrackedObject.EIndex eIndex)
    {
        trackers[trackerIndex].SetDeviceIndex((int)eIndex);
    }

    public void SetTrackerEIndex(SteamVR_TrackedObject tracker, SteamVR_TrackedObject.EIndex eIndex)
    {
        tracker.SetDeviceIndex((int)eIndex);
    }
}
