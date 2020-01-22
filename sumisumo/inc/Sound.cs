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
        // BGMの宣言
        public static string bgm_nomalBGM;
        public static string bgm_warningBGM;
        public static string bgm_titleBGM;
        public static string bgm_gameclearBGM;
        public static string bgm_gameoverBGM;

        // SEの宣言
        public static int se_alarm;
        public static int se_decision;
        public static int se_gameclear;
        public static int se_scream_men;
        public static int se_scream_woman;
        public static int se_surinuke;
        public static int se_switch;
        public static int se_whistle;

        public static void Load()
        {
            // BGMのロード
            bgm_nomalBGM      = "res/Sound/BGM/nomalBGM.mp3";
            bgm_warningBGM    = "res/Sound/BGM/warningBGM.mp3";
            bgm_titleBGM      = "res/Sound/BGM/titleBGM.mp3";
            bgm_gameclearBGM  = "res/Sound/BGM/gameclearBGM.mp3";
            bgm_gameoverBGM   = "res/Sound/BGM/gameoverBGM.mp3";

            // SEのロード
            se_alarm          = DX.LoadSoundMem("res/Sound/SE/alarm.mp3");
            se_decision       = DX.LoadSoundMem("res/Sound/SE/decision.mp3");
            se_gameclear      = DX.LoadSoundMem("res/Sound/SE/gameclear_se.mp3");
            se_scream_men     = DX.LoadSoundMem("res/Sound/SE/scream_men.mp3");
            se_scream_woman   = DX.LoadSoundMem("res/Sound/SE/scream_woman.mp3");
            se_surinuke       = DX.LoadSoundMem("res/Sound/SE/surinuke.mp3");
            se_switch         = DX.LoadSoundMem("res/Sound/SE/swtch.mp3");
            se_whistle        = DX.LoadSoundMem("res/Sound/SE/whistle.mp3");

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
