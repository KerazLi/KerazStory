using System;
using UnityEngine;
using Utilities;

/*
* @Program:TimeManager.cs
* @Author: Keraz
* @Description: 时间管理功能
* @Date: 2025年02月19日 星期三 19:46:52
*/


public class TimeManager : Singleton<TimeManager>
{
    // 游戏时间变量
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    private Season gameSeason = Season.春天;
    private int monthInSeason = 3;

    // 控制游戏时钟的暂停状态
    public bool gameClockPause;
    // 用于累计时间的变量
    private float tikTime;
    public TimeSpan GameTime=> new TimeSpan(gameHour,gameMinute,gameSecond);

    // 在脚本实例化时调用，用于初始化游戏时间
    protected override void Awake()
    {
        base.Awake();
        NewGameTime();
    }

    private void Start()
    {
        EventHandler.CallGameMinuteEvent(gameMinute,gameHour);
        EventHandler.CallGameDataEvent(gameHour,gameDay,gameMonth,gameYear, gameSeason); 
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

        if (Input.GetKey(KeyCode.T))
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }
        //测试使用的，不然月份不增加
        if (Input.GetKeyDown(KeyCode.G))
        {
            gameDay++;
            EventHandler.CallGameDayEvent(gameDay, gameSeason);
            EventHandler.CallGameDataEvent(gameHour,gameDay,  gameMonth,gameYear, gameSeason);
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
        gameSecond++;
        if (gameSecond > Setting.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if (gameMinute > Setting.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if (gameHour > Setting.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if (gameDay > Setting.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;

                        if (gameMonth > 12)
                        {
                            gameMonth = 1; 
                        }
                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;

                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            if (seasonNumber > Setting.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }

                            gameSeason = (Season)seasonNumber;

                            if (gameYear > 9999)
                            {
                                gameYear = 2022;
                            }
                        }
                        //用来刷新地图和农作物生长
                        EventHandler.CallGameDayEvent(gameDay, gameSeason);
                    }
                }
                EventHandler.CallGameDataEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        }
    }



}
