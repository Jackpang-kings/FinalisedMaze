using System;
using System.Reflection.Metadata;
namespace MazeGame { 
class Program { 
    static void Main() {
        string ch = "G";
        Console.WriteLine("T)test script G)Start Game Q)Quit");
        //testScript();
        //ch = Console.ReadLine().ToUpper();
        switch (ch) {
            case "T":{
                testScript();
                break;
            }
            case "G":{
                Game();
                break;
            }
            case "Q":{
                break;
            }
        }
    }
    static void Game() {
        bool valid = false;
        int[] intcommand = new int[2];
        do {
            try{
                Console.Write("Maze size (width,depth): ");
                string size = Console.ReadLine();
                string[] strcommand = size.Split(",");
                for (int i = 0; i < strcommand.Length;i++){
                    intcommand[i] = int.Parse(strcommand[i]);
                }
                valid = true;
            }catch{
                Console.WriteLine("Invalid choice");
            }
        }while(valid==false);
        MazeGrid gameboard = new MazeGrid(intcommand[0],intcommand[1]);
        // Initialising cells in the maze
        gameboard.InitialiseMaze();
        Console.WriteLine("Empty maze");
        Console.WriteLine(gameboard.MazeNoNumPrint());
        Console.Write("Enter to continue...");
        Console.ReadKey();

        Console.WriteLine("D)Depth-first trarversal P)Prim's Algorithm Q)Quit");
        string ch = Console.ReadLine().ToUpper();
        switch (ch){
            case "D":{
                // Generating maze with depth-first search
                gameboard.CreateDFSMaze();
                Console.WriteLine();
                Console.WriteLine("Generated maze using Depth-first traversal");
                Console.WriteLine(gameboard.MazeNoNumPrint());
                Console.Write("Enter to continue...");
                Console.ReadKey();
                break;
            }
            case "P":{
                // Generating maze with Prim's Algorithm
                gameboard.CreatePrimsMaze();
                Console.WriteLine();
                Console.WriteLine("Generated maze using Prim's Algorithm");
                Console.WriteLine(gameboard.MazeNoNumPrint());
                Console.Write("Enter to continue...");
                Console.ReadKey();
                break;
            }
        }

        Console.WriteLine("Dijkstra's Algorithm Path:");
        GameDijkstra(gameboard);
        Console.Write("Enter to continue...");
        Console.ReadKey();

        Console.WriteLine("Breadth First Algorithm Path:");
        GameBreadthFirst(gameboard);
        Console.Write("Enter to continue...");
        Console.ReadKey();

    }
    static void testScript(){
        //testCell();
        //testMazeGrid();
        //testMazeGridAndDijkstra();
        //testCreateRandomGraph(10,10);
        //testGameObject();
        //testGameObjectAndCell();
        //testEnqueue();
        MazeGrid dfsmaze = testCreateRandomDFSMaze(40,40);
        //testDijkstra(dfsmaze);
        //testBFS(dfsmaze);
        MazeGrid primsMaze = testCreateRandomPrimsMaze(40,40);
        testDijkstra(primsMaze);
        testBFS(primsMaze);
        //testHeader();
        //GameBreadthFirst(testCreateRandomGraph(4,4));
        Console.ReadKey();
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
        Console.WriteLine(m.GetMazeCell(2,2).ToString());
        Console.WriteLine(m.MazePrint());
        Console.WriteLine(m.MazeNoNumPrint());
        m.SetAllCellNotVisited();
        //Give cells their neighbours
        m.SetConnectedCells();
        //Print their neighbour out
        Console.WriteLine(m.PrintConnectedNeighbours());
        Console.WriteLine(m.MazePrint());
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
        Console.WriteLine("Dijkstra's Algorithm Path:");
        Console.WriteLine(pathFinder.PrintMazeSolution(nodes));
        Console.WriteLine(PrintSolution(nodes,distances,endNode));

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
        GameObject wall = new GameObject(0, 0, "Wall", false, false);
        GameObject box = new GameObject(1, 2, "Box", true, true);
        GameObject key = new GameObject(2, 3, "Key", true, false);

        // Test the ToString method
        Console.WriteLine(wall.ToString());
        Console.WriteLine(box.ToString());
        Console.WriteLine(key.ToString());

        // Test getters and setters
        Console.WriteLine("\nTesting properties and methods:");
        Console.WriteLine($"Box Name: {box.GetName()}");
        Console.WriteLine($"Is Box Movable? {box.IsMovable()}");

        box.Movable(false);
        Console.WriteLine($"Box Movable after update: {box.IsMovable()}");

        Console.WriteLine($"Is Key Interacting? {key.IsInteracting()}");

        key.Interaction(false);
        Console.WriteLine($"Key Interacting after update: {key.IsInteracting()}");
    }
    static void testGameObjectAndCell(){
        // Create GameObject instances
        GameObject treasure = new GameObject(2, 3, "Treasure", false, false);
        GameObject monster = new GameObject(1, 1, "Monster", true, false);

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
    static void GameDijkstra(MazeGrid maze){           
        // Step 1: Create a PathFinder object
        Graph graph = new Graph(new List<Node>());
        PathFinder pathFinder = new PathFinder(maze,graph);
        pathFinder.LinkNodeRelationships();

        // Step 2: Perform Dijkstra's Algorithm between two nodes
        Cell startCell = maze.GetMazeCell(0,0);
        Cell endCell = maze.GetMazeCell(0,(maze.Height()-1));
        Node startNode = pathFinder.Cell2Node(startCell);  // Top-left corner
        Node endNode = pathFinder.Cell2Node(endCell);    // Bottom-right corner
        var solution = pathFinder.graph.DijkstraAlgorithm(startNode,endNode);

        // Step 3: Output the solution
        List<Node> nodes = solution.Item1;
        int[] distances = solution.Item2;

        int counter = 0;
        foreach (int d in distances){
            if (d!=int.MaxValue){
                counter++;
            }
        }
        Console.WriteLine(pathFinder.PrintMazeSolution(nodes));
        //Console.WriteLine(PrintSolution(nodes,distances,endNode));
        Console.WriteLine($"Distance travelled: {distances[endNode.getNodeID()]}");
        Console.WriteLine($"Total nodes: {graph.GetNodes().Count}");
        Console.WriteLine($"Number of nodes traversed: {counter}");
    }
    static void GameBreadthFirst(MazeGrid maze){           
        // Step 1: Create a PathFinder object
        Graph graph = new Graph(new List<Node>());
        PathFinder pathFinder = new PathFinder(maze,graph);
        pathFinder.LinkNodeRelationships();

        // Step 2: Perform Dijkstra's Algorithm between two nodes
        Cell startCell = maze.GetMazeCell(0,0);
        Cell endCell = maze.GetMazeCell(0,(maze.Height()-1));
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
}
}