using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace NovelTech.libraries
{
    public static class animations
    {
        public static EasingDoubleKeyFrame get_keyframe_double(double value, double from_sec)
        {
            return new EasingDoubleKeyFrame(value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(from_sec)));
        }

        public static EasingThicknessKeyFrame get_keyframe_margin(Thickness value, double from_sec)
        {
            return new EasingThicknessKeyFrame(value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(from_sec)));
        }

        public static DoubleAnimation get_double(double from, double to, int sec)
        {
            DoubleAnimation output = new DoubleAnimation();
            output.From = from;
            output.To = to;
            output.Duration = new Duration(TimeSpan.FromSeconds(sec));
            return output;
        }

        public static ThicknessAnimation get_thickness(Thickness from, Thickness to, int sec)
        {
            ThicknessAnimation output = new ThicknessAnimation();
            output.From = from;
            output.To = to;
            output.Duration = new Duration(TimeSpan.FromSeconds(sec));
            return output;
        }

        //public static EasingDoubleKeyFrame get_keyframe_double(double value, int key_time, double from_sec)
        //{
        //    return new EasingDoubleKeyFrame(value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(from_sec)))
        //    { KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, key_time)) };
        //}

        //public static EasingThicknessKeyFrame get_keyframe_margin(Thickness value, int key_time, double from_sec)
        //{
        //    return new EasingThicknessKeyFrame(value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(from_sec)))
        //    { KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, key_time)) };
        //}
    }
}
