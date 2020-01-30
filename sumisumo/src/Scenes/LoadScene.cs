namespace sumisumo
{
    public class LoadScene : Scene
    {
        public override void Init()
        {
            Image.Load();
            Sound.Load();
            Game.ChangeScene(new TitleScene());
        }

        public override void Update(){}

        public override void Draw(){}
        
    }
}
