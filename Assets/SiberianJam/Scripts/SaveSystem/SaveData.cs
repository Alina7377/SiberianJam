using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystemData
{
    [Serializable]
    public class SaveData
    {
        public string SceneName;
        public string LevelName;
        public List<SObjectData> ObjectsData;
    }

    [Serializable]
    public struct SObjectData
    {
        public string SavingObjectID;
        public List<string> StrParameters;
        public List<float> NumberParameters;
        public List<Vector3> VectorPatameters;
        public List<bool> BoolParamters;
        public Quaternion Rotate;
    }
}
