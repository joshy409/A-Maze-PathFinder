using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    // represents a ame tile
    public class Tile : MonoBehaviour
    {
        public TileType type;
        public Index idx;
        [System.NonSerialized]
        public GameObject obj;
        [System.NonSerialized]
        public MapGenerator data;

        public static Dictionary<TileType, Color> colors;

        Material mat;

        void Start()
        {
            mat = GetComponent<Renderer>().material;
        }

        public void Set(TileType _type, Index _idx, GameObject _obj, MapGenerator _data)
        {
            type = _type;
            idx = _idx;
            obj = _obj;
            data = _data;

            if(null == data) { Debug.LogError("data is null"); }

            mat = obj.GetComponent<Renderer>().material;
        }

        private void OnMouseDown()
        {
            // change type
            Color c;
            type = data.currentSelection;
            colors.TryGetValue(type, out c);
            mat.color = c;
        }

        private void OnMouseEnter()
        {
            if (Input.GetMouseButton(0)) { OnMouseDown(); } // drag select
            else
            {
                Color c = mat.color;
                c.a = 255;
                mat.color = c;
            }           
        }

        private void OnMouseExit()
        {
            // un highlight
        }
    }
}
