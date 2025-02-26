Before play the Game you need:

1. Import DOTween plugin
https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676

2. Install iOS 14 Advertising Support package
https://docs.unity3d.com/Packages/com.unity.ads.ios-support@1.0/manual/index.html

3. Install Vibration plugin
https://github.com/BenoitFreslon/Vibration

4. Setup IAP
Check correspondent .pdf in the "Documentations" folder.

5. Setup AdMob
Check correspondent .pdf in the "Documentations" folder.



How to:
1. Add new Level
https://youtu.be/ASI4g9ml4_E



GameConfig. Resources folder => GameConfig.
1. AdsProviderEditor - provider from which ads will be served on Editor.
2. AdsProvider - provider from which ads will be served on Build.
3. MinUnitsCaughtToWin - number of units needed to win.
4. DefaultIsSeek - default game play mode.
5. Hide mode - Seeker will be the unit with index at GamePlayHideState => _seekerIndex, from the units list.



Game play logic.
1. After each level successfully complete game play mode(Seek/Hide) will change to opposite.



Ads logic.
1. Interstitial ads will be triggered to show each time level is completed/failed.



Run the Game.
1. Open Gameplay scene.
2. Press Play.



Contact me if you have any question:
(add purchase invoice number in your email)

aleksandr.gorodiski@gmail.com