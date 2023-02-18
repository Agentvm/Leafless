/*

// Todo: Remove obsolete calls to Debug.Log & .Warn

### Bugs

    (medium) Player acceleration is not damped in l.199, Movement.cs
    (Reprocuction?) Enemies do not bite
    (Low) A point is still scored when eating with a full belly
    
    
### Features:

    Mobile
        - move
            - make swipe-move feel more intuitive
            - allow tap move
        - see
            - more sight, higher camera
            - enemy indicator at screen border
        - allow shooting on unseen enemies (double-tap)

    Allow to stop eating a leaf
    
    Update Menu (Loose UI and click the leaves instead, hovering some text over them)
    Extend Menu to be used in-game (Esc)
    - Pause
    - Need to remove the Start Game Button

    Find or make a cool model for the edges of the map

    Improve the feeling of shooting, making it more dynamic (speed +)
        
    Make Touch more reliable by tracking each single touch Instance
    General Balancing
        Sparse, giant Plants at maxIntensity, little sprouts at start

    - Add shimmery blue Leaves that act as powerups (flight, pierce shot, reload speed, eating speed)
    - Award permanent bonuses for fully grazed plants (speed, ammo consumption [+ effect], ) (later marked on the minimap)
    - Add Wave Events, where one platform is locked and 10 enemies have to be defeated
        - Started when entering a certain area (later marked on the minimap)
        - Started when fully grazing a plant
        - Wave Events decrease enemy spawn rate (temporarily?)

    - Guitar Sound
    - Make a Minimap with RenderTexture
        - Add POIs to Minimap

### Refactoring:

    Add namespaces for everyone :)
    Think twice about using a singleton for GameState, possible expose static readonly properties instead
    Use global variables for the player, either using ScriptableObjects (https://www.youtube.com/watch?v=raQ3iHhE_Kk)
        or using a static base class featuring an extendable dictionary.
        You could also use events to register a player in its start method for everyone who wants to know.
    

------ ### Done ### ------
    
    Allow moving to a position by (modified) click

    v 1.4.0.1 - released
    (fixed) fix main menu toggle with aspect ratio fitter
    (fixed) Credits broken
    (possibly fixed by f6492db) Two Leaves can be eaten at once, if both of them are clicked
    (fixed) Player does not turn anymore when using mouse
    (fixed) Player can not shoot at empty space

    v 1.4 - released
    Put the credits right
    SceneManager.activeSceneChanged replaced OnLevelWasLoaded
    Make bullets travel through leaves (and bustle them around in the process)
    Make color arrays that are lerped from color to color (use https://color.adobe.com/create/color-wheel for color picking)
    If two leaves are clicked at once, the game crashes
    Leaves shader does not reset after MouseOver
    Rework shooting - strongly increase turn speed after triggering a shot
    Find the right intensity curve
    Added sound toggle
    Added tutorial toggle
    Stability fixes + allow multiple games in a row
    If enemy and leaf are both targeted in one click, prefer enemy action (suppress eating the leaf)
    Added score indicators

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