using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace sumisumo
{
    public static class Sound
    {
        public static string bgm_nomalBGM;
        public static string bgm_warningBGM;

        public static int se_alarm;
        public static int se_gameClear;
        public static int se_gameOver;
        public static int se_scream_men;
        public static int se_scream_woman;
        public static int se_surinuke;
        public static int se_whistle;

        public static void Load()
        {

            bgm_nomalBGM    = "res/Sound/BGM/nomalBGM.mp3";
            bgm_warningBGM  = "res/Sound/BGM/warningBGM.mp3";

            se_alarm        = DX.LoadSoundMem("res/Sound/SE/alarm.mp3");
            se_gameClear    = DX.LoadSoundMem("res/Sound/SE/gameClear.mp3");
            se_gameOver     = DX.LoadSoundMem("res/Sound/SE/gameOver.mp3");
            se_scream_men   = DX.LoadSoundMem("res/Sound/SE/scream_men.mp3");
            se_scream_woman = DX.LoadSoundMem("res/Sound/SE/scream_woman.mp3");
            se_surinuke     = DX.LoadSoundMem("res/Sound/SE/surusuri.mp3");
            se_whistle      = DX.LoadSoundMem("res/Sound/SE/whistle.mp3");

        }
        public static void BgmPlay(string handle)
        {
            DX.PlayMusic(handle, DX.DX_PLAYTYPE_BACK);
        }

        public static void SePlay(int handle)
        {
            DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_BACK);
        }
    }
}
