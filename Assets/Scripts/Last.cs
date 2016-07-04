using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Scripts
{
    public class Last : MonoBehaviour
    {
        public GameObject toggleGroupPanel;
        public GridLayoutGroup gridLayoutGroup;

        private int columnCount;
        private Block[,] blockArray;
        private Block b1;
        private Block b2;

        public Text answerText;

        // Use this for initialization
        void Start()
        {
            columnCount = gridLayoutGroup.constraintCount;

            blockArray = new Block[columnCount, columnCount];

            ResetBoard(5);
        }

		public void ResetBoard(int m)
        {

            var i = 0;
            foreach (var block in toggleGroupPanel.GetComponentsInChildren<Block>())
            {
                //block.toggle.isOn = false;

                var r = i / columnCount;
                var c = i % columnCount;
                blockArray[r, c] = block;

                i++;

                block.v = i;
                block.r = r;
                block.c = c;
                block.empty = false;
                block.toggle.isOn = false;
                block.merged = false;
            }

            answerText.text = FillRandom(m).ToString();
        }

		private int FillRandom(int m)
        {
            var rnd = new Random();

            //var lastLine = 3;//rnd.Next(0, columnCount);

            var answer = 0;
            
            for (var i = 0; i < columnCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                {
                    if (i == 0 || i == columnCount - 1 || j == 0 || j == columnCount - 1)
                    {
                        blockArray[j, i].empty = true;
                    }
                    else
                    {
                        blockArray[j, i].empty = false;
                        blockArray[j, i].v = rnd.Next(1, m + 1);
                    }
                }
            }

            return rnd.Next(10,20);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnToggleChange()
        {
            if (blockArray == null)
            {
                return;
            }

            var checkedBlockArray = new Block[columnCount * columnCount];
            var checkedBlockArrayLength = 0;
            foreach (var block in toggleGroupPanel.GetComponentsInChildren<Block>())
            {
                if (block.toggle.isOn)
                {
                    checkedBlockArray[checkedBlockArrayLength] = block;
                    checkedBlockArrayLength++;
                }
            }

            if (checkedBlockArrayLength == 0)
            {
                b1 = null;
                b2 = null;
                return;
            }

            if (checkedBlockArrayLength == 1)
            {
                b1 = checkedBlockArray[0];
                return;
            }

            if (checkedBlockArrayLength == 2)
            {
                if (b1 == null && b2 == null)
                {
                    b1 = checkedBlockArray[0];
                    b2 = checkedBlockArray[1];
                }
                else
                {
                    b2 = b1 == checkedBlockArray[0] ? checkedBlockArray[1] : checkedBlockArray[0];
                }
            }
            else
            {
                return;
            }

            if (b1 == null || b2 == null)
            {
                throw new Exception("Unknown error");
            }

            if (b1.v == b2.v)
            {
                // 두 블럭이 서로 만날 수 있는지 확인하고 가능하면 둘 다 삭제한다.

                var blockMemo = new HashSet<Block>();

                if (CanMeet(b1, b2, blockMemo))
                {
                    CleanBlock(b1);
                    CleanBlock(b2);
                }
            }
            else if (b1.merged && b2.merged)
            {
                // 두 블럭이 인접했는지를 보고, a --> a 방향으로 합친다.
                var blockMemo = new HashSet<Block>();

                if (CanMeet(b1, b2, blockMemo))
                {
                    MoveAbs(b1, b2);
                }
            }
            else
            {
                // 두 블럭이 인접했는지를 보고, a --> a 방향으로 합친다.
                var blockMemo = new HashSet<Block>();

                if (CanMeet(b1, b2, blockMemo))
                {
                    MoveAdd(b1, b2);
                    b2.merged = true;
                }
            }

            b1.toggle.isOn = false;
            b2.toggle.isOn = false;

            //Collapse();
        }

        private bool CanMeet(Block a, Block b, HashSet<Block> blockMemo)
        {
            blockMemo.Add(a);

            foreach (var n in GetNeighbors(a))
            {
                if (n == b)
                {
                    return true;
                }

                if (n.empty == false)
                {
                    continue;
                }
                
                if (blockMemo.Contains(n))
                {
                    continue;
                }

                if (CanMeet(n, b, blockMemo))
                {
                    return true;
                }
            }

            return false;
        }

        private Block[] GetNeighbors(Block a)
        {
            var blockList = new List<Block>();

            if (a.r >= 1)
            {
                blockList.Add(blockArray[a.r - 1, a.c]);
            }

            if (a.r < columnCount - 1)
            {
                blockList.Add(blockArray[a.r + 1, a.c]);
            }

            if (a.c >= 1)
            {
                blockList.Add(blockArray[a.r, a.c - 1]);
            }

            if (a.c < columnCount - 1)
            {
                blockList.Add(blockArray[a.r, a.c + 1]);
            }

            return blockList.ToArray();
        }

        private void Collapse()
        {
            for (var c = 0; c < columnCount; c++)
            {
                CollapseColumn(c);
            }
        }

        public void CollapseColumn(int c)
        {
            for (var r = columnCount - 1; r >= 0; r--)
            {
                if (blockArray[r, c].empty)
                {
                    continue;
                }

                for (var rr = columnCount - 1; rr >= r + 1; rr--)
                {
                    if (blockArray[rr, c].empty)
                    {
                        Move(blockArray[r, c], blockArray[rr, c]);
                        break;
                    }
                }
            }
        }

        private bool IsNeighbor(Block a, Block b)
        {
            return (Mathf.Abs(a.r - b.r) + Mathf.Abs(a.c - b.c)) <= 1;
        }

        public void MoveAdd(Block a, Block b)
        {
            CleanBlock(a);
            b.v += a.v;
        }

        public void MoveAbs(Block a, Block b)
        {
            CleanBlock(a);
            b.v = Mathf.Abs(a.v - b.v);
        }

        public void Move(Block a, Block b)
        {
            CleanBlock(a);
            b.v = a.v;
            b.empty = false;
        }

        public void CleanBlock(Block a)
        {
            a.empty = true;
        }
    }

}
