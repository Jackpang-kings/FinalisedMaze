# MazeGame-NeaProject

# Introduction
In this project, I would be making a two-dimensional game that would generate a random maze. The objective of the game is to escape from the maze, with the addition of possible mobs and items to interrupt or help the escape from the maze.

Maze game includes different 'mobs' and 'NPCs' to interact with the player who is trying to solve the maze and reach the end goal. These 'NPCs' would include certain behaviour that involves interaction with the player. I would create different random maze algorithms to create different styles of mazes.

## Why I made this
Finding the shortest path has been a universal problem in the world, from finding the shortest path in transport from England to Wales. Finding the shortest path is essentially finding the most efficient way to solve a problem.

One of the reasons I decided to do this project is to further understand how map navigation works. Navigating through unknown areas and only trusting Google Maps is a stressful experience, especially in a foreign country; you never know where you would end up and what path you would take until you have seen the goal. By playing this game, you would hopefully have more strategic and safer ways of moving around an undiscovered area.

## The Purpose
Develop users' strategic and logical thinking while solving an undiscovered area as a human by not relying on external help.

The aim of the project is to create a game that promotes logical thinking and solving the maze using the shortest path possible.

Playing the game helps with acknowledging the position of where you are.

## Target Audience
The audience would be players that love to challenge themselves to get the fastest possible run, often called (Real-time attack speedrun), and players that like to explore unknown territory.

# Player POV
In the User Interface, there should include:
- User input
  - Movement
  - Interact with items
  - Inventory access
- Maze
  - Maze fog
- Item held
- Item in the cell
The user interface should only include information that is essential for the user based on the purpose of the maze game.

## User input
The inputs of the player should allow it to perform the functions of what a player should be able to do.

### Movement
- Up/Down/Left/Right
- Sprint
As this is a two-dimensional game, we would not need to worry about a vertical plane. The mouse cursor would have to be hidden and only revealed when the inventory is opened.

### Interaction
In-game interaction buttons should allow the user to interact with other objects such as:
- Items
- Mobs
- Goal

### Inventory access
To open and view the inventory, a button should be assigned to gain access to it. The mouse cursor should always be hidden when the inventory is not opened.

## Camera angle
The player would always be in the center of the game, creating an effect of:
- 'Dejavu' effect
- Feeling lost in the area

The benefits of these effects to the player are:
- More focused on where they currently are compared to where they started
- Practices spatial awareness

## Maze fog
The maze would only be visible within a certain distance from the center of the player. There should be two stages of fog:
- Half fog
- Full fog

### Half fog
The half fog would reveal the maze structure but hide any other objects:
- Mobs
- Items
- Shops
- Treasures
- Traps

### Full fog
The full fog should hide everything in the maze. The area of the fog should be everything except the half fog and clear space.

# Base of the maze
The game would be programmed using object-oriented programming (OOP). The objects would roughly be mazes, players, mobs, and items. Most programming skills are applied to NPC behaviour, item mechanics, and random graph generation.

## Random Maze Generator
To generate a random structure, we would have to use one of the algorithms below:

Algorithms:
- Depth-first Search Algorithm
- Randomised Prim's Algorithm
- Eller's Algorithm
- Hunt-and-Kill Algorithm
- Binary Tree Algorithm

Example of **Depth-first Search Algorithm**:
```
PROCEDURE Depth-First-Search(Node n)
    IF n is the goal THEN
        RETURN true
    ELSE
        Mark n as visited
        FOREACH Node neighbour in n.neighbourNode
            IF neighbour is "Unvisited"
                Depth-First-Search(_n_)
        ENDFOREACH
    ENDIF
End procedure
```

Example of **Randomised Prim's Algorithm**:
```
The maze starts with a full grid of cells
PROCEDURE PrimsAlgorithm(Cell c)
    List of cells ShuffleList
    c is visited
    Add the neighbourNode of c distanced 2 cells away from c to ShuffleList
    Shuffle the Shuffle List
    nextCell = first cell in ShuffleList
    Break the wall between c and nextCell
    WHILE ShuffleList is not empty
        Cell currentCell = nextCell
        Add the neighbourNode of currentCell distanced 2 cells away from currentCell to ShuffleList
        Shuffle the Shuffle List
        nextCell = first cell in ShuffleList
        Remove the first cell in ShuffleList
    ENDWHILE
```

After creating a maze, it would be stored in a form that can be searched for the best route to the end goal.

Graph is one of the data structures that could store the maze as it handles the cost and direction of the path.

## Objects

### Cells
Cells are the building blocks of the maze, each cell represents one block in the maze.

A cell has:
- Walls
- Method of breaking the walls
- Item storage used to store items

#### Different types of cells:
- Cell (normal)
- Traps
- Goal cell

### Maze
The maze would be created by many cells. For traversal purposes, the maze would be converted into graphs.

> The use of weighted graphs would mostly affect mob behaviour and some collectable items.

- Unidirectional weighted graphs.
- Nodes would represent turning points.
- Edges would be the length of the path, counting the number of cells.

### Players
- Player location (x, y) based on the cell
- Controllable by user input
- Hearts (hit points)
- Movement speed

Commands allow:
- Move only if no wall blocks the path
- Pick or drop items
- Open inventory

Inventory limits:
- Maximum 3 items (including one held)
- Drop or pick items from cells
- Switch items between hand and inventory

#### Player hit detection
When a mob is within range, the player loses hearts. The mob type determines heart loss. Range is the distance at which damage occurs.

### Mobs / NPCs
- Behaviour patterns
- Speed
- Damage
- Range

Ideas:
- Mobs follow players sensed within 3 to 5 cells
- Graph traversal for movement
- Game over if adjacent to the player
- Items or hits given to the player

### Items
- Interactable
- Various functions:

Ideas:
- Compass: Shows the general direction to the goal
- Tip: Reveals 3 cells closest to the goal
- Keys: Grant access to rooms or the goal
- Weapons: Fight back
- Trash: No effect

## Size of the maze

### Rendering
The maze may scale based on size. Ideal rendering keeps:
- Constant cell size
- Limited visible area

Large mazes restrict cell visibility.

# Navigating system
This allows objects (mobs/players) to move through the maze.

### Merging navigation with gameplay
Hints guide the player. Navigation improves efficiency and provides clues.

## Path searching Algorithm

Used for mob movement and goal location.

Algorithms:
- Dijkstra's algorithm
- Depth-first search

### Benefits of Dijkstra's
Shortest pathfinding ensures accuracy without predefined solutions.

## Scripted behaviours
- Maze resets
- Mob spawning
- Random events
- Item placement

# Collision system
Prevents players from passing through walls. Boundary checks enforce movement limits.

# Technology
- C# (text-based)
- Unity (optional)

### Text-based pros:
- Easier collision detection and debugging
- Simpler rendering

### Cons:
- Pixelated graphics

# Unity 2D
Future consideration for:
- Enhanced user experience
- Simplified collision handling

# Further criteria

### Saving system
Stores game progress by saving object states to files.

### Small projects (prototyping):
1. Random maze generation
   - [ ] Maze object
   - [ ] Cells
   - [ ] Goal cells
   - [ ] Algorithms
   - [ ] Display method
2. Path search engine
   - [ ] Any-to-any navigation
   - [ ] Start-to-end navigation
3. Objects
   - [ ] Players
   - [ ] Mobs
   - [ ] NPCs
4. Items
5. Path Search + Objects
   - [ ] Integrate algorithms
6. Interface
7. Maze styles
   - [ ] Size
   - [ ] Shape (circle, hexagon)
8. Game modes
   - [ ] AI races
   - [ ] Battle royale
9. Save system
10. Options
    - Regenerate, pause, resume

# User Requirements

## Objectives

### Mechanics
- Initialization: Maze, mobs, items, player
- Navigation system
- Controls
- Interface
- Win/loss conditions

### Maze functions
- Random routes

### Mobs
- Initialized with behaviour, speed, range, damage

### Items
- Shop, coins, treasure (1-3 items)
- Item types:
  - Compass, apple, shield, mask

### Player
- Health: 5 hearts
- Inventory: 7 items
- Speed control

### Navigation
- Shortest path visualization

### Controls
- Arrow-key movement

### Interface
- Start menu, maze view, health/inventory bars

### Game clear
- Reaching goal cell

### Game over
- Health depletion, timeout, surrender

## Optional
- Fog system
- Real-time interaction with event system

# References
| Title                                     | Link                                                                                      | Remarks |
| ----------------------------------------- | ----------------------------------------------------------------------------------------- | ------- |
| 8 Maze Generating Algorithms in 3 Minutes | https://www.youtube.com/watch?v=sVcB8vUFlmU                                               |         |
| A* Algorithm                              | https://www.youtube.com/watch?v=ySN5Wnu88nE                                               |         |
| Fisher–Yates shuffle Algorithm            | https://www.geeksforgeeks.org/shuffle-a-given-array-using-fisher-yates-shuffle-algorithm/ |         |
| Depth first search                        | https://brilliant.org/wiki/depth-first-search-dfs/                                        |         |

