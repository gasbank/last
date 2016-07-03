using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Block : MonoBehaviour
    {
        public Toggle toggle;
        public Text text;
        public int r;
        public int c;
        public bool merged
        {
            get {return _merged;}
            set {
                _merged = value;
                var cb = new ColorBlock();
                cb.normalColor = _merged ? Color.red : Color.white;
                cb.highlightedColor = cb.normalColor;
                cb.disabledColor = cb.normalColor;
                cb.pressedColor = cb.normalColor;
                cb.colorMultiplier = 1;
                toggle.colors = cb;

                Debug.Log("Merged set to " + value);
            }
        }
        public bool _merged;

        public int v
        {
            get { return _v; }
            set
            {
                _v = value;
                
                text.text = value.ToString();
            }
        }
        private int _v;

        public bool empty
        {
            get { return _empty; }
            set
            {
                _empty = value;
                toggle.gameObject.SetActive(!value);
            }
        }
        private bool _empty;

    }
}
