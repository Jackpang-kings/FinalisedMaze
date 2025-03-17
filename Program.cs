using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;
namespace MazeGame { 
class Program { 
    static void Main() {
        string ch = "";
        Console.WriteLine("T)test script C)Start Classic Game Q)Quit");
        //testScript();
        ch = Console.ReadLine().ToUpper();
        switch (ch) {
            case "T":{
                testScript();
                break;
            }
            case "C":{
                ClassicGame();
                break;
            }
            case "Q":{
                break;
            }
        }
    }
    static void SpeedGame(int i,int j,string t, int playerview){
        
    }
    static void ClassicGame() {
        bool valid = true;
        do {
            string debugMessage = "conversion";
            int difficulty = 0;
            Console.WriteLine("Which difficulty? \n1) Beginner 2) Easy 3) Medium 4) Hard");
            Console.Write("Difficulty: ");
            string ch = Console.ReadLine().ToUpper();
            int.TryParse(ch, out difficulty);
            if (difficulty==0) {
                debugMessage = "level";
            }
            if (difficulty == 1){
                Console.WriteLine("You chose Beginner level");
                Game(9,9,"D",4,null);
                valid = true;
            }else if (difficulty == 2){
                Console.WriteLine("You chose Easy level");
                Game(12,12,"D",3,null);
                valid = true;
            }else if (difficulty == 3){
                Console.WriteLine("You chose Medium level");
                Game(21,21,"P",2,null);
                valid = true;
            }else if (difficulty == 4){
                Console.WriteLine("You chose Hard level");
                Game(30,30,"P",1,null);
                valid = true;
            }else{
                valid = false;
                Console.WriteLine("Invalid "+debugMessage);
            }
        }while(valid==false);
    }
    static void Game(int i,int j,string t, int playerview,List<object> gameObjects) {
        MazeGrid gameboard = new MazeGrid(i,j);
        // Initialising cells in the maze
        int goalx = gameboard.GetEndX();
        int goaly = gameboard.GetEndY();
        gameboard.InitialiseMaze();
        if (t=="P"){
            // Generating maze with Prim's Algorithm
            gameboard.CreatePrimsMaze();
            Console.WriteLine("Generated maze using Prim's Algorithm");
        }else{
            // Generating maze with depth-first search
            gameboard.CreateDFSMaze();
            Console.WriteLine("Generated maze using Depth-first traversal");
        }
        if (i<31){
                Console.WriteLine($"{gameboard.MazeNoNumPrint()}");
        }else{
            Console.WriteLine("Too wide to print the whole maze");
        }
        
        Console.WriteLine("Read Entire maze from "+Environment.CurrentDirectory+"/game.txt");
        // Output gameTextfile.txt
        OutputTextFile(gameboard.MazeNoNumPrint());


        // Starting pos
        int startx = gameboard.PickRandomNum((i-1)/3);
        int starty = gameboard.PickRandomNum((j-1)/3);
        // Move GameObject in maze
        Player player = new Player(startx,starty,"Player1",7); 
        GameObject box = new GameObject(1, 2, "Box",'B', false);
        GameObject key = new GameObject(2, 3, "Key",'K', false);
        Navigator compass= new Navigator(0,1,"Compass",new PathFinder(gameboard,new Graph(new List<Node>())),5);
        GameObject goal = new GameObject(goalx,goaly,"Goal Indicator", 'G',false);
        PathFinder pathFinder = new PathFinder(gameboard,new Graph()); // Assuming PathFinder has a constructor that takes a MazeGrid
        Mob mob = new Mob(5, 5, "Mob", 1, 3, 10, pathFinder); // Mob starts at (5,5)
        string message = "not moved";
        Cell startCell = gameboard.GetMazeCell(startx,starty);

        // Place the GameObject in (0,0)
        //startCell.SetGameObject(player);
        gameboard.AddObject(player);
        gameboard.AddObjects(new List<object> {compass,box,key,goal,mob});
        Cell endCell = gameboard.GetMazeCell(gameboard.Width()-1,gameboard.Height()-1);

        Cell nextCell;
        int radius = playerview;
        
        while (message!="PAUSE"&&message!="CLEAR"){
            // Instructions for inputs
            Console.WriteLine("Arrow keys to move, E) Check if you are at goal D) To find path to goal\n\nTab) To open Inventory Spacebar) To use item holding X) To check for interactions");
            (nextCell,message) = Input(gameboard,startCell,endCell,player);
            Console.Clear();
            startCell = nextCell;
            mob.Tick(startCell);
            Console.WriteLine(gameboard.PrintCameraAngle(startCell,radius));
            //Console.WriteLine(gameboard.PrintAddGameObjects(gameboard,gameboard.MazeNoNumPrint()));
            Console.WriteLine(message);
        }
        Console.ReadKey();
    } 
    static void OutputTextFile(string print){
        string path = Environment.CurrentDirectory+"/game.txt";
        //Console.WriteLine(path);
        // Write file using StreamWriter
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(print);
            writer.Close();
        }
    }
    static void testScript(){
        //testCell();
        //testMazeGrid();
        //testMazeGridAndDijkstra();
        
        //testGameObject();
        //testGameObjectAndCell();
        //testEnqueue();
        //MazeGrid dfsmaze = testCreateRandomDFSMaze(10,10);
        //testDijkstra(dfsmaze);
        //testBFS(dfsmaze);
        //MazeGrid primsMaze = testCreateRandomPrimsMaze(40,40);
        //testDijkstra(primsMaze);
        //testBFS(primsMaze);
        //testHeader();
        //GameBreadthFirst(testCreateRandomGraph(4,4));
        //testCompareCell();
        //OutputTextFile(testCreateRandomDFSMaze(10,10));
        //testPlayer();
        testMob();
        TestMobPatrolsWhenPlayerIsOutOfRange();
    }
    static void testMob(){
            // Arrange
            var maze = new MazeGrid(10, 10); // Assuming MazeGrid has a constructor that takes width and height
            var player = new Player(6,5,"TestPlayer",7);
            maze.InitialiseMaze();
            maze.CreateDFSMaze();
            maze.ClearWall(maze.GetMazeCell(player.x, player.y),maze.GetMazeCell(5, 5));
            maze.AddObject(player);
            var pathFinder = new PathFinder(maze,new Graph()); // Assuming PathFinder has a constructor that takes a MazeGrid
            var mob = new Mob(5, 5, "TestMob", 1, 3, 10, pathFinder); // Mob starts at (5,5)
            maze.AddObject(mob);
            
            Console.WriteLine(maze.MazePrintWithGmObj(maze));
            var currentCell = maze.GetMazeCell(player.x,player.y); // Get the current cell of the player
            // Act
            mob.Tick(currentCell);
            mob.Tick(currentCell);
            Console.WriteLine(maze.MazePrintWithGmObj(maze));
            Console.WriteLine($"Following: {mob.GetFollowing().ToString()}");
            Console.WriteLine($"{mob.x},{mob.y}");
            // Additional checks can be added here to verify the path taken by the mob
        }

    static void TestMobPatrolsWhenPlayerIsOutOfRange()
        {
            // Arrange
            var maze = new MazeGrid(10, 10);
            var player = new Player(8,8,"TestPlayer",7);
            maze.InitialiseMaze();
            maze.CreateDFSMaze();
            maze.AddObject(player);
            var pathFinder = new PathFinder(maze,new Graph()); // Assuming PathFinder has a constructor that takes a MazeGrid
            var mob = new Mob(5, 5, "TestMob", 1, 3, 10, pathFinder); // Mob starts at (5,5)
            maze.AddObject(mob);
            
            Console.WriteLine(maze.MazePrintWithGmObj(maze));
            var currentCell = maze.GetMazeCell(player.x,player.y); // Get the current cell of the player
            // Act
            mob.Tick(currentCell);
            mob.Tick(currentCell);
            Console.WriteLine(maze.MazePrintWithGmObj(maze));
            Console.WriteLine($"Following: {mob.GetFollowing().ToString()}");
            Console.WriteLine($"{mob.x},{mob.y}");
            // Assert
            // Additional checks can be added here to verify the new position of the mob after patrolling
        }
    static void testPlayer(){
        MazeGrid m = testCreateRandomPrimsMaze(3,3);
        Player player = new Player(0,0,"Player1",7); 
        Navigator n = new Navigator(new PathFinder(m,new Graph()));
        GameObject g = new GameObject(0,0,"Object",'O',false);
        m.AddObjects([n,g]);
        player.AddItem(n);
        Console.WriteLine(player.DisplayInventory());
        //player.GetInventory
    }
    static void testCompareCell(){
        Cell cell1 = new Cell(0,0);
        Cell cell2 = new Cell(1,0);
        Cell cell3 = new Cell(0,1);
        Cell cell4 = new Cell(1,1);
        if (CompareCell(cell1,cell2)=="LEFT"){
            Console.WriteLine("TRUE");
        }else{
            Console.WriteLine("FALSE");
        }
        if (CompareCell(cell2,cell1)=="RIGHT"){
            Console.WriteLine("TRUE");
        }else{
            Console.WriteLine("FALSE");
        }
        if (CompareCell(cell1,cell3)=="UP"){
            Console.WriteLine("TRUE");
        }else{
            Console.WriteLine("FALSE");
        }
        if (CompareCell(cell2,cell4)=="UP"){
            Console.WriteLine("TRUE");
        }else{
            Console.WriteLine("FALSE");
        }
    }
    static void testCell(){
        Console.WriteLine("Class cell test:");
        Cell cell1= new Cell(1,1,false);
        Cell cell2= new Cell(9,0,true);
        Cell[] cells = {cell1,cell2};
        cell1.RightWall = false;
        cell2.LeftWall = false;
        cell2.Visited(true);
        Console.WriteLine(cell1.ToString());
        Console.WriteLine(cell2.ToString());
        Console.WriteLine();
        MazeGrid mazeGrid= new MazeGrid(1,1);
        Console.Write(mazeGrid.PrintCellFrontWall(cells));
        Console.Write(mazeGrid.PrintCellLeftRightWall(cells));
        Console.Write(mazeGrid.PrintCellBackWall(cells));
        Console.WriteLine();
    }
    static void testMazeGrid(){
        MazeGrid m = new MazeGrid(3,3);
        m.InitialiseMaze();
        m.GetMazeCell(0,0).FrontWall = false;
        m.GetMazeCell(2,2).BackWall = false;
        m.GetMazeCell(2,2).Goal(true);
        m.ClearWall(m.GetMazeCell(0,0),m.GetMazeCell(1,0));
        m.ClearWall(m.GetMazeCell(1,0),m.GetMazeCell(2,0));
        m.ClearWall(m.GetMazeCell(2,0),m.GetMazeCell(2,1));
        m.ClearWall(m.GetMazeCell(2,1),m.GetMazeCell(2,2));
        m.ClearWall(m.GetMazeCell(1,0),m.GetMazeCell(1,1));
        m.ClearWall(m.GetMazeCell(0,1),m.GetMazeCell(1,1));
        m.ClearWall(m.GetMazeCell(0,1),m.GetMazeCell(0,2));
        m.ClearWall(m.GetMazeCell(0,2),m.GetMazeCell(1,2));
        m.ClearWall(m.GetMazeCell(1,2),m.GetMazeCell(2,2));
        //Console.WriteLine(m.GetMazeCell(2,2).ToString());
        //Console.WriteLine(m.MazePrint());
        Console.WriteLine(m.MazeNoNumPrint());
        m.SetAllCellNotVisited();
        //Give cells their neighbours
        m.SetConnectedCells();
        //Print their neighbour out
        //Console.WriteLine(m.PrintConnectedNeighbours());
        //Console.WriteLine(m.MazePrint());

        GameObject[] objects = {new GameObject(0,0,"One",'1',true),new GameObject(1,0,"Two",'2',true),new GameObject(2,0,"Three",'3',true),
        new GameObject(0,1,"Four",'4',true),new GameObject(1,1,"Five",'5',true),new GameObject(2,1,"Six",'6',true),
        new GameObject(0,2,"Seven",'7',true),new GameObject(1,2,"Eight",'8',true),new GameObject(2,2,"Nine",'9',true)};

        string mazePrint = m.MazeNoNumPrint();
        char[] cells = mazePrint.ToCharArray();
        foreach (GameObject obj in objects){
            if (obj.x<m.Width()&&obj.x>=0&&obj.y<m.Height()&&obj.y>=0){
                int x = Math.Abs(m.GetMazeCell(0,0).X() - obj.x);
                int y = Math.Abs(m.GetMazeCell(0,0).Y() - obj.y)+1;
                int row = (m.Width()*5+1)*(1+2*(y-1))+2+x*5;
                cells[row] = obj.GetLabel();
            }
        }
        string newPrint = "";
        foreach (char c in cells){
            newPrint+=c;
        }
        Console.WriteLine(newPrint);
    }
    static void testMazeGridAndDijkstra() {
        MazeGrid mazeBoard = new MazeGrid(5, 5);
        mazeBoard.InitialiseMaze();
        mazeBoard.GetMazeCell(0, 0).FrontWall = false;
        mazeBoard.GetMazeCell(4, 4).BackWall = false;
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(0, 0), mazeBoard.GetMazeCell(1, 0));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(1, 0), mazeBoard.GetMazeCell(2, 0));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(4, 0), mazeBoard.GetMazeCell(3, 0));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(3, 0), mazeBoard.GetMazeCell(4, 0));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(0, 0), mazeBoard.GetMazeCell(0, 1));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(1, 0), mazeBoard.GetMazeCell(1, 1));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(2, 0), mazeBoard.GetMazeCell(1, 1));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(3, 0), mazeBoard.GetMazeCell(3, 1));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(1, 1), mazeBoard.GetMazeCell(2, 1));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(3, 1), mazeBoard.GetMazeCell(4, 1));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(0, 1), mazeBoard.GetMazeCell(0, 2));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(2, 1), mazeBoard.GetMazeCell(2, 2));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(4, 1), mazeBoard.GetMazeCell(4, 2));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(0, 2), mazeBoard.GetMazeCell(0, 3));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(1, 2), mazeBoard.GetMazeCell(1, 3));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(2, 2), mazeBoard.GetMazeCell(2, 3));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(3, 2), mazeBoard.GetMazeCell(3, 3));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(4, 2), mazeBoard.GetMazeCell(4, 3));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(0, 3), mazeBoard.GetMazeCell(0, 4));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(1, 3), mazeBoard.GetMazeCell(1, 4));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(3, 3), mazeBoard.GetMazeCell(3, 4));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(1, 3), mazeBoard.GetMazeCell(2, 3));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(3, 3), mazeBoard.GetMazeCell(4, 3));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(0, 4), mazeBoard.GetMazeCell(1, 4));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(2, 4), mazeBoard.GetMazeCell(3, 4));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(3, 4), mazeBoard.GetMazeCell(4, 4));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(3, 4), mazeBoard.GetMazeCell(4, 4));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(1, 4), mazeBoard.GetMazeCell(2, 4));
        mazeBoard.ClearWall(mazeBoard.GetMazeCell(2, 0), mazeBoard.GetMazeCell(3, 0));
        testDijkstra(mazeBoard);
        }
    static void testEnqueue(){
        DistanceQueue q = new DistanceQueue();
        int j = 2;
        int i = 1;
        Node n = new Node(1,1,1,new List<Edge>());
        Node n2 = new Node(3,10,10,new List<Edge>());
        q.Enqueue(n,i);
        q.Enqueue(n2,j);
        // change variable i
        i = 5;
        // n2 should be out of the queue first as it has the smallest priority
        q.UpdateQueue(n,i);
        Node nOut3 = q.Dequeue();
        Console.WriteLine(nOut3.ToString());
    }
    static void testDijkstra(MazeGrid maze){
        Graph graph = new Graph(new List<Node>());


        // Step 1: Print maze
        //Console.WriteLine(maze.MazePrint());

        // Step 2: Create a PathFinder object
        PathFinder pathFinder = new PathFinder(maze,graph);

        // Step 3: Link nodes and relationships and Print nodes
        pathFinder.LinkNodeRelationships();
        //Console.WriteLine(graph.PrintNodeList(graph.GetNodes()));

        // Step 4: Perform Dijkstra's Algorithm between two nodes
        Cell startCell = maze.GetMazeCell(0,0);
        Cell endCell = maze.GetMazeCell(maze.Width()-1,maze.Height()-1);
        Node startNode = pathFinder.Cell2Node(startCell);  // Top-left corner
        Node endNode = pathFinder.Cell2Node(endCell);    // Bottom-right corner
        
        var solution = pathFinder.graph.DijkstraAlgorithm(startNode,endNode);

        // Step 5: Output the solution
        List<Node> nodes = solution.Item1;
        int[] distances = solution.Item2;

        int counter = 0;
        foreach (int d in distances){
            if (d!=int.MaxValue){
                counter++;
            }
        }
        Console.WriteLine("Dijkstra's Algorithm Path:");
        Console.WriteLine(pathFinder.PrintMazeSolution(nodes));
        Console.WriteLine(PrintSolution(nodes,distances,endNode));
        Console.WriteLine($"Distance travelled: {distances[endNode.getNodeID()]}");
        Console.WriteLine($"Total nodes: {graph.GetNodes().Count}");
        Console.WriteLine($"Number of nodes traversed: {counter}");

    }
    static void testBFS(MazeGrid maze){
        Graph graph = new Graph(new List<Node>());


        // Step 1: Print maze
        //Console.WriteLine(maze.MazePrint());

        // Step 2: Create a PathFinder object
        PathFinder pathFinder = new PathFinder(maze,graph);

        // Step 3: Link nodes and relationships and Print nodes
        pathFinder.LinkNodeRelationships();
        //Console.WriteLine(graph.PrintNodeList(graph.GetNodes()));

        // Step 4: Perform Dijkstra's Algorithm between two nodes
        Cell startCell = maze.GetMazeCell(0,0);
        Cell endCell = maze.GetMazeCell(maze.Width()-1,maze.Height()-1);
        Node startNode = pathFinder.Cell2Node(startCell);  // Top-left corner
        Node endNode = pathFinder.Cell2Node(endCell);    // Bottom-right corner

        var solution = pathFinder.graph.BreadthFirstTraversal(startNode,endNode);

        // Step 5: Output the solution
        List<Node> nodes = solution.Item1;
        int[] distances = solution.Item2;
        Console.WriteLine("Breadth-First Traversal Path:");
        Console.WriteLine(pathFinder.PrintMazeSolution(nodes));
        Console.WriteLine(PrintSolution(nodes,distances,endNode));
    }
    static string PrintSolution(List<Node> nodes,int[] distances, Node endNode) {
        string message ="";
        // header row
        message += "No. | ID |(x,y)  |Distance\n";
        message += "----+----+-------+--------\n";


        for (int i = 0;i < nodes.Count;i++){
            Node node = nodes[i];
            message += $"{i+1,-4}|{node.getNodeID(),-4}|{node.X(),-2}, {node.Y(),-2} |{distances[node.getNodeID()],-3}\n";
        }
        message += $"Total Distance: {distances[endNode.getNodeID()]}";
        return message;
    }
    static MazeGrid testCreateRandomDFSMaze(int width,int height){
        Console.WriteLine("Class Pathfinder test using random maze and graph structure");
        MazeGrid mazeBoard2 = new MazeGrid(width,height);
        mazeBoard2.InitialiseMaze();
        mazeBoard2.CreateDFSMaze();
        Console.WriteLine(mazeBoard2.MazePrint());
        return mazeBoard2;
    }
    static MazeGrid testCreateRandomPrimsMaze(int width,int height){
        Console.WriteLine("Class Pathfinder test using random maze and graph structure");
        MazeGrid mazeBoard = new MazeGrid(width,height);
        mazeBoard.InitialiseMaze();
        mazeBoard.CreatePrimsMaze();
        Console.WriteLine(mazeBoard.MazePrint());
        return mazeBoard;
    }
    static void testHeader() {
        string mazeprintmessage = "   ";
        for (int i = 0; i < 41; i++)
        {
            mazeprintmessage += $"{Convert.ToString(i),3}  ";
        }
        Console.WriteLine(mazeprintmessage);
    }
    static void testGameObject(){
        // Create some GameObject instances
        GameObject wall = new GameObject(0, 0, "Wall",'W', false);
        GameObject box = new GameObject(1, 2, "Box",'B', true);
        GameObject key = new GameObject(2, 3, "Key",'K', false);

        // Test the ToString method
        Console.WriteLine(wall.ToString());
        Console.WriteLine(box.ToString());
        Console.WriteLine(key.ToString());

        // Test getters and setters
        Console.WriteLine("\nTesting properties and methods:");
        Console.WriteLine($"Box Name: {box.GetName()}");
    }
    static void testGameObjectAndCell(){
        // Create GameObject instances
        GameObject treasure = new GameObject(2, 3, "Treasure",'T', false);
        GameObject monster = new GameObject(1, 1, "Monster",'M', false);

        // Create Cell instances
        Cell cell1 = new Cell(0, 0);
        Cell cell2 = new Cell(1, 1, false, monster);
        Cell cell3 = new Cell(2, 3, true, treasure);

        // Test Cell ToString
        Console.WriteLine("Cell 1:");
        Console.WriteLine(cell1.ToString());

        Console.WriteLine("\nCell 2:");
        Console.WriteLine(cell2.ToString());

        Console.WriteLine("\nCell 3:");
        Console.WriteLine(cell3.ToString());

        // Test adding connected cells
        cell1.AddConnectedCell(cell2);
        cell2.AddConnectedCell(cell3);

        Console.WriteLine("\nConnected cells for Cell 1:");
        foreach (var connectedCell in cell1.GetConnectedCells())
        {
            Console.WriteLine(connectedCell.ToString());
        }
    }
    static MazeGrid GameDijkstra(MazeGrid maze,Cell currentCell,Cell endCell){           
        // Step 1: Create a PathFinder object
        Graph graph = new Graph(new List<Node>());
        PathFinder pathFinder = new PathFinder(maze,graph);
        pathFinder.LinkNodeRelationships();

        // Step 2: Perform Dijkstra's Algorithm between two nodes
        Node startNode = pathFinder.Cell2Node(currentCell);  // Top-left corner
        Node endNode = pathFinder.Cell2Node(endCell);    // Bottom-right corner
        var solution = pathFinder.graph.DijkstraAlgorithm(startNode,endNode);

        // Step 3: Output the solution
        List<Node> nodes = solution.Item1;
        for (int i = 0; i < nodes.Count-1;i++){
            int x = nodes[i+1].X();
		    int y = nodes[i+1].Y(); 
		    Cell nextCell = maze.GetMazeCell(x,y);
            maze.AddGameObject(new GameObject(x,y,"Mark "+i,'#',false));
		    //nextCell.SetGameObject(new GameObject(x,y,"Mark "+0,'#',false,false));
		    int count = graph.GetDistanceBetweenNodes(nodes[i],nodes[i+1]);
		    string direction = CompareCell(currentCell,nextCell);
		
		// Repeat n times in specific direction by comparing two nodes
            for(int j = 1; j < count;j++){
                int a = x;
                int b = y;
                if (direction=="LEFT"){
                    a = x-j;
                }else if (direction=="RIGHT"){
                    a = x+j;
                }else if (direction=="UP"){
                    b = y-j;
                }else if (direction=="DOWN"){
                    b=y+j;
                }
                // Sets object marks
                //nextCell = maze._mazeGrid[a,b];
                maze.AddGameObject(new GameObject(a,b,"Mark "+i,'#',false));
                //nextCell.SetGameObject(new GameObject(a,b,"Mark "+j,'#',false,false));
            }
		    currentCell = maze.GetMazeCell(x,y);
        }
			return maze;
        }
    static string CompareCell(Cell currCell,Cell nextCell){
            string message = "";
            if (nextCell!=null){
                if (nextCell.X() > currCell.X()){
                    message = "LEFT";
                }else if (nextCell.X() < currCell.X()){
                    message = "RIGHT";
                }else if (nextCell.Y() > currCell.Y()){
                    message = "UP";
                }else if (nextCell.Y() < currCell.Y()){
                    message = "DOWN";
                }
            }
            return message;
        }
    static void GameBreadthFirst(MazeGrid maze){           
        // Step 1: Create a PathFinder object
        Graph graph = new Graph(new List<Node>());
        PathFinder pathFinder = new PathFinder(maze,graph);
        pathFinder.LinkNodeRelationships();

        // Step 2: Perform Dijkstra's Algorithm between two nodes
        Cell startCell = maze.GetMazeCell(0,0);
        Cell endCell = maze.GetMazeCell((maze.Width()-1)/2,(maze.Height()-1));
        Node startNode = pathFinder.Cell2Node(startCell);  // Top-left corner
        Node endNode = pathFinder.Cell2Node(endCell);    // Bottom-right corner
        var solution = pathFinder.graph.BreadthFirstTraversal(startNode,endNode);

        // Step 3: Output the solution
        List<Node> nodes = solution.Item1;
        int[] distances = solution.Item2;
        
        int counter = 0;
        foreach (Node n in graph.GetNodes()){
            if (n.IsVisited()){
                counter++;
            }
        }
        Console.WriteLine(pathFinder.PrintMazeSolution(nodes));
        //Console.WriteLine(PrintSolution(nodes,distances,endNode));
        Console.WriteLine($"Distance travelled: {distances[endNode.getNodeID()]}");
        Console.WriteLine($"Total nodes: {graph.GetNodes().Count}");
        Console.WriteLine($"Number of nodes traversed: {counter}");
    }
    static (Cell,string) Input(MazeGrid gameBoard,Cell currentCell,Cell endCell, Player player){
        System.ConsoleKeyInfo input = Console.ReadKey();
        Cell nextCell = currentCell;
        //Console.Clear();
        string message = "Not moved";
        if (input.Key == ConsoleKey.UpArrow){
            Moving(player,0,-1);
        }else if (input.Key == ConsoleKey.DownArrow){
            Moving(player,0,1);
        }else if (input.Key == ConsoleKey.LeftArrow){
           Moving(player,-1,0);
        }else if (input.Key == ConsoleKey.RightArrow){
            Moving(player,1,0);
        }else if (input.Key == ConsoleKey.Escape){
            message = "PAUSE";
        }else if (input.Key == ConsoleKey.E){
            message = GameClear(currentCell);
        }else if (input.Key == ConsoleKey.X){
            message ="Checking for interactions\n";
            CheckInteractionForAllTypesOfGameObject();
        }else if (input.Key == ConsoleKey.Tab){
            Console.WriteLine(player.DisplayInventory());
            Console.WriteLine("Press tab to exit, Number to chose the item to hold");
            System.ConsoleKeyInfo ch = Console.ReadKey();
            Console.WriteLine(player.CheckHold(ch.KeyChar));
        }else if (input.Key==ConsoleKey.Spacebar){
            Console.WriteLine("Using "+player.GetItemHeld());
            UseItem();
            
        }
        return (nextCell,message);
        void Moving(object control, int a,int b){
            if (gameBoard.MoveValid(currentCell,a,b)){
                if (control.GetType() == typeof(Player)){
                    Player player = (Player)control;
                    player.Move(currentCell.X()+a,currentCell.Y()+b);
                    
                }else if (control.GetType() == typeof(Mob)){
                    Mob mob = (Mob)control;
                    mob.Move(currentCell.X()+a,currentCell.Y()+b);
                }else if (control.GetType()== typeof(GameObject)){
                    GameObject obj = (GameObject)control;
                    obj.Move(currentCell.X()+a,currentCell.Y()+b);
                }
                nextCell = gameBoard.GetMazeCell(currentCell.X()+a,currentCell.Y()+b);
                message = "Moved to "+$"x:{nextCell.X()},y:{nextCell.Y()}";
            }
        }
        void CheckInteractionForAllTypesOfGameObject(){
            bool found = false;
            int i = 0;
            while (i<gameBoard.gameObjects.Count&&found==false){
                if (gameBoard.gameObjects[i] is Tool){
                    Tool obj = (Tool) gameBoard.gameObjects[i];
                    if (obj.x==player.x&&obj.y==player.y){
                        found = true;
                        player.AddItem(obj);
                        message+="Picking up "+obj.name;
                    }
                   
                }else if (gameBoard.gameObjects[i] is Navigator){
                    Navigator obj = (Navigator) gameBoard.gameObjects[i];
                    if (obj.x==player.x&&obj.y==player.y){
                        found = true;
                        player.AddItem(obj);
                        message+="Picking up "+obj.name;
                    }
                }else if (gameBoard.gameObjects[i] is Weapon){
                    Weapon obj = (Weapon) gameBoard.gameObjects[i];
                    if (obj.x==player.x&&obj.y==player.y){
                        found = true;
                        player.AddItem(obj);
                        message+="Picking up "+obj.name;
                    }
                }
                i++;
            }
        }
        void UseItem(){
            if (player.GetItemHeld() is Navigator){
                Navigator n = (Navigator)player.GetInventory(0);
                gameBoard.AddObjects(n.pathFinder.ReturnPath(player.x,player.y,n.x,n.y,n.GetDistance()));
                //player.Remove(0);
            }else if (player.GetItemHeld() is Tool){
                Tool n = (Tool)player.GetInventory(0);
                
            }else if (player.GetItemHeld() is Food){
                Food n = (Food)player.GetInventory(0);
                player.AddHealth(n.GetHeal());
            }
        }
    }
    static string GameClear(Cell c){
        if (c.IsGoal()){
            return "CLEAR";
        }else{
            return "Not there yet";
        }
    }  
    }
}