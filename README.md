# MazeGame-NeaProject
# Introduction
In this project, I would be making a two dimensional game that would generate random maze. The objective of the game is to escape from the maze, with the addition of possible mobs and items to interrupt or help the escape of the maze.

Maze game includes different 'mobs' and 'NPCs' to interact with the player who is trying to solve the maze and reach the end goal. 
These 'NPCs' would include certain behaviour that involves interaction with the player.
I would create different random maze algorithm to create different styles of mazes.

## Why I made this
Finding the shortest path has been a universal problem in the world, from finding the shortest path in transport from England to Wales. Finding the shortest path is essentially finding the most efficient way to solve a problem. 

One of the reasons I decided to do this project is to further understand how map navigation works.
Navigating through unknown areas and only trusting google maps is a stressful experience especially in a foreign country, you never know where you would end up and what path you would take until you have seen the goal. By playing this game, you would hopefully have more strategic and safer ways of moving around an undiscovered area.

## The Purpose
Develop user's strategical and logical thinking while solving an undiscovered area as a human by not relying on external help.

The aim of the project is to create a game that promotes logical thinking and solving the maze using the shortest path possible. 

Playing the game helps with actknowledging the position of where you are.
## Target Audience
The audience would be players that love to challenge themselves to get the fastest possible run, often called (Real-time attack speedrun). And players that like to explore through unknown territory.

# Player POV
In the User Interface, there should include
- User input
	- Movement
	- Interact with items
	- Inventory access
- Maze
	- Maze fog
- Item held
- Item in the cell
The user interface should only include information that is esssential for the user based on the purpose of maze game.
## User input
The inputs of the player should allow it to perform the functions of what a player should be able to do.

### Movement
- Up/Down/Left/Right
- Sprint
As this is a two dimensional game, we would not need to worry about a vertical plane. The mouse cursor would have to be hidden and only reviewed when the inventory is opened.

### Interaction
In game interaction button should allow the user to interact with other objects such as
- Items
- Mobs
- Goal

### Inventory access
To open and view the inventory, a button should be assigned to gain access to it.
The mouse cursor should be always hidden when the inventory is not opened.

## Camera angle
The player would always be in the center of the game, this would have an effect of 
- 'Dejavu' effect
- Feeling lost in the area

The benefits of the effects to the player are
- More focused on where they currently are compared to where they start with
- Practises spacial awareness

## Maze fog
The maze would only be seen in a certain distance away from the centre of the player.
There should be two stage of the fog
- Half fog
- Full fog
### Half fog
The half fog would reveal the maze structure, but hide any other objects
- Mobs
- Items
- Shops
- Treasures
- Traps

### Full fog
The full fog should hide everthing in the maze, the area of the fog should be everything but the area of the half fog and area of the clear space.
# Base of the maze
The game would be programmed with object oriented programming(OOPs). The objects would roughly be mazes, players, mobs, items.
Most of the programming skills are applied in NPCs behaviour, mechanics of the items and random graph generation.

## Random Maze Generator
In order to generate a random structure, we would have to use one of the algorithms below.

Algorithms
- Depth-first Search Algorithm
- Randomised Prim's Algorithm
- Eller's Algorithm
- Hunt-and-Kill Algorithm
- Binary Tree Algorithm

Example of **Depth-first Search Algorithm**
```
PROCEDURE Depth-First-Search(Node n)
	IF n is the goal THEN
	        RETURN true
	ELSE
		Mark n as visited
		FOREACH Node neighbour in n.neighbourNode
			If neighbour is "Unvisited"
				Depth-First-Search( _n_ )
		ENDFOREACH
	ENDIF
End procedure
```

Example of **Randomised Prim's Algorithm**
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
		remove the first cell in ShuffleList
	ENDWHILE
```
After creating a maze, the maze would have to be stored in a form that it could search for the best route to the end goal.

Graph is one of the data structures that could store the maze as it could deal with the cost of the path and direction of the path.

## Objects
### Cells
Cells are the building blocks of the maze, each cell represents one block in the maze.
A cell has
- Walls
- Method of breaking the walls
- Item storage used to store item

#### Different types of cell
- cell (normal)
- traps
- goal cell

### Maze
The maze would be created by many cells. 
For traversal purposes, the maze would be converted into graphs.
>The use of weighted graphs would be mostly for the behaviour of the mobs and some part of the items that could be collected.
- Unidirectional weighted graphs.
- Node would represent a turning point
- Edges would be the length of the path by counting the number of cells

### Players
- Player location (x, y)
- using the location of the cell
- Controllable, by user input
- Hearts (how many times a player could be hit)
- Moving speed
Use of commands to 
- Move the player only if there is not a wall
- Pick or drop an item held
- Open Inventory
The inventory would act as a storage, storing the items the player picked up.
The inventory is limited to 3 items including the one held.
The player should also be able to 
- Drop or pick the item from the inventory or the cell.
- Switch items from the inventory and item holding.

#### How the player is considered hit
When a mob is in a cell close enough to a player, the player would lose a specific amount of heart according to the type of mob it is close to. The distance between the player and the mob is called **range**. If the **range** is met, the player lose heats.

### Mobs / NPCs 
- Behaviours
- Moving speed
- Damage
- Range
Ideas:
- Player would be followed if the mob sense the player in next 3 or 5 cells in any direction
- A graph traversal algorithm to go to the mob
- Game over when mobs is next to the player
- Gives player an item
- The mob would give the player a hit

### Items 
- Interactable with player
- Function (Mostly helps the player going through the maze)
Ideas:
- Compass, gives a general direction of where the goal is located at
- Tip, gives 3 cells closest to the goal
- Keys, gives permission to enter a specific room or end goal
- Weapons, gives player a chance to fight back
- Trash, nothing

## Size of the maze
The size of the maze would affect
- Difficulty of the game
	- Time to solve
	- Amount of route it could take
- Rendering of the game
- How the player sees it
### Rendering
The rendering could either adjust according to the size of the maze by zooming out and matching the borders of the maze. 
But this is would not keep the player in the centre of the screen. If the maze is so large, that the cell is scaling down to a point where it could not be seen. The maximum size of the maze is restricted

The rendering I wish to achieve is
- The size of the cell is constant
- The maze would be revealed as the player moves

The maze would be bigger than what the player could see which makes it a problems as rendering would not be the same anymore.

# Navigating system
The navigating system would allow objects like mobs and players to use to navigate themselves throughout the maze.

## How is navigating going to merge in the game
As the objective of the player is to solve and escape from the maze, there has to be something to guide the player to reach the goal. 
Using a navigating system helps could hint where the goal is and this could affect the efficiency and gives the player some clue.

## Path searching Algorithm
The path searching algorithm is essential for mob behaviours and some of the items. 
The algorithm is either used for searching for the end goal or mobs behaviour to search where the player is.

The types of path searching algorithms I would use is Dijkstra's algorithm, depth-first search algorithm. Both would find routes from a point to another, and helps with differencing the "efficiency" of a route finding algorithm.

## Benefits of Using Dijkstra's algorithm
Finding the shortest path to a target allows to solution to be accurate and unambiguous. 
Since the maze would be random, a predefined solution does not work, as every path and route is different every time, resulting in many redundant code.

## Scripted behaviour in the maze
Behaviours could be implemented for different objects, these behaviours are random events that would happen in a maze. Like
- Maze resetting (Maze structure changes during run-time)
- Mob spawning
- Mob walk area
- Mob communicating to each other
- Item spawning

# Collision system
A restriction have to be made to restrict the player's control of the character of not going through walls.

The player and the walls of the cell would need to have a body that checks its outer boundary and its area.

# Technology using
- C#, text-based
- Unity (optional)
The technologies I am using is C#. The first and second release will be focused on the background programming side, to make sure the technical side of the project is done first.

## Text-based
Advantages of using text-based game
- Objects would have a "collision box" as each characters takes up a space in the screen and they could be detected by checking the text.
- Easier to debug
- Easier to render as the maze is text-based

Disadvantages
- Pixel graphics

## Unity 2D
Unity 2D would be the other engine I will consider using after finishing the first and second release of my project. 
  
Benefits of implementing into Unity 2D
- Improve user experience
- Easier for user to play in the maze
- Not text-based
- Do not have to deal with collisions and rendering issues


# Further criteria
### Saving system
A good game should allow users to play and store their progress.
A way to make a saving system is to take every object in the game, and write it in files.

## Prototyping and critical path
### Small projects
I have separated the game into individual projects
The list of the below 
1. Random maze generation
	- [ ] Maze Object
	- [ ] Cells
	- [ ] Goal Cells
	- [ ] Random maze algorithm
	- [ ] Display method
2. Path searching engine
	- [ ] Navigate from anywhere to anywhere
	- [ ] Start cell to end cell
	- [ ] Anywhere to end cell
3. Other Objects
	- [ ] Player
	- [ ] Mobs
	- [ ] NPCs
4. Objects in cell
5. Path Search + Objects
	- [ ] Implement the path searching into items and objects
6. User interface
7. Different styles of maze
	- [ ] Size of cell and maze
	- [ ] Shape
	- [ ] Circle
	- [ ] Hexagon
	- [ ] Items
	- [ ] Mobs
8. Different game modes
	- [ ] Race with AI
	- [ ] Maze battle royale
9. Save system
	- [ ] Turn all the data in the game and store it into a folder
10. There should be a regenerate option, pause and resume option

# User Requirements

## Objectives

### 1 Game mechanics
- Game initialisation
	- Maze Generating (Functions of the maze met)
	- Mob generation
	- Item generation
	- Player initialise
- Navigating system
- Player control
- User interface
- Game clear
- Game over
### 2 Base of the maze
- Cell (Building blocks)
	- Players could move on it
	- Item(s) could be stored in a cell

### 3 Functions of the maze
- Randomness of the maze
	- The route has to be random everytime
	- With the starting point and goal point the same

### 4 Mob generation
- Mob initialisation
	- Mob damage (hits)
	- Mob moving pattern
	- Movement speed
	- Range
	- Damage

### 5 Item generation
- Shop
	- Allow the player to spent coins to obtain items
- Coin
- Treasure
	- Gives 1 -3 random items
- Items
	- Compass
		- Give 5 cell hints to the goal cell
	- Apple
		- Gives the player 1 heart
	- Shield
		- Gives the player take a hit without losing health
	- Mask
		- Reduce the range of a certain mob

### 6 Player initialisation
- Health of 5 hearts
- Inventory of 7 spaces
	- Could hold a maximum of 7 items
- Movement speed

### 7 Navigating system
- Show the shortest route from one point to the other in the maze
	- Allows mobs and players to use the system
	- Players could gain access to the navigating system through items

### 8 Player controls
- The character could move
	- Up
	- Down
	- Left
	- Right

### 9 User interface
- Start menu
- Show the maze structure in a visual way
	- Visual of the cells
		- Same size
		- Same colour
		- Same wall colour
- Health bar
	- Shows your current health
	- Shows your maximum possible health
- Inventory bar
	- Shows what items you have
	- Shows how many items you have
- Item held
	- Shows features of the item
		- The name
		- Function

### 10 Game clear
- When the goal cell is reached by the player, game ends
	- Reach by a player instead of other objects

### 11 Game over
- Player health reached zero
- Time limit has passed
- Player surrender

## Optional objectives
### 1 Game mechanics 2.0
- Fog system
- Random event system

### 2 Fog system
- Player could only see the area around itself with a certain radius
	- The fog system could be changed using different items
		- Mask decrease the radius 
		- Match increase the radius

### 3 User interface 2.0
- Mapping system
- Fog visualisation
- The game should be able to pause

### 4 Saving system

### 5 Real-time interaction
- The game follows a clock instead of waiting the user to input and outputs it
	- Without the user inputting
		- The mobs would still move
		- Random events would still occur
	- The clock
		- Time passes affects the game
			- Time limit of clearing the game

# References

| Title                                     | Link                                                                                      | Remarks |
| ----------------------------------------- | ----------------------------------------------------------------------------------------- | ------- |
| 8 Maze Generating Algorithms in 3 Minutes | https://www.youtube.com/watch?v=sVcB8vUFlmU                                               |         |
| A* Algorithm<br>                          | https://www.youtube.com/watch?v=ySN5Wnu88nE                                               |         |
| Fisher–Yates shuffle Algorithm            | https://www.geeksforgeeks.org/shuffle-a-given-array-using-fisher-yates-shuffle-algorithm/ |         |
| Depth first search                        | https://brilliant.org/wiki/depth-first-search-dfs/                                        |         |
