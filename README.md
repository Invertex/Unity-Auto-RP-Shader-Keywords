Automatically sets global shader keywords depending on whether URP or HDRP is currently installed as well as which is currently active, allowing you to properly conditionally compile your shaders for more than one pipeline and also have them swap shader variants automatically at runtime.

In Unity 2021 this script will automatically detected when pipeline has been removed/swapped and update its values. If you are on older version, then you must either reimport the script or restart Unity for it to be able to apply its changes, since there does not appear to be any sort of fired events when pipeline is changed in older versions.

<h3> In most cases you should not need this script, but instead can simply use this to avoid issue with your shader being used in multiple pipelines: https://docs.unity3d.com/2021.2/Documentation/Manual/SL-PackageRequirements.html </h3>
