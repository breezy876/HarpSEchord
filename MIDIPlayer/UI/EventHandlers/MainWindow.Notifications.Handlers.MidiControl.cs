using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Window;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Settings;
using Hscm.UI.Notifications.Playlist;
using System.Windows.Controls;
using Common;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.ViewModels.MainWindow;
using Common.Models.Ensemble;
using Common.Helpers;
using System.Windows.Media;
using System.Windows.Threading;
using Common.Messaging.Player;
using System.Windows;
using Common.Messaging;
using Common.Messaging.Settings;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
using System.Windows.Input;
using Common.Music;
using Common.Interop;
using static Common.Interop.WindowHelpers;
using System.Runtime.InteropServices;
using Common.Midi;
using System.Threading;

namespace Hscm.UI
{
    public partial class MainWindow
    {


        private void TempoResetNotificationReceived(TempoResetNotification obj)
        {
            ResetTempo();
        }

        private void TempoMouseEnterNotificationReceived(TempoMouseEnterNotification obj)
        {
            tempoSlider.Focus();
        }

        //private void TempoShiftNotificationReceived(TempoShiftNotification obj)
        //{
        //    mouseWheelShiftDown = obj.IsDown;
        //}

        //private void TempoRevertNotificationReceived(TempoRevertNotification obj)
        //{
        //    if (!obj.IsDown)
        //    {
        //        this.viewModel.Tempo = prevTempo;
        //        //UpdateTempo();
        //        tempoRevertStarted = false;
        //    }
        //    else
        //    {
        //        if (!tempoRevertStarted)
        //        {
        //            prevTempo = this.viewModel.Tempo;
        //            tempoRevertStarted = true;
        //        }
        //    }
        //}

        private async void SeekMouseLeftButtonDownNotificationReceived(SeekMouseLeftButtonDownNotification obj)
        {

            //sliderChangeBegin = true;
            //AppendLog("","YESSSS");
            //await Task.Run(() =>
            //{
            //    Thread.Sleep(2000);
            //    sliderChangeBegin = false;
            //});
        }

        private void SeekMouseEnterNotificationReceived(SeekMouseEnterNotification obj)
        {
            //canSliderScroll = true;
        }

        private void SeekMouseLeaveNotificationReceived(SeekMouseLeaveNotification obj)
        {
            //canSliderScroll = false;
        }

        private async void TempoMouseWheelNotificationReceived(TempoMouseWheelNotification msg)
        {
            //if (!Common.Settings.AppSettings.MouseAndKeyboardSettings.MouseWheelEnabled)
            //    return;

            if (msg.Delta > 0)
               await IncreaseTempo(5);
            else
                await DecreaseTempo(5);
        }

        private async void SeekMouseWheelNotificationReceived(SeekMouseWheelNotification msg)
        {
            sliderChangeBegin = true;

            if (msg.Delta > 0)
                await SeekForward(5000);
            else
                await SeekBackward(5000);

            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                sliderChangeBegin = false;
            });

        }

        private async void SequenceTempoChangedNotificationReceived(SequenceTempoChangedNotification msg)
        {
            bool changed = await ChangeTempo(msg.Tempo);

            if (!changed)
                return;

            toolTip.PlacementTarget = tempoSlider;
            toolTip.Content = viewModel.TempoText;
            toolTip.IsOpen = true;
            toolTip.StaysOpen = false;
        }

        private async void SequenceTempoUpdatedNotificationReceived(SequenceTempoUpdatedNotification obj)
        {
            await SavePlaylistSettings();
        }

        private async void SeekChangedNotificationReceived(SeekChangedNotification msg)
        {
            if (sliderChangeBegin)
                return;

            await Seek((long)this.SeekSlider.Value);
            this.viewModel.TimeElapsed = new SequenceTimeSpan((long)SeekSlider.Value).ToString();
        }

        private async Task IncreaseTempo(int percent)
        {
            if (this.viewModel.Tempo == 200)
                return;

            this.viewModel.Tempo += percent;

            await SavePlaylistSettings();
            //this.UpdateTempo();
        }

        private async Task DecreaseTempo(int percent)
        {
            if (this.viewModel.Tempo <= 10)
                return;

            this.viewModel.Tempo -= percent;

            await SavePlaylistSettings();
            //this.UpdateTempo();
        }

        private async Task SeekForward(long val)
        {
            if (this.viewModel.SeekValue >= this.viewModel.SequenceLength)
                return;

            long seekVal = this.viewModel.SeekValue;

            if (seekVal + val >= this.viewModel.SequenceLength)
                this.viewModel.SeekValue = this.viewModel.SequenceLength;
            else
                this.viewModel.SeekValue += val;
            position += val;
           await Seek(position);
        }

        private async Task SeekBackward(long val)
        {
            if (this.viewModel.SeekValue <= 0)
                return;

            long seekVal = this.viewModel.SeekValue;

            if (seekVal - val <= 0)
                this.viewModel.SeekValue = 0;
            else
                this.viewModel.SeekValue -= val;
            position -= val;
            await Seek(position);
        }



        private void ResetTempo()
        {
            this.viewModel.Tempo = 100;
            //UpdateTempo();
        }



    }
}
