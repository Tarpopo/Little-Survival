using UnityEngine;

#if FLAG_GA
	using GameAnalyticsSDK;
#endif

namespace SquareDino.Scripts.MyAnalytics
{
	public class MyGameAnalytics : MonoBehaviour
#if FLAG_GA	
	, IGameAnalyticsATTListener
#endif
	{
#if FLAG_GA	
		private void Start()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameAnalytics.RequestTrackingAuthorization(this);
			}
			else
			{
				GameAnalytics.Initialize();
			}
		}

		public void GameAnalyticsATTListenerNotDetermined()
		{
			GameAnalytics.Initialize();
		}

		public void GameAnalyticsATTListenerRestricted()
		{
			GameAnalytics.Initialize();
		}

		public void GameAnalyticsATTListenerDenied()
		{
			GameAnalytics.Initialize();
		}

		public void GameAnalyticsATTListenerAuthorized()
		{
			GameAnalytics.Initialize();
		}
#endif
	}
}