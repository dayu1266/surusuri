using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace sumisumo
{
    class Goal : GameObject
    {
        public Goal(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 96;
            imageHeight = 192;
            hitboxOffsetLeft = -32;
            hitboxOffsetRight = 32;
            hitboxOffsetTop = 26;
            hitboxOffsetBottom = 0;
        }
        public override void Update()
        {
            
        }
        public override void OnCollision(GameObject other)
        {
            
        }

        public override void Draw()
        {
            
        }

    }
}
