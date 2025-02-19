/*
* @Program:Setting.cs
* @Author: Keraz
* @Description:一些常量的设置
* @Date: 2025年02月19日 星期三 20:03:10
*/

namespace Utilities
{
    public class Setting
    {
        // 定义物品淡入淡出的持续时间
        public const float itemFadeDuration = 0.35f;
        // 定义目标透明度，用于控制显示效果
        public const float targetAlpha = 0.3f;
        // 定义时间阈值，用于判断是否需要进行时间相关的处理
        public const float secondThreshold = 0.0012f;
        // 定义秒的最大持有量，用于时间计算或限制
        public const int secondHold = 59;
        // 定义分钟的最大持有量，用于时间计算或限制
        public const int minuteHold = 59;
        // 定义小时的最大持有量，用于时间计算或限制
        public const int hourHold = 23;
        // 定义天的最大持有量，用于时间计算或限制
        public const int dayHold = 30;
        // 定义季节的最大持有量，用于时间计算或限制
        public const int seasonHold = 3;
        // 定义淡入淡出的总持续时间
        public const float fadeDuration = 1.5f;
    }
}
