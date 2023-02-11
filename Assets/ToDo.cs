/*
### Bugs
    
    (Low) A point is still scored when eating with a full belly
    (Low) Two Leaves can be eaten at once, if both of them are clicked

### ToDo:

    Add Sound Toggle
        
### Refactoring:

    Think twice about using a singleton for GameState, possible expose static readonly properties instead
    Use global variables for the player, either using ScriptableObjects (https://www.youtube.com/watch?v=raQ3iHhE_Kk)
        or using a static base class featuring an extendable dictionary.
        You could also use events to register a player in its start method for everyone who wants to know.

### Features:
        Find or make a cool model for the edges of the map
        
        Make Touch more reliable by tracking each single touch Instance
        General Balancing
            Sparse, giant Plants at maxIntensity, little sprouts at start

        - Add shimmery blue Leaves that act as powerups (flight, pierce shot, reload speed, eating speed)
        - Award permanent bonuses for fully grazed plants (speed, ammo consumption [+ effect], ) (later marked on the minimap)
        - Add Wave Events, where one platform is locked and 10 enemies have to be defeated
            - Started when entering a certain area (later marked on the minimap)
            - Started when fully grazing a plant

        - Guitar Sound
        - Make a Minimap with RenderTexture
            - Add POIs to Minimap
    
### Done:

        v 1.4 - in development
        Put the credits right
        SceneManager.activeSceneChanged replaced OnLevelWasLoaded
        Make Bullets travel through leaves (and bustle them around in the process)
        Make Color Arrays that are Lerped from color to color (use https://color.adobe.com/create/color-wheel for color picking)
        Blending Colors is not blended, but digitalized
        If two leaves are clicked at once, the game crashes
        Leaves Shader does not reset after MouseOver
        Rework Shooting - strongly increase turn speed after triggering a shot
        Find the right Intensity Curve

        v 1.3 - released
        The farther, the more intense
        Start up Bug when leaves tried to delete themselves twice
        Mobile Support (Touch)
        Smoothed Shooting behaviour
        Fixed Player starting orientation

        v 1.2 - released
        Tutorials
        Springy Leaves
        Enemy Rigidbody death (Collider change, Bullet Size reduce?)
        Bug: Spawn on Leaf at game start
        Menu Scene Fixed
        Growing Leaves
        Bug: Ins Leere klicken lässt das Spiel abstürzen
        Shoot in State Machine
        Schießen optimieren

        v 1.1 - released
        Procedural
 */