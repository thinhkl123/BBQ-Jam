using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Atom
{
    public class LeaderboardItemPanel : MonoBehaviour
    {
        public Text Rank;
        public Text StatValue;
        public Text Name;

        public void Setup(int rank, int crowns, string name)
        {
            Rank.text = rank.ToString();
            StatValue.text = crowns.ToString();
            Name.text = name;
        }
    }
}