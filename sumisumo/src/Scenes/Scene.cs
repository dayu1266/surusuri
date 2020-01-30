namespace sumisumo
{
    public abstract class Scene
    {
        // シーンで一回のみ呼ばれる
        public abstract void Init();

        // シーンで1フレーム1回呼ばれる（更新）
        public abstract void Update();

        // シーンで1フレーム1回呼ばれる（描画）
        public abstract void Draw();
    }
}
