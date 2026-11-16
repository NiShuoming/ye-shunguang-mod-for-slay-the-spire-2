using BaseLib.Audio;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Other;

namespace YeShunguang
{
    public static class VoiceManager
    {
        static string voicePath = "res://YeShunguang/voice/";

        static string attackVfx = "打击音效";

        static string attackVoice1 = "攻";
        
        static int attackVoice1N = 3;

        static string attackVoice2 = "白攻";

        static int attackVoice2N = 3;

        static string MindVoice = "明心境";

        static int MindVoiceN = 3;

        static string BigSkillVoice = "大招";

        static int BigSkillVoiceN = 3;

        static string CharacterSelectionVoice = "入场";

        static int CharacterSelectionVoiceN = 2;

        private static readonly Random _rng = new Random();

        public static int GetRandomN(int n)
        {
            return _rng.Next(n);  
        }

        public static string GetLanguage()
        {
            return ZZZConfig.VoiceLanguage == ZZZConfig.Language.Chinese ? "中" : "日";
        }

        public static void PlayBigSkillVoice()
        {
            if (!ZZZConfig.CharacterVoice) return;

            int n = GetRandomN(BigSkillVoiceN)+1;
            
            ModAudio.PlaySound(new ModSound(
                voicePath+BigSkillVoice + n + GetLanguage()+".mp3",
                ModAudio.SoundType.Sfx), 7);
        }

        public static void PlayCharacterSelectionVoice()
        {
            if (!ZZZConfig.CharacterVoice) return;

            int n = GetRandomN(CharacterSelectionVoiceN)+1;

            ModAudio.PlaySound(new ModSound(
                voicePath + CharacterSelectionVoice + n + GetLanguage() + ".mp3",
                ModAudio.SoundType.Sfx), 10);
        }

        public static void PlayAttackVfx()
        {           
                ModAudio.PlaySound(new ModSound(
                voicePath + attackVfx + ".mp3",
                ModAudio.SoundType.Sfx));
        }

        public static void PlayAttackVoice(bool haveMind)
        {
            if (!ZZZConfig.CharacterVoice) return;

            if (!haveMind) PlayAttackVoice1();
            else PlayAttackVoice2();
        }

        private static void PlayAttackVoice1()
        {
            int n = GetRandomN(attackVoice1N+2)+1;
            if (n <= attackVoice1N)
                ModAudio.PlaySound(new ModSound(
                voicePath + attackVoice1 + n + ".mp3",
                ModAudio.SoundType.Sfx), 7);
        }

        private static void PlayAttackVoice2()
        {
            int n = GetRandomN(attackVoice2N+2)+1;
            if (n <= attackVoice2N)
                ModAudio.PlaySound(new ModSound(
                voicePath + attackVoice2 + n  + ".mp3",
                ModAudio.SoundType.Sfx), 7);
        }

        public static void PlayMindVoice()
        {
            if (!ZZZConfig.CharacterVoice) return;

            int n = GetRandomN(MindVoiceN)+1;
            ModAudio.PlaySound(new ModSound(
                voicePath + MindVoice + n + GetLanguage() + ".mp3",
                ModAudio.SoundType.Sfx), 7);
        }
    }
}
