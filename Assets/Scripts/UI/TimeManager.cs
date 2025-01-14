using UnityEngine;
using Utilities;


public class TimeManager : MonoBehaviour
{
    // 游戏时间变量
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    private Season gameSeason = Season.春天;
    private int monthInSeason = 3;

    // 控制游戏时钟的暂停状态
    public bool gameClockPause;
    // 用于累计时间的变量
    private float tikTime;

    // 在脚本实例化时调用，用于初始化游戏时间
    private void Awake()
    {
        NewGameTime();
    }

    // 每帧调用，用于更新游戏时间
    private void Update()
    {
        // 检查游戏计时器是否处于暂停状态
        if (!gameClockPause)
        {
            // 累加时间tick，用于游戏时间的更新
            tikTime += Time.deltaTime;
        
            // 检查是否达到游戏时间更新的阈值
            if (tikTime >= Setting.secondThreshold)
            {
                // 重置时间tick，准备下一轮的时间更新
                tikTime -= Setting.secondThreshold;
                // 调用更新游戏时间的方法，确保游戏时间的准确性
                UpdateGameTime();
            }
        }
    }

    // 初始化游戏时间
    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2022;
        gameSeason = Season.春天;
    }

    // 更新游戏时间，根据当前时间递增并处理时间进位
    private void UpdateGameTime()
    {
        // 增加游戏时间的秒数
        gameSecond++;
        // 检查秒数是否超过设定的上限
        if (gameSecond > Setting.secondHold)
        {
            // 增加游戏时间的分钟数
            gameMinute++;
            // 重置秒数为0
            gameSecond = 0;
        
            // 检查分钟数是否超过设定的上限
            if (gameMinute > Setting.minuteHold)
            {
                // 增加游戏时间的小时数
                gameHour++;
                // 重置分钟数为0
                gameMinute = 0;
        
                // 检查小时数是否超过设定的上限
                if (gameHour > Setting.hourHold)
                {
                    // 增加游戏时间的天数
                    gameDay++;
                    // 重置小时数为0
                    gameHour = 0;
        
                    // 检查天数是否超过设定的上限
                    if (gameDay > Setting.dayHold)
                    {
                        // 重置天数为1，增加游戏时间的月数
                        gameDay = 1;
                        gameMonth++;
        
                        // 检查月数是否超过12个月
                        if (gameMonth > 12)
                            // 重置月数为1
                            gameMonth = 1;
        
                        // 减少季节内的月数计数
                        monthInSeason--;
                        // 检查季节内的月数是否用完
                        if (monthInSeason == 0)
                        {
                            // 重置季节内的月数计数为3
                            monthInSeason = 3;
        
                            // 增加季节编号
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
        
                            // 检查季节编号是否超过设定的上限
                            if (seasonNumber > Setting.seasonHold)
                            {
                                // 重置季节编号为0，增加游戏时间的年数
                                seasonNumber = 0;
                                gameYear++;
                            }
        
                            // 更新当前季节
                            gameSeason = (Season)seasonNumber;
        
                            // 检查年数是否超过9999年
                            if (gameYear > 9999)
                            {
                                // 重置年数为2022年
                                gameYear = 2022;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Second: " + gameSecond + " Minute: " + gameMinute);
    }

        
    
}
