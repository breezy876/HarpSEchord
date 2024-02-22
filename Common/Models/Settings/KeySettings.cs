using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Models.Settings
{
    public class KeySettings
    {

        private const int MAPVK_VK_TO_CHAR = 2;

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);



        const int MaxNotes = 37;

        public Key[] KeyMappings { get; set; }

        public static Key[] DefaultKeyMappings { get; }

        public Dictionary<int, char> CharacterMappings { get; }

        public KeySettings()
        {
            this.CharacterMappings = new Dictionary<int, char>();

            this.KeyMappings = new Key[MaxNotes];

            DefaultKeyMappings.CopyTo(this.KeyMappings, 0);

            foreach (var key in KeyMappings)
            {
                int vkCode = KeyInterop.VirtualKeyFromKey(key);
                char ch = (char)MapVirtualKey((uint)vkCode, MAPVK_VK_TO_CHAR);
                CharacterMappings.AddIfNotKeyExists(vkCode, ch);
            }
        }

        static KeySettings()
        {

            //DefaultKeyMappings = new[] {

            //         Key.A,        //C
            //         Key.V,        //C#
            //         Key.S,        //D
            //         Key.D,        //Eb
            //         Key.F,        //E
            //         Key.G,        //F
            //         Key.H,        //F#
            //         Key.J,        //G
            //         Key.K,        //G#
            //         Key.L,        //A
            //         Key.OEM_1,   //Bb
            //         Key.OEM_2,        //B

            //         Key.Q,        //C
            //         Key.K,        //C#
            //         Key.W,        //D
            //         Key.L,        //Eb
            //         Key.R,        //E
            //         Key.W,        //F
            //         Key.Z,        //F#
            //         Key.E,        //G
            //         Key.X,        //G#
            //         Key.R,        //A
            //         Key.C,        //Bb
            //         Key.T,        //B


            //     };



            DefaultKeyMappings = new Key[] {

                     Key.Y,        //C
                     Key.V,        //C#
                     Key.U,        //D
                     Key.B,        //Eb
                     Key.I,        //E
                     Key.O,        //F
                     Key.N,        //F#
                     Key.P,        //G
                     Key.M,        //G#
                     Key.A,        //A
                     Key.OemComma,   //Bb
                     Key.S,        //B

                     Key.D9,        //C
                     Key.K,        //C#
                     Key.D0,        //D
                     Key.L,        //Eb
                     Key.Q,        //E
                     Key.W,        //F
                     Key.Z,        //F#
                     Key.E,        //G
                     Key.X,        //G#
                     Key.R,        //A
                     Key.C,        //Bb
                     Key.T,        //B

                     Key.D1,        //C
            Key.D,        //C#
            Key.D2,        //D
            Key.F,        //Eb
            Key.D3,        //E
            Key.D4,        //F
            Key.G,        //F#
            Key.D5,        //G
            Key.H,        //G#
            Key.D6,        //A
            Key.J,        //Bb
            Key.D7,        //B

            Key.D8         //C+2
                 };


 

        }
    }
}
