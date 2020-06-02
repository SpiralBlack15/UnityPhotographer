# UnityPhotographer
Make a photo of your GameView in almost ANY possible resolution you want.

This util adds <i>Spiral Tools / PhotoStudio</i> item on Editor's toolbar. It produce PhotoStudio EditorWindow which contains following settings:
- Path where screenshots will be saved.
- Filename prefix.
- Use (or not) native resolution of the GameView. If not,screenshots are taken at a fixed resolution set by the user. The bigger the resolution, the higher the resolution, the longer it will take to render, which can lead to delays
- Toggle and timer to make a screenshot in N seconds after game start (if you interrupt the game earlier, it takes screenshot anyway after this time passed).

If "make a photo" button disabled, it means that you need to open (or to restart, if it already opened) Editor's GameView. Remember, even if you are in PlayMode, you cannot render screenshot of the game while you don't have a GameView. Also, non of the functionality can be called outside UnityEditor, so make sure you don't call screenmaker functions outside #if UNITY_EDITOR brackets.
