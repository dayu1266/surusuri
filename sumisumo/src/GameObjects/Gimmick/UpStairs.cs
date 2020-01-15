using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace sumisumo
{
    public class UpStairs : GameObject
    {
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
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, Image.upStairs);
        }

        public override void OnCollision(GameObject other)
        {
        }
    }
}
