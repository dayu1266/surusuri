﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace sumisumo
{
    public class UpStairs : GameObject
    {
        bool use; // プレイヤーが使えるかどうかの判定
        public UpStairs(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 92;
            imageHeight = 192;
            hitboxOffsetLeft = 30;
            hitboxOffsetRight = -48;
            hitboxOffsetTop = 26;
            hitboxOffsetBottom = 0;

            use = false;
        }

        public override void Update()
        {
            use = false;
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, Image.upStairs);
            if (use) // 使える状態ならビックリマークを表示する
            {
                Camera.DrawGraph(pos.X + 128, pos.Y - 16, Image.gimmicksign);
            }
        }

        public override void OnCollision(GameObject other)
        {
            if(other is Player) // プレイヤーにぶつかっていたら
            {
                use = true; // 使える状態にする
            }
        }
    }
}
