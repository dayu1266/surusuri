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
        private int posX = 180; // カーソルのX座標
        private int posY = 575; // カーソルのY座標
        public void Update()
        {
            // 下ボタン入力でカーソルを下に移動
            if(Input.GetButtonDown(DX.PAD_INPUT_RIGHT) && !moveflag)
            {
                moveflag = true;
                posX = 720;
            }
            // 上ボタン入力でカーソルを上に移動
            else if(Input.GetButtonDown(DX.PAD_INPUT_LEFT) && moveflag)
            {
                moveflag = false;
                posX = 180;
            }
        }
        public void Draw()
        {
            DX.DrawGraph(posX, posY, Image.cursor, 1);
        }
    }
}
