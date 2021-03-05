using System;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Entities;

public partial class ObjectsGrid
{
    [Serializable]
    private class ObjDictionary : Dictionary<Vector2Int, GridObject>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<Vector2Int> keys = new List<Vector2Int>();

        [SerializeField]
        private List<GridObject> values = new List<GridObject>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<Vector2Int, GridObject> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new Exception($"there are {keys.Count} keys and {values.Count} values after deserialization. " +
                                    $"Make sure that both key and value types are serializable.");

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }
    }
}
