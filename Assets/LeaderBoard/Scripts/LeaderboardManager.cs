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
        private string username;

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

        //private void GetListLeaderboardItem(System.Action<LeaderboardItemData, List<LeaderboardItemData>> action)
        //{
        //    List<LeaderboardItemData> leaderboardItemDatas = new List<LeaderboardItemData>();
        //    LeaderboardItemData user = null;
        //    PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
        //    {
        //        StatisticName = "BBQJam",
        //        MaxResultsCount = 1
        //    }, (result) => {
        //        if (result.Leaderboard.Count > 0)
        //        {
        //            PlayerLeaderboardEntry playerData = result.Leaderboard[0];
        //            user = new LeaderboardItemData() { Rank = playerData.Position + 1, StatValue = playerData.StatValue, Name = playerData.DisplayName };
        //        }
        //        -----------------------
        //        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        //        {
        //            StatisticName = "BBQJam",
        //            MaxResultsCount = 10,
        //        }, (result) => {
        //            for (int i = 0; i < result.Leaderboard.Count; i++)
        //            {
        //                PlayerLeaderboardEntry playerData = result.Leaderboard[i];
        //                leaderboardItemDatas.Add(new LeaderboardItemData() { Rank = playerData.Position + 1, StatValue = playerData.StatValue, Name = playerData.DisplayName });
        //            }
        //            action.Invoke(user, leaderboardItemDatas);
        //        }, FailureCallback);
        //    }, FailureCallback);
        //}

        private void GetListLeaderboardItem(System.Action<LeaderboardItemData, List<LeaderboardItemData>> action)
        {
            List<LeaderboardItemData> leaderboardItemDatas = new List<LeaderboardItemData>();
            LeaderboardItemData user = null;

            PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = "BBQJam",
                MaxResultsCount = 1
            }, (aroundResult) => {
                if (aroundResult.Leaderboard.Count > 0)
                {
                    var playerEntry = aroundResult.Leaderboard[0];
                    GetUsernameFromPlayFabId(playerEntry.PlayFabId, (username) =>
                    {
                        user = new LeaderboardItemData()
                        {
                            Rank = playerEntry.Position + 1,
                            StatValue = playerEntry.StatValue,
                            Name = username // dùng Username thay vì DisplayName
                        };

                        // Sau khi lấy xong user, tiếp tục lấy top leaderboard
                        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
                        {
                            StatisticName = "BBQJam",
                            MaxResultsCount = 10
                        }, (topResult) => {
                            int count = topResult.Leaderboard.Count;
                            int processed = 0;

                            for (int i = 0; i < count; i++)
                            {
                                var entry = topResult.Leaderboard[i];
                                GetUsernameFromPlayFabId(entry.PlayFabId, (usernameTop) =>
                                {
                                    leaderboardItemDatas.Add(new LeaderboardItemData()
                                    {
                                        Rank = entry.Position + 1,
                                        StatValue = entry.StatValue,
                                        Name = usernameTop
                                    });

                                    processed++;
                                    if (processed == count)
                                    {
                                        // Chỉ invoke khi đã lấy đủ username
                                        action.Invoke(user, leaderboardItemDatas);
                                    }
                                });
                            }

                        }, FailureCallback);
                    });
                }
                else
                {
                    action.Invoke(null, new List<LeaderboardItemData>());
                }
            }, FailureCallback);
        }

        private void GetUsernameFromPlayFabId(string playFabId, System.Action<string> callback)
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
            {
                PlayFabId = playFabId
            }, (accountResult) => {
                var username = accountResult.AccountInfo?.Username ?? "Unknown";
                callback.Invoke(username);
            }, (error) => {
                Debug.LogError("Failed to get account info: " + error.ErrorMessage);
                callback.Invoke("Unknown");
            });
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