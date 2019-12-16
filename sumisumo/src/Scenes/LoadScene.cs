using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
