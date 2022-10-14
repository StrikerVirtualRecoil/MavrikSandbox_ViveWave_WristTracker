using Newtonsoft.Json;
using StrikerLink.Unity.Runtime.Core;
using StrikerLink.Unity.Runtime.HapticEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StrikerLink.Unity.Dev
{
    public class HapticDemo : MonoBehaviour
    {
        public StrikerDevice device;
        public TMP_Dropdown triggerDropdown;
        public TMP_Dropdown leftButtonDropdown;
        public TMP_Dropdown rightButtonDropdown;
        public TMP_Dropdown menuTopDropdown;
        public TMP_Dropdown menuBottomDropdown;
        public TMP_Dropdown reloadDropdown;

        public TMP_Dropdown frontSlideDropdown;

        public HapticLibraryAsset library;

        float lastBarGripValue = -1f;

        private void Awake()
        {
            BasicHapticLibraryData data = JsonConvert.DeserializeObject<BasicHapticLibraryData>(library.json);

            ApplyLibrary(triggerDropdown, data);
            ApplyLibrary(leftButtonDropdown, data);
            ApplyLibrary(rightButtonDropdown, data);
            ApplyLibrary(menuTopDropdown, data);
            ApplyLibrary(menuBottomDropdown, data);
            ApplyLibrary(reloadDropdown, data);
            ApplyLibrary(frontSlideDropdown, data);

        }

        void ApplyLibrary(TMP_Dropdown dropdown, BasicHapticLibraryData data)
        {
            dropdown.options = data.Effects.Select(x => new TMP_Dropdown.OptionData(x.EffectId)).ToList();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (device.GetTriggerDown())
                device.FireHaptic(library.libraryKey, triggerDropdown.options[triggerDropdown.value].text);

            if (device.GetButtonDown(Shared.Devices.DeviceFeatures.DeviceButton.SideLeft))
                device.FireHaptic(library.libraryKey, leftButtonDropdown.options[leftButtonDropdown.value].text);

            if (device.GetButtonDown(Shared.Devices.DeviceFeatures.DeviceButton.SideRight))
                device.FireHaptic(library.libraryKey, rightButtonDropdown.options[rightButtonDropdown.value].text);

            if (device.GetButtonDown(Shared.Devices.DeviceFeatures.DeviceButton.MenuTop))
                device.FireHaptic(library.libraryKey, menuTopDropdown.options[menuTopDropdown.value].text);

            if (device.GetButtonDown(Shared.Devices.DeviceFeatures.DeviceButton.MenuBottom))
                device.FireHaptic(library.libraryKey, menuBottomDropdown.options[menuBottomDropdown.value].text);

            if(device.GetSensorDown(Shared.Devices.DeviceFeatures.DeviceSensor.ReloadTouched))
                device.FireHaptic(library.libraryKey, reloadDropdown.options[reloadDropdown.value].text);

            if (device.GetSensorDown(Shared.Devices.DeviceFeatures.DeviceSensor.ForwardBarGripTouched))
            {
                float val = device.GetAxis(Shared.Devices.DeviceFeatures.DeviceAxis.ForwardBarGripPosition);
                if (Mathf.Abs(lastBarGripValue - val) > 0.1f)
                {
                    device.FireHaptic(library.libraryKey, frontSlideDropdown.options[frontSlideDropdown.value].text, device.GetAxis(Shared.Devices.DeviceFeatures.DeviceAxis.ForwardBarGripPosition));
                    lastBarGripValue = val;
                }
            }
            else
                lastBarGripValue = -1f;
        }
    }
}