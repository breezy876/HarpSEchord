
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common.Models.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class MouseSettings
    {
        public MouseSettings()
        {
            MouseWheelEnabled = true;

            this.MouseWheelChangePercent = 15;
            this.MouseWheelShiftPercent = 5;

            this.MouseWheelSeekDuration = 15;
            this.MouseWheelSeekShiftDuration = 5;

            UseMediaKeys = true;
        }

        public MouseWheelFunction Function { get; set; }

        public bool MouseWheelEnabled { get; set; }

        public int MouseWheelChangePercent { get; set; }

        public int MouseWheelShiftPercent { get; set; }

        public int MouseWheelSeekDuration { get; set; }

        public int MouseWheelSeekShiftDuration { get; set; }

        public bool UseMediaKeys { get; set; }
    }
}
