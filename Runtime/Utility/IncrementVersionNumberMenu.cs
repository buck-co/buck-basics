#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    public class IncrementVersionNumberMenu
    {
        [MenuItem("Tools/Versioning/Increment Major Version Number", false)]
        static void IncrementMajorVersionNumberFromMenu()
            => IncrementVersionNumber(0);
        
        [MenuItem("Tools/Versioning/Increment Minor Version Number", false)]
        static void IncrementMinorVersionNumberFromMenu()
            => IncrementVersionNumber(1);
        
        [MenuItem("Tools/Versioning/Increment Patch Version Number", false)]
        static void IncrementPatchVersionNumberFromMenu()
            => IncrementVersionNumber(2);

        static void IncrementVersionNumber(int componentIndex)
        {
            // Get the current version string
            string versionNumber = Application.version;
            
            // Split the version string into its components
            string[] versionComponents = versionNumber.Split('.');
            
            // Parse the major, minor, and patch version numbers
            int majorVersion = int.Parse(versionComponents[0]);
            int minorVersion = int.Parse(versionComponents[1]);
            int patchVersion = int.Parse(versionComponents[2]);

            string incrementationType = "";
            
            switch (componentIndex)
            {
                case 0: // Major version
                    majorVersion++;
                    minorVersion = 0;
                    patchVersion = 0;
                    incrementationType = "Major";
                    break;
                case 1: // Minor version
                    minorVersion++;
                    patchVersion = 0;
                    incrementationType = "Minor";
                    break;
                case 2: // Patch version
                    patchVersion++;
                    incrementationType = "Patch";
                    break;
                default:
                    Debug.LogError("Invalid component index for version number increment.");
                    return;
            }
            
            // Construct the new version number
            string newVersionNumber = $"{majorVersion}.{minorVersion}.{patchVersion}";
            
            // Present a confirmation dialog to the user before incrementing the version number
            if (EditorUtility.DisplayDialog($"Increment {incrementationType} Version Number",
                    $"Increment the version number from {Application.version} to {newVersionNumber}?",
                    "Yes", "No"))
            {
                // Update the version number in the Player Settings
                PlayerSettings.bundleVersion = newVersionNumber;
                
                // Log the new version number to the console
                Debug.Log($"Version number incremented to {newVersionNumber}");
            }
        }
    }
}
#endif