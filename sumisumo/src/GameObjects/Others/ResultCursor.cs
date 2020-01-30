using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class ResultCursor
    {
        public bool moveflag = false; // 最初の位置から移動しているか
        public int posX = 210; // カーソルのX座標
        public int posY = 575; // カーソルのY座標
        private int flashTimar;
        private float moveInterval = 5.0f;
        private float moveTimer = 0.0f;
        public void Update()
        {
            flashTimar++;
            moveTimer ++;
            
            // 右ボタン入力でカーソルを下に移動
            if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT) && !moveflag && moveTimer > moveInterval)
            {
                moveTimer = 0;
                flashTimar = 0;
                moveflag = true;
                posX = 700;
                Sound.SePlay(Sound.se_switch);
            }
            // 左ボタン入力でカーソルを上に移動
            else if(Input.GetButtonDown(DX.PAD_INPUT_LEFT) && moveflag && moveTimer > moveInterval)
            {
                moveTimer = 0;
                flashTimar = 0;
                moveflag = false;
                posX = 210;
                Sound.SePlay(Sound.se_switch);
            }
        }
        public void Draw()
        {
            if (flashTimar / 16 % 4 == 0)
            DX.DrawGraph(posX, posY, Image.cursor, 1);
        }
    }
}
