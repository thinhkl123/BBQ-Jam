using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Athena.Common.UI;
using PlayFab;
using PlayFab.ClientModels;

namespace Atom
{
    public class LeaderboardManager : MonoSingleton<LeaderboardManager>
    {
        private int curLevelShowed;

        protected LeaderboardUI _royalLeagueLeaderboardUI;

        public LeaderboardUI LeaderboardUI
        {
            get
            {
                return _royalLeagueLeaderboardUI;
            }
        }

        public void Setup()
        {
            _royalLeagueLeaderboardUI = AppManager.Instance.ShowSafeTopUI<LeaderboardUI>("UI/LeaderBoardUI", false);

            _royalLeagueLeaderboardUI.OnHomeBtn = HideUI;

            HideUI();
        }

        public void GetLeaderboard()
        {
            GetListLeaderboardItem((user, list) =>
            {
                _royalLeagueLeaderboardUI.Setup(user.Name, user.Rank, user.StatValue, new System.TimeSpan(1, 1, 1), list);
            });
        }

        public void UpdateLeaderboard(int statValue)
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate {
                        StatisticName = "BBQJam",
                        Value = statValue
                    }
                }
            }, OnStatisticsUpdated, FailureCallback);
        }

        private void GetListLeaderboardItem(System.Action<LeaderboardItemData, List<LeaderboardItemData>> action)
        {
            List<LeaderboardItemData> leaderboardItemDatas = new List<LeaderboardItemData>();
            LeaderboardItemData user = null;
            PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = "BBQJam",
                MaxResultsCount = 1
            }, (result) => {
                if (result.Leaderboard.Count > 0)
                {
                    PlayerLeaderboardEntry playerData = result.Leaderboard[0];
                    user = new LeaderboardItemData() { Rank = playerData.Position, StatValue = playerData.StatValue, Name = playerData.DisplayName };
                }
                //-----------------------
                PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
                {
                    StatisticName = "BBQJam",
                    MaxResultsCount = 10,
                }, (result) => {
                    for (int i = 0; i < result.Leaderboard.Count; i++)
                    {
                        PlayerLeaderboardEntry playerData = result.Leaderboard[i];
                        leaderboardItemDatas.Add(new LeaderboardItemData() { Rank = playerData.Position, StatValue = playerData.StatValue, Name = playerData.DisplayName });
                    }
                    action.Invoke(user, leaderboardItemDatas);
                }, FailureCallback);
            }, FailureCallback);
        }

        private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
        {
            Debug.Log("Successfully submitted high score");
        }

        private void FailureCallback(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        }

        public void ShowUI()
        {
            curLevelShowed = DataManager.Instance.GetCurMaxLevel();

            _royalLeagueLeaderboardUI.ClearLeaderBoard();
            GetLeaderboard();
            //_royalLeagueLeaderboardUI.gameObject.SetActive(true);
            UIManager.Instance.ShowUI(_royalLeagueLeaderboardUI, true);
        }

        public void HideUI()
        {
            _royalLeagueLeaderboardUI.gameObject.SetActive(false);
        }
    }

    public class LeaderboardItemData
    {
        public int Rank;
        public int StatValue;
        public string Name;
    }
}