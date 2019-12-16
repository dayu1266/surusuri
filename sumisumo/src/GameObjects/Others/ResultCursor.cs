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
        private int posX = 500; // カーソルのX座標
        private int posY = 300; // カーソルのY座標
        public void Update()
        {
            // 下ボタン入力でカーソルを下に移動
            if(Input.GetButtonDown(DX.PAD_INPUT_DOWN) && !moveflag)
            {
                moveflag = true;
                posY = 450;
            }
            // 上ボタン入力でカーソルを上に移動
            else if(Input.GetButtonDown(DX.PAD_INPUT_UP) && moveflag)
            {
                moveflag = false;
                posY = 300;
            }
        }
        public void Draw()
        {
            DX.DrawGraph(posX, posY, Image.cursor, 0);
        }
    }
}
