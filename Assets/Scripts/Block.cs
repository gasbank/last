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
