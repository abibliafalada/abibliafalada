using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Controls;

namespace SpokenBible.View
{
    class DefaultEffects
    {
        public static void HidePrincipal(Window window, string target)
        {
            int seconds = 1;
            Storyboard storyboard = new Storyboard();
            TimeSpan time = new TimeSpan(0,0,seconds);

            DoubleAnimation animationFade = new DoubleAnimation(0, new Duration(time));
            Storyboard.SetTargetName(animationFade, target);
            Storyboard.SetTargetProperty(animationFade, new PropertyPath(Frame.OpacityProperty));

            ObjectAnimationUsingKeyFrames animationHide = new ObjectAnimationUsingKeyFrames();
            ObjectKeyFrame hideFrame = new DiscreteObjectKeyFrame(Visibility.Hidden, KeyTime.FromTimeSpan(time));
            animationHide.KeyFrames.Add(hideFrame);
            Storyboard.SetTargetName(animationHide, target);
            Storyboard.SetTargetProperty(animationHide, new PropertyPath(Frame.VisibilityProperty));
            
            storyboard.Children.Add(animationFade);
            storyboard.Children.Add(animationHide);

            storyboard.Begin(window);
        }

        public static void MoveShortcuts(Page page, string target, int leftSize)
        {
            int seconds = 1;
            Storyboard storyboard = new Storyboard();
            TimeSpan time = new TimeSpan(0, 0, seconds);

            DoubleAnimation animationFade = new DoubleAnimation(leftSize, new Duration(time));
            Storyboard.SetTargetName(animationFade, target);
            Storyboard.SetTargetProperty(animationFade, new PropertyPath(Canvas.LeftProperty));

            storyboard.Children.Add(animationFade);

            storyboard.Begin(page);
        }
    }
}
