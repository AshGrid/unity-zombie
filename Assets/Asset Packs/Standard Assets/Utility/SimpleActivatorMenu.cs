using System;
using UnityEngine;
using UnityEngine.UI;  // Import UI namespace

namespace UnityStandardAssets.Utility
{
    public class SimpleActivatorMenu : MonoBehaviour
    {
        // Reference to the UI Text component
        public Text camSwitchButton;
        public GameObject[] objects;

        private int m_CurrentActiveObject;

        private void OnEnable()
        {
            // Start with the first object active
            m_CurrentActiveObject = 0;
            UpdateButtonText();
        }

        public void NextCamera()
        {
            // Determine the next active object in the list
            int nextActiveObject = (m_CurrentActiveObject + 1) % objects.Length;

            // Activate only the selected object
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(i == nextActiveObject);
            }

            // Update the current active object index
            m_CurrentActiveObject = nextActiveObject;
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            // Update the button's text to reflect the active object name
            if (camSwitchButton != null)
            {
                camSwitchButton.text = objects[m_CurrentActiveObject].name;
            }
        }
    }
}
