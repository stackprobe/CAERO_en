CAERO
The Silent Archipelago

Created by stackprobe (cerulean.charlotte)


ABOUT THE GAME

Stone islands stretch endlessly across a sea shrouded in mist.
Climb towers, cross corridors, and sometimes stop to take in the distant view.

CAERO is a single-player puzzle escape game in which you explore a fantastical
world from a first-person perspective. Follow your curiosity, listen to the
people you meet, gather clues, and solve the riddles to find a way out.


PUZZLE DIFFICULTY

The puzzles in this game are intended to be very difficult.
They require advanced knowledge of a particular subject and careful thought.

If you find them too difficult, please feel free to search the web.
You are also welcome to ask AI for help when you need it.
Looking up unfamiliar ideas and seeking hints about how to approach a puzzle
are both intended ways to play. Use outside help as you wish, and work toward
an escape at your own pace.


A NOTE ABOUT THE STORY

Some of the dialogue hints at a larger story, but the story is not actually
finished yet! There may be moments when you wonder how the pieces fit together.
There is a plan, and I hope to develop it gradually in future versions.
For now, I hope you enjoy this world, including those glimpses of a story.


SYSTEM REQUIREMENTS

  OS                Windows 10 / 11 (64-bit)
  Graphics          A GPU and driver supporting OpenGL 3.3 or later
  Input             Keyboard and mouse
  Language          English
  Players           1

The distribution includes the files needed to run .NET.
You do not need to install the .NET SDK or runtime separately.
Text uses Segoe UI, Arial, or Tahoma from Windows, with raylib's built-in font
as a fallback. Japanese fonts are not required.


INSTALLATION AND STARTUP

1. Extract the ZIP to a writable folder on a local disk.
2. Double-click StartGame.bat beside this Readme.txt.
3. Select "New Game" on the title screen to begin.
   Select "Continue" to resume a saved game.

Keep StartGame.bat and the bin folder together in the same folder.
Extract all files before starting; do not run the game from inside the ZIP.
You can also launch CAERO.exe in the bin folder directly.


BASIC CONTROLS

[While exploring]
  W / A / S / D     Move
  Mouse             Look
  Shift             Move faster
  Space             Jump / swim upward
  E                 Talk / examine
  Enter             Pause
  Esc               Release the mouse to use the desktop or other applications
  Alt + Enter       Toggle fullscreen

To talk to someone or examine something, move close and look toward it.
After releasing the mouse, click inside the game to resume mouse control.

[While a message is open]
  E / Space / Enter     Show all text / next page / confirm
  Arrow keys / W A S D  Select an answer
  Esc                   Close the message

[Menus]
  Up / Down or W / S    Select an entry
  Enter                 Confirm

You can also use the mouse in menus.
The in-game "Controls" screen lists the controls too.


SAVING AND QUITTING

Your progress is saved automatically. You do not need to save manually.
There is one save slot. Selecting "New Game" overwrites your current save.

To quit, click the X button in the upper-right corner of the window.
Your progress is saved when you exit this way too.
You can also press Enter to pause, select "Return to Title", then choose "Quit".

Save data is stored in bin/generated-view.json, and the escape achievement is
stored separately in bin/achievements.json. To make a backup, close the game
first and copy these files. When moving the game, move the whole folder.


UNINSTALLING

Close the game, then delete the entire folder you extracted.
The game does not register settings in the Windows registry.
This also deletes your save and escape achievement, so back them up first
if you want to keep them.


CREATION

The game's code, 3D objects, images, and music were all created using Codex.


LIBRARIES AND CREDITS

This game uses raylib for graphics, input, audio, and related functionality.
Raylib-cs provides the C# bindings.
The raylib logo shown at startup is the official raylib logo.

[raylib]
  Created by Ramon Santamaria (@raysan5)
  Official website: https://www.raylib.com/
  License: zlib/libpng
  See the included raylib-LICENSE.txt for the copyright notice and full license.

[Raylib-cs 7.0.2]
  Copyright (C) 2018-2025 raylib-cs
  Project: https://github.com/raylib-cs/raylib-cs
  License: zlib
  Full license:
  https://github.com/raylib-cs/raylib-cs/blob/v7.0.2/LICENSE


COPYRIGHT

Rights to the code, text, images, music, and other material in this game
belong to their respective rights holders.
Rights to third-party works, including libraries and logos, belong to their
respective rights holders. Follow the applicable licenses when using them.


CONTACT

stackprobes@gmail.com
