# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**


**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replay any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

## Producer

**Describe the steps you took in your role as producer. Typical items include group scheduling mechanism, links to meeting notes, descriptions of team logistics problems with their resolution, project organization tools (e.g., timelines, depedency/task tracking, Gantt charts, etc.), and repository management methodology.**

## User Interface

### Student Information
*Name: Jehryn Rillon*   
*Email: jmrillon@ucdavis.edu*   
*Github: jmo-gh*

### Menus
Several menus were implemented into Fall King. Menus were implemented as their own scene or as an overlay above `Stages`, our gameplay scene. Scenes are loaded through Unity's Scene Manager using a predefined build order, with the exception of overlayed menus being loaded by name.

#### Main Menu
The main menu (viewable in the `MainMenu` scene) consists of three buttons: [Play](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/MainMenu.cs#L6), [How to Play](), and [Quit](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/MainMenu.cs#L11). The `Play` button uses Unity's Scene Manager to open the next scene in the build index, as defined in the project's build settings. The `How to Play` button overlays a canvas on top of the existing canvas in the `MainMenu` scene containing information about game mechanics. Lastly, the `Quit` button simply exits the application.

#### Pause Menu
Pausing the game consists of setting Unity's `Time.timeScale` to `0` which stops the flow of time within the `Stages` scene. An `Input Action` called `PauseAction` listens for the user to input `Esc` before invoking [PauseGame()](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/PauseManager.cs#L44) which is responsible for setting `Time.timeScale` to `0`, setting a `boolean paused` to `true` and enabling the `Canvas` component named `Menu` which holds the UI of the pause menu. 

The pause menu (viewable by pressing `Esc` during gameplay) consists of two buttons: [Resume](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/PauseManager.cs#L53) and [Give Up](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/PauseManager.cs#L61). The `Resume` button unfreezes the scene by setting `Time.timeScale` back to `1`, setting the `boolean paused` to `false`, and disabling the `Menu` component from being displayed. The `Give Up` button does the exact opposite of the `Resume` button and loads the `MainMenu` scene.

After the core functionality of the pause menu was implemented, we noticed that the ongoing audio was stilly playing even after pausing the game. We soon discovered that Audio was unaffected by `Time.timeScale` so we had to directly access Unity's `AudioListener` and setting its `pause` field to `true` and `false` within `PauseGame()` and `ResumeGame()`.

Lastly, in order to assign functionality to these buttons we had to create an empty `GameObject PauseSystem` which we then assigned the `PauseManager.cs` component to. Then in Unity's Inspector window for each button, assign each `onClick()` to its respective `PauseGame()` and `ResumeGame()` function.

The design of the pause menu was based on the design of main menu, see `Resources Used` for inspiration.

### Other UI Elements

#### Stage Name Overlay
When the player enters a new stage (goes from Level `1-3` to `2-1`, or `3-3` to `4-1`) a text overlay appears on the screen with the name of the stage. Not only does this text overlay serve as a visual checkpoint for the player, but it also gives insight into the particular challenges that the player will face that stage.

The text overlay was achieved by first creating a `Canvas` to attach a `TextMeshPro` component to as a child. The `Canvas` is then attached to the `Main Camera` by changing the `Render Mode` to `Screen Space - Camera` in the Inspector window of the `Canvas` component. Then, the `TextMeshPro` component is aligned  vertically and horizontally within the `Canvas` to center the text on the screen.

In order to properly display the text overlay when the player enters a new stage, a script [StageNameOverlay.cs](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/StageNameOverlay.cs#L1) is attached to the `LevelBoundary` of the last level of each stage. `StageNameOverlay.cs` takes two `Serialized Fields` as input: the `TextMeshPro` UI component and a `string` text value that will be overlayed on the screen. Once the `Player` game object collides with the `LevelBoundary` we access the `TextMeshPro` UI component's text field and update it accordingly to the passed `string` text value.

However, just displaying the stage name didn't give off the retro platformer feel we were hoping for. So instead of immediately displaying the stage name on the screen, we decided to give the text overlay some life and apply a typewriter animation. The stage name is displayed letter by letter, giving off that retro vibe due to its association with older technology and nostalgic aesthetics. 

Implementing the typewriter animation required the use of a `Coroutine`, specifically [IEnumerator TypeStageName()](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/StageNameOverlay.cs#L30). `TypeStageName()` uses a `foreach` loop to add each character of the `string` text value to the `TextMeshPro` UI component. After each iteration, a delay of `float typingSpeed` is applied to control the speed at which the text appears on screen. Then after all characters are written out, a delay of `float animationLength` is applied to control how long before a reverse typewriter animation is applied to remove each character from the `TextMeshPro` UI component one at a time. Similar to the addition of each character, there is a delay of `float typingSpeed` to control the speed at which characters are removed from the screen.

`TypeStageName()` is invoked via `StartCoroutine()` in the function [StartTyping()](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/StageNameOverlay.cs#L20). This function checks that a `Coroutine` for `TypeStageName()` doesn't already exist before invoking it.

Lastly, `StartTyping()` is called once the `Player` object collides with the `LevelBoundary` which we check in the [OnTriggerEnter2D](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/StageNameOverlay.cs#L47).

#### Resources Used
[Main Menu](https://www.youtube.com/watch?v=IuuKUaZQiSU)    
[Pause Menu](https://www.youtube.com/watch?v=9tsbUoFfAgo&t=759s)    
[Unity Canvas docs](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/UICanvas.html)


## Movement/Physics

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

## Animation and Visuals

**List your assets including their sources and licenses.**

**Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists.**

## Input

**Describe the default input configuration.**

**Add an entry for each platform or input style your project supports.**

## Game Logic

**Document what game states and game data you managed and what design patterns you used to complete your task.**

# Sub-Roles

## Cross-Platform

**Describe the platforms you targeted for your game release. For each, describe the process and unique actions taken for each platform. What obstacles did you overcome? What was easier than expected?**

## Audio

**List your assets including their sources and licenses.**

**Describe the implementation of your audio system.**

**Document the sound style.** 

## Gameplay Testing

**Add a link to the full results of your gameplay tests.**

**Summarize the key findings from your gameplay tests.**

## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**



## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**
