using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DxLibDLL;

namespace sumisumo
{
    class PlayerUi : GameObject
    {
        const float moveAmoutY = 120.0f; // Y方向の移動量
        const float velocityY = 3.0f;    // Y方向の移動速度

        float totalAmout;
        int getMoney;

        public PlayerUi(PlayScene playScene, Vector2 pos, int getMoney):base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;
            this.getMoney = getMoney;

            totalAmout = 0;
        }

        public override void Update()
        {
            pos.Y -= velocityY;
            totalAmout += velocityY;
            if (totalAmout > moveAmoutY)
                isDead = true;
        }
        public override void Draw()
        {
            Camera.DrawGraph(pos.X - 16, pos.Y, Image.number[10]);
            string RepresentsGold = getMoney.ToString();
            for (int i = 0; i < RepresentsGold.Length; i++)
            {
                Camera.DrawGraph(pos.X + (i * 16), pos.Y, Image.number[RepresentsGold[i] - '0']);
            }
        }
        public override void OnCollision(GameObject other)
        {
        }

    }
}
