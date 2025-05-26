using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Common.UI;
using System;
using TMPro;
using UnityEngine.UI;

namespace Atom
{
    public class LeaderboardUI : UIController
    {
        //public System.Action OnCloseClicked;
        //public System.Action OnInfoClicked;
        //public System.Action OnPlayClicked;
        //public System.Action OnRename;
        public System.Action OnHomeBtn;
        public System.Action OnNextLevel;
        public System.Action OnPreviousLevel;

        public GameObject itemPrefab;
        public Transform Content;
        [SerializeField] private Button homeBtn;
        [SerializeField] private TextMeshProUGUI levelText;
        public Button nextLevel;
        public Button previousLevel;

        public UserPanel UserPanel;

        public void OnHomeBtnClicked()
        {
            OnHomeBtn?.Invoke();
        }

        public void OnNextLevelClick()
        {
            OnNextLevel?.Invoke();
        }

        public void OnPreviousLevelClick()
        { 
            OnPreviousLevel?.Invoke();
        }

        public void SetLevelText(int value)
        {
            levelText.text = "Level " + value.ToString();
        }

        //public void onCloseClicked()
        //{
        //    OnCloseClicked?.Invoke();
        //}

        //public void onInfoClicked()
        //{
        //    OnInfoClicked?.Invoke();
        //}

        //public void onPlayClicked()
        //{
        //    OnPlayClicked?.Invoke();
        //}

        //public void onRenameClicked()
        //{
        //    OnRename?.Invoke();
        //}

        public void Setup(string userName, int userRank, int userStatValue, TimeSpan remain, List<LeaderboardItemData> itemsData)
        {
            //TimeRemain.Setup(remain);
            SetupLeaderboardItems(itemsData);
            //PlayBtn.TextMesh.text = "ROUND " + (G.RoyalLeagueLogic.CurrentRound + 1).ToString();
            UserPanel.Setup(userRank, userStatValue, userName);
        }

        private void SetupLeaderboardItems(List<LeaderboardItemData> itemsData)
        {
            foreach (var itemData in itemsData)
            {
                GameObject itemGameObject = GameObject.Instantiate(itemPrefab, Content);
                itemGameObject.SetActive(true);
                itemGameObject.GetComponent<LeaderboardItemPanel>().Setup(itemData.Rank, itemData.StatValue, itemData.Name);
                /*
                var item = addItem();
                var rw = G.RoyalLeagueLogic.GetRewardAtRank(itemData.Rank);
                item.Setup(itemData, rw);
                if (itemData.Name == G.RoyalLeagueLogic.Name)
                {
                    _cacheUserPanel = item;
                }
                */
            }
        }

        public void ClearLeaderBoard()
        {
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
