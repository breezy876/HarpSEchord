using Common;
using Common.Helpers;
using Common.Interop;
using Common.Messaging;
using Common.Messaging.Player;
using Common.Music;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace Hscm.UI
{
    public partial class MainWindow
    {

        #region UI events

        #region playback
        private async void btnPlay_Click(object sender, EventArgs e)
        {
            await Play();
        }

        private async void btnPause_Click(object sender, EventArgs e)
        {
            await Pause();
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            await Stop();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            await Next();
        }

        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            await Previous();
        }

 

        private void SeekSlider_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                sliderChangeBegin = true;
            }
        }

        private async void SeekSlider_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && sliderChangeBegin)
            {
                await Seek((long)this.SeekSlider.Value);
                sliderChangeBegin = false;
            }
        }

        private void SeekSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {


        }
        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            //base.OnClosing(e);

            try
            {
                Cleanup();
            }
            catch (Exception ex)
            {

            }

            e.Cancel = false;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //HandleResize(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             Initialize();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (shown)
                return;

            shown = true;

      
                Dispatcher.Invoke(() => InitializeChildWindows());

                InitializeCore();

                ApplySettings();

                ScanPluginPath();
                
                CreateAndStartCharConfigWatcher();
          
                CreateAndStartPluginWatcher();

                UpdateCharacters();

                FfxivControl.AddCharsFromConfigInTestMode();
     
        }

        private async void Window_Drop(object sender, DragEventArgs e)
        {
            await DragDropFiles(e);
        }

        private async Task DragDropFiles(DragEventArgs args)
        {
            if (args.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = args.Data.GetData(DataFormats.FileDrop, true) as string[];

                if (Path.GetExtension(filePaths.First()).Equals(".pl"))
                {
                    await this.playlistControl.OpenPlaylist(filePaths.First(), true);
                    return;
                }

                if (!filePaths.IsNullOrEmpty())
                    await this.playlistControl.OpenFiles(filePaths.ToList());
            }
        }
        #endregion
    }
}
