using System.Numerics;

namespace sumisumo
{
    public class DownStairs : GameObject
    {
        bool use;
        public DownStairs(PlayScene playScene, Vector2 pos) : base(playScene)
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
            use = false;
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, Image.downStairs);
            if (use)
            {
                Camera.DrawGraph(pos.X + 128, pos.Y - 16, Image.gimmicksign);
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                use = true;
            }
        }


    }
}
