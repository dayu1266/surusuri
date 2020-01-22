using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class ResultCursor
    {
        public bool moveflag = false; // 最初の位置から移動しているか
        private int posX = 210; // カーソルのX座標
        private int posY = 575; // カーソルのY座標
        private int flashTimar;
        public void Update()
        {
            flashTimar++;
            
            if (Game.GetStageLevel() == 3)
            {
                posX = 450;
                moveflag = true;
            }
            // 右ボタン入力でカーソルを下に移動
            else if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT) && !moveflag)
            {
                flashTimar = 0;
                moveflag = true;
                posX = 700;
            }
            // 左ボタン入力でカーソルを上に移動
            else if(Input.GetButtonDown(DX.PAD_INPUT_LEFT) && moveflag)
            {
                flashTimar = 0;
                moveflag = false;
                posX = 210;
            }
        }
        public void Draw()
        {
            if (flashTimar / 16 % 4 == 0)
            DX.DrawGraph(posX, posY, Image.cursor, 1);
        }
    }
}
