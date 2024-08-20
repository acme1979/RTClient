using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WorkStation
{
    public class SoundPlayerHelp
    {
        #region 成员变量
        string MusicPath = Application.StartupPath + @"\AudioFile\error.wav";
        string PassPath = Application.StartupPath + @"\AudioFile\ok.wav";
        public System.Media.SoundPlayer sndplayer;
        public System.Media.SoundPlayer passPlayer;
        #endregion

        public SoundPlayerHelp()
        {
            sndplayer = new System.Media.SoundPlayer(MusicPath);
            passPlayer = new System.Media.SoundPlayer(PassPath);
        }
    }
}
