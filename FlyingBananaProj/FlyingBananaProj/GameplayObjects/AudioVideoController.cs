//Written by Eric Plaza

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace FlyingBananaProj
{
    class AudioVideoController : Microsoft.Xna.Framework.Game
    {
        Song song;
        SoundEffect sound;
        Video video;
        VideoPlayer videoPlayer;
        public AudioVideoController()
        {
            Content.RootDirectory = "Content";
            videoPlayer = new VideoPlayer();
        }

        public void playMusic(string songTitle)
        {
            song = Content.Load<Song>("audio/music/"+songTitle);
            MediaPlayer.Play(song);
        }
        public void playSoundEffect(string sound, float vol)
        {
            this.sound = Content.Load<SoundEffect>("audio/sfx/" + sound);
            this.sound.Play(vol, 0, 0);
        }
        public void playMovie(string videos)
        {
            video = Content.Load<Video>(videos);
            videoPlayer.Play(video);
        }
        public void stopMusic()
        {
        }
    }
}
