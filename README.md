# CAERO

**The Silent Archipelago - Version 1.0, English Edition**

![Stone arches and stairways in the mist](img/0004.png)

Stone islands stretch endlessly across a sea shrouded in mist.
Explore this fantastical world, solve its riddles, and find a way to escape.

CAERO is a single-player first-person puzzle escape game, built around very difficult mathematical riddles. Walk among the islands, listen to the people you meet, and examine anything that catches your attention to gather clues to a way out.

## Player guide

### Explore, solve, and escape

Stone buildings, towers, and corridors stand on islands surrounded by the sea. Cross bridges, climb stairs, and look closely at your surroundings.
New islands are generated as you explore. Return to a place you have visited, and the same landscape will be waiting.

Use what you learn from conversations and objects to think about the mysteries of this world. Connect your discoveries with mathematical knowledge to find a path to escape.

Day gives way to dusk and night. Changing light, glowing lanterns, and music that follows the day and night create the atmosphere of this world.
Take time to enjoy the view and approach the riddles at your own pace.

### Screenshots

| A bridge stretching into the mist | Passages between high towers |
| --- | --- |
| ![A misty bridge and towers beyond it](img/0002.png) | ![High towers joined by narrow passages](img/0006.png) |
| **Structures floating in the air** | **An island lit by lanterns at night** |
| ![Floating stone structures and passages disappearing into the mist](img/0009.png) | ![Stone walls and an island number sign illuminated by lanterns](img/0011.png) |

### Difficulty, web searches, and AI

The puzzles focus on **mathematical problems**.
They are intended to be extremely challenging, even for people with a strong interest in mathematics.

**If they feel too difficult, please feel free to search the web. You are welcome to ask AI for help too.**
Looking up unfamiliar ideas, asking for hints about an approach, and using what you learn to think further are all intended ways to play. Use those resources as you wish while working toward an escape.

### System requirements

| Item | Requirement |
| --- | --- |
| OS | Windows 10 / 11 (64-bit) |
| Graphics | A GPU and driver supporting OpenGL 3.3 or later |
| Input | Keyboard and mouse |
| Language | English |
| Players | 1 |

The distribution includes the files needed to run .NET; the .NET SDK is not required.
Text uses Segoe UI, Arial, or Tahoma from Windows, with raylib's built-in font as a fallback. Japanese fonts are not required.

### Getting started

1. Extract the entire distribution ZIP.
2. Double-click `StartGame.bat` in the extracted `CAERO` folder.
3. Select **New Game** on the title screen to begin. Select **Continue** to resume a saved game.

Keep `StartGame.bat` and the `bin` folder together. You can also run `bin/CAERO.exe` directly.

### Basic controls

| Key or input | Action |
| --- | --- |
| W / A / S / D | Move |
| Mouse | Look |
| Shift | Move faster |
| Space | Jump / swim upward |
| E | Talk / examine |
| Enter | Pause |
| Esc | Release the mouse |
| Left click | Resume mouse control after releasing it |
| Alt + Enter | Toggle windowed / fullscreen mode |

Move close to a person or object and look toward it to see the interaction prompt.
You can swim in the sea. Look for stairs extending below the water to return to land.
The **Controls** screen is available from the title and pause menus.

**While a message is open**, press E / Space / Enter to reveal all text, advance to the next page, or confirm. Esc closes the message. Use the arrow keys or W / A / S / D to choose an answer.

**In menus**, use Up / Down or W / S to select, and Enter to confirm. Mouse input is also supported.

### Saving and quitting

Progress is saved automatically about every ten seconds, when pausing, when returning to the title, and when quitting.
Select **Continue** on the title screen to resume. It is unavailable without a valid save.

**There is one save slot. Selecting New Game overwrites your current save.**

The distribution stores its save in `bin/generated-view.json`. To back it up, close the game and copy the file.
When moving the game, move the entire folder, including your save.

To quit through the menus, press Enter to pause, choose **Return to Title**, then choose **Quit**.

## Technical information

### Development and repository layout

Implemented in C# / .NET 8 with Raylib-cs 7.0.2 (native raylib 5.5).
Rendering uses OpenGL / GLSL. World generation, movement, collision, conversations, audio, and saving are implemented by the game.

| Path | Purpose |
| --- | --- |
| `src/CAERO.sln` | Visual Studio solution |
| `src/CAERO/` | Game source and project |
| `src/CAERO/Shaders/` | World, sky, and post-processing shaders |
| `src/CAERO/Content/` | Editable images and text catalogues; the runtime loads the English JSON files |
| `src/CAERO/Music/` | Day, night, and title music |
| `src/LOCALIZATION.md` | English terminology and clue translation notes (for developers; contains spoilers) |
| `tools/ContentPacker/` | Distribution content packer |
| `tools/verify_translation.py` | Compare English catalogue structure and gameplay metadata with the Japanese originals |
| `doc/` | Instructions and launcher copied into the distribution ZIP |
| `Release.ps1` | Build, package content, and create the ZIP |

### World generation and streaming

The world is managed in sectors measuring 192 units on each side. Architecture and props are generated procedurally from shared geometry and placement rules, with checks for support, walking routes, and overlapping structures.

The first 128 bits of a SHA-512 hash of integer island coordinates, combined with the world seed and purpose-specific values, form the random seeds. Generation uses a custom xor128 generator. The same generation version, seed, and coordinates reproduce the same island.

At startup, the surrounding 5 by 5 sectors are prepared. During movement, background tasks generate additional sectors, and at most one completed sector is installed for rendering per frame. At most 49 sectors, a 7 by 7 area, are retained around the current location. Distant render models and collision data are released and regenerated on return, avoiding memory use that grows with total travel distance.

Island addresses use signed 64-bit integers; positions within an island use local coordinates. A Floating Origin shifts the rendering and physics origin to the current island whenever the player crosses an island boundary, reducing floating-point precision loss at great distances.

### Rendering, audio, and data

- **Rendering:** Procedural meshes and materials combine with fog, day and night lighting, lanterns, water and underwater effects, and post-processing.
- **Audio:** Footsteps, water sounds, and underwater ambience are synthesized from waveforms and noise at startup. Included original MP3 music changes with day and night.
- **Text and images:** Development builds read JSON and PNG assets from `Content/`. Distribution builds read them from `Content.dat` in memory. English dialogue and special content use `dialogue.en.json` and `special-islands.en.json`; Japanese originals remain as references.
- **Saving:** JSON records the seed, position, view, progress, generation version, and coordinate-format version. The game's version number, 1.0, is separate from the save's generation version.

### Build and run from source

Development requires the **.NET 8 SDK**. The initial package restore needs access to NuGet.
Run these commands from the repository root:

```powershell
dotnet build src/CAERO.sln -c Debug
dotnet run --project src/CAERO -c Debug
```

For Visual Studio, use Visual Studio 2022 17.8 or later with the **.NET desktop development** workload, and open `src/CAERO.sln`.

The default world seed is `1701`. To start a new game with another seed, specify it at startup. Continue uses the saved seed.

```powershell
dotnet run --project src/CAERO -c Debug -- --seed=48291
```

Use `--no-audio` to skip audio initialization.

Development save locations depend on the build configuration:

| Configuration | Save location |
| --- | --- |
| Debug | `src/SaveData/generated-view.json` |
| Release | `generated-view.json` beside the executable |

Save data and the escape achievement (`achievements.json`) use formatted plain JSON in Debug. Release writes a `CAERO-SAVE:1` header and a Base64-encoded JSON body. Filenames remain unchanged for compatibility. The Release format discourages casual direct editing; it is obfuscation, not encryption. Both configurations can read both formats and write the format appropriate to their configuration.

### Create a distribution

Run from the repository root:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\Release.ps1
```

This creates a self-contained Windows x64 Release build, packages the content, copies the distribution instructions and launcher, and writes `out/CAERO.zip`.

```text
CAERO/
|-- StartGame.bat
|-- Readme.txt
`-- bin/
    |-- CAERO.exe
    |-- Content.dat
    |-- Music/
    |-- Shaders/
    `-- Runtime DLLs and other required files
```

`Content.dat` encrypts the content, including filenames, using AES-256-GCM. It discourages casual browsing but does not guarantee secrecy against executable or memory analysis.
Ordinary `dotnet build` / `dotnet publish` commands do not package the content. Use `Release.ps1` to create the distribution ZIP.

### Validation

The game includes self-tests and rendering checks for generation, movement, saving, UI, and related behavior.

```powershell
# Generation, movement, streaming, and other regression checks
dotnet run --project src/CAERO -c Debug -- --stream-test

# The above plus actual rendering and resizing; requires OpenGL
dotnet run --project src/CAERO -c Debug -- --self-test

# Focused English content and line-clipping checks
dotnet run --project src/CAERO -c Debug -- --dialogue-test

# Compare translated catalogue structure and numerical clue markers; requires Python 3
python tools/verify_translation.py
```

These commands are development checks; players do not need to run them.
English text clipping and layout at different resolutions are reserved for a later visual review. The translation is not shortened to fit the current panels.
