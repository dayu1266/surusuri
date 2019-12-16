using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace sumisumo
{
    public class DownStairs : GameObject
    {
        public DownStairs(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 100;
            imageHeight = 36;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 0;

        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, Image.downStairs);
        }

        public override void OnCollision(GameObject other)
        {
        }


    }
}
