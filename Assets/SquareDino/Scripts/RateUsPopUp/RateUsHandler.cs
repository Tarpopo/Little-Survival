using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquareDino.Scripts.RateUsPopUp
{
    public class RateUsHandler
    {
        private const string NEVER_SHOW_POPUP = "NeverShowPopup";
        private const string NUMBER_OF_SESSIONS = "NumberOfSessions";

        public static bool NeverShowPopup
        {
            get => PlayerPrefs.GetInt(NEVER_SHOW_POPUP, 0) == 1;
            set => PlayerPrefs.SetInt(NEVER_SHOW_POPUP, value ? 1 : 0);
        }

        public static int NumberOfSessions
        {
            get => PlayerPrefs.GetInt(NUMBER_OF_SESSIONS, 0);
            set => PlayerPrefs.SetInt(NUMBER_OF_SESSIONS, value);
        }
    }
}