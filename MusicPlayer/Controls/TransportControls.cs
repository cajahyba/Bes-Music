﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MusicPlayer.Controls
{
    public sealed class TransportControls : MediaTransportControls
    {




        public MediaPlaybackList PlayList
        {
            get { return (MediaPlaybackList)GetValue(PlayListProperty); }
            set { SetValue(PlayListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayListProperty =
            DependencyProperty.Register("PlayList", typeof(MediaPlaybackList), typeof(TransportControls), new PropertyMetadata(null));




        public SongInformation CurrentSong
        {
            get { return (SongInformation)this.GetValue(CurrentSongProperty); }
            set { this.SetValue(CurrentSongProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentSong.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSongProperty =
            DependencyProperty.Register("CurrentSong", typeof(SongInformation), typeof(TransportControls), new PropertyMetadata(null));





        public ICommand NextCommand
        {
            get { return (ICommand)this.GetValue(NextCommandProperty); }
            set { this.SetValue(NextCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextCommandProperty =
            DependencyProperty.Register("NextCommand", typeof(ICommand), typeof(TransportControls), new PropertyMetadata(DisabledCommand.Instance));



        public ICommand PreviousCommand
        {
            get { return (ICommand)this.GetValue(PreviousCommandProperty); }
            set { this.SetValue(PreviousCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousCommandProperty =
            DependencyProperty.Register("PreviousCommand", typeof(ICommand), typeof(TransportControls), new PropertyMetadata(DisabledCommand.Instance));




        public ICommand PlayCommand
        {
            get { return (ICommand)this.GetValue(PlayCommandProperty); }
            set { this.SetValue(PlayCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayCommandProperty =
            DependencyProperty.Register("PlayCommand", typeof(ICommand), typeof(TransportControls), new PropertyMetadata(DisabledCommand.Instance));




        public ICommand PauseCommand
        {
            get { return (ICommand)this.GetValue(PauseCommandProperty); }
            set { this.SetValue(PauseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PauseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PauseCommandProperty =
            DependencyProperty.Register("PauseCommand", typeof(ICommand), typeof(TransportControls), new PropertyMetadata(DisabledCommand.Instance));



        public ICommand ShuffleCommand
        {
            get { return (ICommand)this.GetValue(ShuffleCommandProperty); }
            set { this.SetValue(ShuffleCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShuffleCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShuffleCommandProperty =
            DependencyProperty.Register("ShuffleCommand", typeof(ICommand), typeof(TransportControls), new PropertyMetadata(DisabledCommand.Instance));




        public ICommand RepeateCommand
        {
            get { return (ICommand)this.GetValue(RepeateCommandProperty); }
            set { this.SetValue(RepeateCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RepeateCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RepeateCommandProperty =
            DependencyProperty.Register("RepeateCommand", typeof(ICommand), typeof(TransportControls), new PropertyMetadata(DisabledCommand.Instance));




        public ICommand SwitchFullScreenCommand
        {
            get { return (ICommand)this.GetValue(SwitchFullScreenCommandProperty); }
            set { this.SetValue(SwitchFullScreenCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SwitchFullScreenCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SwitchFullScreenCommandProperty =
            DependencyProperty.Register("SwitchFullScreenCommand", typeof(ICommand), typeof(TransportControls), new PropertyMetadata(DisabledCommand.Instance));



        public bool IsFullscreen
        {
            get { return (bool)this.GetValue(IsFullscreenProperty); }
            set { this.SetValue(IsFullscreenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFullscreen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFullscreenProperty =
            DependencyProperty.Register("IsFullscreen", typeof(bool), typeof(TransportControls), new PropertyMetadata(false, IsFullscreenChanged));

        private static void IsFullscreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as TransportControls;
            var isFullscreen = (bool)e.NewValue;

            if (isFullscreen)
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            else
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();


            VisualStateManager.GoToState(me, isFullscreen ? "FullScreen" : "Windowed", true);

        }

        public bool IsShuffled
        {
            get { return (bool)this.GetValue(IsShuffledProperty); }
            set { this.SetValue(IsShuffledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShuffled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShuffledProperty =
            DependencyProperty.Register("IsShuffled", typeof(bool), typeof(TransportControls), new PropertyMetadata(false, IsShuffledChanged));

        private static void IsShuffledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as TransportControls;
            var isShuffeling = (bool)e.NewValue;

            if (me.mediaPlayerElement.Source is MediaPlaybackList list)
                list.ShuffleEnabled = isShuffeling;
        }

        public bool IsRepeate
        {
            get { return (bool)this.GetValue(IsRepeateProperty); }
            set { this.SetValue(IsRepeateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRepeate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRepeateProperty =
            DependencyProperty.Register("IsRepeate", typeof(bool), typeof(TransportControls), new PropertyMetadata(false, IsRepeateChanged));

        private static void IsRepeateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as TransportControls;
            var isRepeating = (bool)e.NewValue;

            if (me.mediaPlayerElement.Source is MediaPlaybackList list)
                list.AutoRepeatEnabled = isRepeating;

        }

        public bool IsPlaying
        {
            get { return (bool)this.GetValue(IsPlayingProperty); }
            set { this.SetValue(IsPlayingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPlaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(TransportControls), new PropertyMetadata(false, IsPlayingChanged));

        private static void IsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as TransportControls;
            var isPlaying = (bool)e.NewValue;
            VisualStateManager.GoToState(me, isPlaying ? "PauseState" : "PlayState", true);
            if (isPlaying)
                me.mediaPlayerElement.MediaPlayer.Play();
            else
                me.mediaPlayerElement.MediaPlayer.Pause();
        }

        private MediaPlayerElement mediaPlayerElement;

        public TransportControls()
        {
            this.DefaultStyleKey = typeof(TransportControls);

            Loaded += this.TransportControls_Loaded;

        }
        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    if (!ReferenceEquals(lastParent, Parent))
        //        SetCommands();

        //    return base.MeasureOverride(availableSize);
        //}

        private void TransportControls_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetCommands();
        }

        private void SetCommands()
        {
            this.mediaPlayerElement = FindParent(this);
            if (this.mediaPlayerElement != null)
            {
                if (this.mediaPlayerElement.MediaPlayer is null)
                    this.mediaPlayerElement.SetMediaPlayer(new MediaPlayer());

                this.mediaPlayerElement.Source = this.PlayList;

                this.PlayList.CurrentItemChanged += this.PlayList_CurrentItemChanged;

                this.NextCommand = new MediaBehaviorCommand(this.mediaPlayerElement.MediaPlayer.CommandManager.NextBehavior, this.Next);
                this.PreviousCommand = new MediaBehaviorCommand(this.mediaPlayerElement.MediaPlayer.CommandManager.PreviousBehavior, this.Previous);
                this.PlayCommand = new MediaBehaviorCommand(this.mediaPlayerElement.MediaPlayer.CommandManager.PlayBehavior, this.Play);
                this.PauseCommand = new MediaBehaviorCommand(this.mediaPlayerElement.MediaPlayer.CommandManager.PauseBehavior, this.Pause);

                this.ShuffleCommand = new MediaBehaviorCommand(this.mediaPlayerElement.MediaPlayer.CommandManager.ShuffleBehavior, this.SwitchShuffle);
                this.RepeateCommand = new MediaBehaviorCommand(this.mediaPlayerElement.MediaPlayer.CommandManager.AutoRepeatModeBehavior, this.SwitchRepeat);
                //this.mediaPlayerElement.RegisterPropertyChangedCallback(MediaPlayerElement.SourceProperty, this.SourceChanged);

                this.SwitchFullScreenCommand = new DelegateCommand(() => this.IsFullscreen = !this.IsFullscreen);
                this.IsShuffled = this.PlayList.ShuffleEnabled;
                this.IsRepeate = this.PlayList.AutoRepeatEnabled;
                
            }

            MediaPlayerElement FindParent(DependencyObject current)
            {
                if (current is null || current is MediaPlayerElement)
                    return current as MediaPlayerElement;
                return FindParent(VisualTreeHelper.GetParent(current));
            }


        }


        private async void PlayList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {

            var playbackItem = args.NewItem;
            if (playbackItem != null)
            {
                await SetCurrentSong(playbackItem.GetDisplayProperties());
            }
            else
                await SetCurrentSong(null);

            async Task SetCurrentSong(MediaItemDisplayProperties displayPropertys)
            {
                if (this.Dispatcher.HasThreadAccess)
                {
                    if (displayPropertys != null)
                        this.CurrentSong = await SongInformation.Create(displayPropertys.MusicProperties.AlbumTitle, string.IsNullOrWhiteSpace(displayPropertys.MusicProperties.Artist)? displayPropertys.MusicProperties.AlbumArtist: displayPropertys.MusicProperties.Artist, displayPropertys.MusicProperties.Title, displayPropertys.Thumbnail);
                    else
                        this.CurrentSong = null;
                }
                else
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => await SetCurrentSong(displayPropertys));
                }
            }
        }


        private void Play()
        {
            this.IsPlaying = true;
        }
        private void Pause()
        {
            this.IsPlaying = false;
        }

        private void Next()
        {
            if (this.mediaPlayerElement.Source is MediaPlaybackList list)
                list.MoveNext();
        }
        private void Previous()
        {
            if (this.mediaPlayerElement.Source is MediaPlaybackList list)
                list.MovePrevious();

        }

        private void SwitchShuffle()
        {
            this.IsShuffled = !this.IsShuffled;
        }

        private void SwitchRepeat()
        {
            this.IsRepeate = !this.IsRepeate;
        }



        public class SongInformation
        {
            public SongInformation(string albumTitle, string artist, string title, ImageSource thumbnail)
            {
                this.AlbumTitle = albumTitle;
                this.Artist = artist;
                this.Title = title;
                this.Thumbnail = thumbnail;
            }

            public static async Task<SongInformation> Create(string albumTitle, string artist, string title, RandomAccessStreamReference thumbnail)
            {
                using (var stream = await thumbnail.OpenReadAsync())
                {
                    var imageSource = new BitmapImage();
                    await imageSource.SetSourceAsync(stream);
                    return new SongInformation(albumTitle, artist, title, imageSource);
                }

            }

            public string Artist { get; }

            public string AlbumTitle { get; }

            public string Title { get; }

            public ImageSource Thumbnail { get; }
        }

    }
}