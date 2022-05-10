using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
    public class AnimatedSpriteComponent : SpriteComponent
    {
        private SpriteFrame _currentFrame;
        public SpriteFrame CurrentFrame 
        { 
            get
            {
                return _currentFrame;
            }
            set
            {
                _currentFrame = value;
                if (_currentFrame != null)
                {
                    Source = _currentFrame.Source;
                }
                else
                {
                    Source = null;
                }
            }
        }

        public bool AutoAnimation { get; set; } = true;

        public void NextFrame()
        {
            if (CurrentFrame != null && CurrentFrame.NextFrame != null)
            {
                CurrentFrame = CurrentFrame.NextFrame;
            }
        }
    }

    public class SpriteFrame
    {
        public Uri Source { get; set; }
        public SpriteFrame NextFrame { get; set; }
    }

    public class SpriteFrameSet
    {
        public static SpriteFrame GetFirstFrame(string relativePath)
        {
            string runningProcessDir = Process.GetCurrentProcess().MainModule.FileName;
            string folder = runningProcessDir + @"\..\" + relativePath;
            string[] files = Directory.GetFiles(folder);

            SpriteFrame firstFrame = null;
            SpriteFrame currentFrame = firstFrame;

            foreach(var file in files)
            {
                if (firstFrame == null)
                {
                    firstFrame = new SpriteFrame();
                    firstFrame.Source = new Uri(file, UriKind.Absolute);
                    currentFrame = firstFrame;
                }
                else
                {
                    var frame = new SpriteFrame();
                    frame.Source = new Uri(file, UriKind.Absolute);

                    currentFrame.NextFrame = frame;
                    currentFrame = frame;
                }
            }
            if (currentFrame != firstFrame)
            {
                currentFrame.NextFrame = firstFrame;
            }

            return firstFrame;
        }
    }
}
