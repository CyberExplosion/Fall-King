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

### Student Information
*Name: Rijul Saxena*   
*Email: rsax@ucdavis.edu*   
*Github: rsax1*

Our team realized early in the production process that our varying schedules would make it hard to pair-program and meet regularly until later in the quarter. In lieu of regular meetings, we made sure to clearly define tasks within the development process and created a Discord group chat as a "drop in" space, where team members would start calls when needed for decision making and debugging. This worked well for us because it was rare that an issue or decision required all hands on deck. 

Some roles had to wait for others before they could begin their work (which was reflected on our original Gantt chart), so those waiting to start their role's task acted as floating helpers to whoever was in progress with their task. Our major logistical issue arose while designing the main player's movement, since we had not fully figured out how we wanted that mechanism to work in relation to the rest of the game. At one point, there was confusion between within the team regarding how to best accomplist the desired player movement feel, and we ended up with two scripts from two authors. We as a team value each member's contribution, so instead of voting or making a hasty descision for which script to use, we deliberated as a team to understand the differences between the two files and hopped on a long call to combine their strengths. 

Organizing and managing the repository was not a struggle for us, since a convention for the structure of the game components was agreed upon beforehand, and clarification was shared continually throughout the development process. One great aspect of our workflow was to only push changes when something was fixed or improved, and to notify the team groupchat when a push to the repo was made (so that others could check for conflicts or be ready to pull before adding their own changes)

## User Interface

### Student Information
*Name: Jehryn Rillon*   
*Email: jmrillon@ucdavis.edu*   
*Github: jmo-gh*

### Menus
Several menus were implemented into Fall King. Menus were implemented as their own scene or as an overlay above `Stages`, our gameplay scene. Scenes are loaded through Unity's Scene Manager using a predefined build order, with the exception of overlayed menus being loaded by name.

#### Main Menu
The main menu (viewable in the `MainMenu` scene) consists of three buttons: [Play](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/MainMenu.cs#L6), [How to Play](https://github.com/CyberExplosion/Fall-King/blob/f2a0007c1a618ef50166676bc54c587740e284ed/FallKing/Assets/Scripts/MainMenu.cs#L11), and [Quit](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/MainMenu.cs#L11). The `Play` button uses Unity's Scene Manager to open the next scene in the build index, as defined in the project's build settings. The `How to Play` button loads the `HowToPlay` scene containing information about game mechanics. Lastly, the `Quit` button simply exits the application.

The UI panels (the background panels for the buttons) were made using a generic 9-spliced white 8 bit sprite, allowing us to reuse the same sprite for various sizes without losing proportioinality. By adjusting the thresholds for the 9-splice within Unity's Sprite Editor and testing different `Pixels per Unit` for the sprite, we were able to achieve the pixel-art like look for the panels.

All other menus within our game are created using the same method since we are able to resize the UI panel as needed.

See the `Resources Used` section for inspiration on the Main Menu implementation.

#### How to Play Menu
The How to Play menu (viewable in the `HowToPlay` scene) consists of core gameplay mechanics as well as tips/lore about the game and a [Back](https://github.com/CyberExplosion/Fall-King/blob/f2a0007c1a618ef50166676bc54c587740e284ed/FallKing/Assets/Scripts/HowToPlay.cs#L8) button that loads the `MainMenu` scene, bringing the player back to the main menu using Unity's Scene Manager.

The design of the How to Play menu was based on the design of main menu, see `Resources Used` for inspiration.

#### Pause Menu
Pausing the game consists of setting Unity's `Time.timeScale` to `0` which stops the flow of time within the `Stages` scene. An `Input Action` called `PauseAction` listens for the user to input `Esc` before invoking [PauseGame()](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/PauseManager.cs#L44) which is responsible for setting `Time.timeScale` to `0`, setting a `boolean paused` to `true` and enabling the `Canvas` component named `Menu` which holds the UI of the pause menu.

The pause menu (viewable by pressing `Esc` during gameplay) consists of two buttons: [Resume](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/PauseManager.cs#L53) and [Give Up](https://github.com/CyberExplosion/Fall-King/blob/5ef40d1162ed6164906d221db77a068eeaa471ec/FallKing/Assets/Scripts/PauseManager.cs#L61). The `Resume` button unfreezes the scene by setting `Time.timeScale` back to `1`, setting the `boolean paused` to `false`, and disabling the `Menu` component from being displayed. The `Give Up` button does the exact opposite of the `Resume` button and loads the `MainMenu` scene.

After the core functionality of the pause menu was implemented, we noticed that the ongoing audio was stilly playing even after pausing the game. We soon discovered that Audio was unaffected by `Time.timeScale` so we had to directly access Unity's `AudioListener` and setting its `pause` field to `true` and `false` within `PauseGame()` and `ResumeGame()`.

Lastly, in order to assign functionality to these buttons we had to create an empty `GameObject PauseSystem` which we then assigned the `PauseManager.cs` script to. Then in Unity's Inspector window for each button, assign each `onClick()` to its respective `PauseGame()` and `ResumeGame()` function.

The design of the pause menu was based on the design of main menu, see `Resources Used` for inspiration.

#### Victory Menu
The victory menu (viewable when player collides with Princess) consists of a button [Return Home](https://github.com/CyberExplosion/Fall-King/blob/702af631bc49e0b5cd21c61c89789b4d5b1fffa9/FallKing/Assets/Scripts/PauseManager.cs#L61) and a UI panel with a informing the player that they have rescued the Princess. The Return Home button uses the same `onClick()` event as the pause menu's Give Up button since they both return the player to the main menu. 

Similar to the pause menu, the victory menu is a `Canvas` object that overlays on top of the camera and by default is disabled. The script [VictoryManager.cs](https://github.com/CyberExplosion/Fall-King/blob/702af631bc49e0b5cd21c61c89789b4d5b1fffa9/FallKing/Assets/Scripts/VictoryManager.cs#L1) is attached to the `GameObject Princess` and monitors when a collision is made with the player. This collision then enables the Victory Menu canvas and overlays the UI on screen.

The design of the victory menu was based on the design of main menu, see `Resources Used` for inspiration.

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
### Student Information
*Name: Rijul Saxena*   
*Email: rsax@ucdavis.edu*   
*Github: rsax1*


To build a world for our game, we drew inspiration from 2D platformers, like the Mario games and Jump King. Our narrative design follows a knight moving deeper and deeper through obstacles in search of his princess. We have multiple Biomes divided between our stages and levels, including forests, underground caves, cold tundras, and volcanic terrain. In order to match these requirements, the Unity Asset Store was a key resource for us to get all of our visuals. 
 
* Knight, clouds, plants, extra terrain elements: https://assetstore.unity.com/packages/2d/environments/flat-platformer-template-108101
* Terrain Tiles: https://assetstore.unity.com/publishers/48778
* Icy visuals: https://assetstore.unity.com/packages/2d/environments/2d-ice-world-106818
* Magnets and other obstacles: https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360


We went through many iterations of asset themes for this project. Initially we thought of sticking to a medieval theme to match our main character, the knight, but instead decided as a team that we wanted more variety and color for a more interesting game feel. To achieve this, we decided on varying terrain types and imported assets according to our narrative plan. It was easy to find sprites for our forest biome, and eventually made tile palettes for the caves and cold weather stages with smaller asset packs. One issue that came up during the importing process was sizing and gameplay interaction, so a recurring topic during team meetings was sizing and assigning mass to elements of our game's world should be in relation to the man character.

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

### Student Information
*Name: Jehryn Rillon*   
*Email: jmrillon@ucdavis.edu*   
*Github: jmo-gh*

Fall King features a Knight tasked with saving the Princess who was been taken underground. Each underground layer features their own set of obstacles in the form of new environmental hazards or enemies. The Knight must strategically and precisely descend down to the Princess to rescue her, dodging platforms and maneuvering around enemies along the way.

The level structure is broken down into 5 stages, with each stage consisting of 3 to 5 levels identified as `STAGE_NUM-LEVEL_NUM` (for example, Level 1 of Stage 1 is denoted as `1-1`). Each Stage is built around its own unique gameplay mechanic and theme, as a result we have a stage outline as follows:

* Stage 1: Tutorial stage, learning the controls
* Stage 2: Sand Biome - learning about enemies, slow zone
* Stage 3: New mechanic: limited visibility (spotlight effect)
* Stage 4: Snow biome -  Freeze and wind zones
* Stage 5: Lava biome - Fire zone + fireballs

One of the challenges that we faced as a team was finding assets that matched the aesthetic and feel that we were trying to achieve. None of us were experienced with animation or visuals with respect to game design, so the idea of creating our own sprites for characters was out of the question. Moreover, we didn't want to buy any assets from the Unity Store so we opted into using the free assets listed on the Unity store that matched our narrative design as much as possible. Due to this limitation of assets, we were not able to reach the creativity and atmosphere for level design that we hoped for.

However, we didn't let this stop us from creating what we thought was the best we could do with what we had available. Following the outline of stages above, we made sure to use assets that following the theme of the stage. For instance, in `Stage 1` we use dirt tiles with a grass layer on top to indicate the beginning of the underground layer. The simplicity of the `Stage 1` tileset allows the player to focus on the main purpose of the stage which is to learn the core gameplay mechanics. As for the following stages, we made sure to use tilesets that match the biome of that respective stage, i.e. for the sand biome we used a desert tileset and the snow biome we used an iceblock tileset.

Aside from assets, we also made the core gameplay mechanic of gliding/descending follow suit with the narrative design of the game. Our initial inspiration was from the rage platformer Jump King, where a Knight has to climb its way up a tower to save a Princess. We decided to take that approach but in the opposite direction where we have to descend towards the Princess. One of the most notable features in Jump King is the simplicity of the gameplay mechanics, so we wanted to make sure that we reached that same level of simplicity by making the movement system easy to understand and simple enough in terms of user input. The main objective of the game is also quite simple, *save the Princess.* Portraying such a simple objective within a game that has very simple mechanics indulges the player to completing due to it being so trivial.

Through these outlets, we were able to create a narrative design around our game that the player can follow and immerse themselves within.

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**



## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**
