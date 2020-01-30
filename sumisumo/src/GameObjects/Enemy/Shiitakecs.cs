using System.Numerics;

namespace sumisumo
{
    public class Shiitake : GameObject
    {
        public Shiitake(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 32;
            imageHeight = 32;
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
            Camera.DrawGraph(pos.X, pos.Y, Image.test_shiitake);
        }

        public override void OnCollision(GameObject other)
        {
        }
    }
}

