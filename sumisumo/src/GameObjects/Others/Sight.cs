using System.Numerics;

namespace sumisumo
{
    // 沼の視野クラス
    public class Sight : GameObject
    {
        // 各オブジェクトの参照
        GameObject obj;
        static GameObject target;

        int imageHandle;
        bool flip;

        public Sight(PlayScene playScene, GameObject gameObject, Vector2 pos) : base(playScene)
        {
            this.pos = pos;
            obj = gameObject;
            this.playScene = playScene;
            flip = false;

            imageHeight = 140;

            // 親により視野の広さを変える（）
            if (typeof(Player) == obj.GetType())
            {
                imageWidth = 130;
                imageHandle = Image.sight;
            }
            else
            {
                imageWidth = 70;
                imageHandle = Image.enemysight;
            }

            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 10;
        }

        public override void Update()
        {
            // 親の向いている方向を取得
            direction = obj.direction;

            // 向いている方向により視界のポジションを変える
            if (direction == Direction.Right)
            {
                pos.X = obj.GetRight();
                pos.Y = obj.GetTop() - 11.0f;
                flip = false;
            }
            if (direction == Direction.Left)
            {
                pos.X = obj.GetLeft() - imageWidth;
                pos.Y = obj.GetTop() - 11.0f;
                flip = true;
            }

            if (obj.isDead)
            {
                isDead = true;
            }

            // プレイヤーが試着室に隠れた場合視界を消す
            if (typeof(Player) == obj.GetType() && (obj as Player).isHiding)
            {
                isDead = true;
            }
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, imageHandle, flip);
        }

        public override void OnCollision(GameObject other)
        {
            // 親がプレイヤーで相手が一般ピーポーなら
            if ((typeof(Player) == obj.GetType()) && other is People)
            {
                target = other;
                if (other.Surizumi == false)
                {
                    obj.suri = true;
                }
                else
                {
                    obj.suri = false;
                }
            }

            // 親がプレイヤーで相手がなら
            if ((typeof(Player) == obj.GetType()) && other is Guardman && playScene.state == PlayScene.State.OnAlert)
            {
                target = other;
                
                (obj as Player).Guardman_isDead = true;
            }

            // 親が警備員で相手がプレイヤーなら
            if ((typeof(Guardman) == obj.GetType()) && other is Player)
            {
                obj.alert = true;
                if (playScene.state == PlayScene.State.OnAlert)
                {
                    obj.find = true;
                }
            }

            // 親が一般ピーポーで相手がプレイヤーなら
            if ((typeof(People) == obj.GetType()) && other is Player)
            {
                target = other;
                People people = obj as People;
                people.see_player = true;
            }
        }

        // 親のSight中に入ったGameObjectを取得する
        static public GameObject GetTarget()
        {
            return target;
        }
    }
}
