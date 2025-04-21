using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
namespace MazeGame { 
class Program { 
    static void Main() {
        string ch = "";
        bool resume = true;
        while(resume!=false){
            Console.WriteLine("1)Custom test in maze generating and path searching \n2)Start Game without robot \n3)Start Game \nQ)Quit");
            Console.Write(">");
            //testScript();
            ch = Console.ReadLine()!.ToUpper();
            if (ch=="1"){
                CustomGame();
            }else if (ch=="2"){
                TestGame();
            }else if (ch=="3"){
                GameDifficulty();
            }else if (ch=="Q"){
                Console.WriteLine("Shutting Game");
                resume = false;
            }
        }
    }
    static void GameDifficulty() {
        bool valid = true;
        do {
            string debugMessage = "conversion";
            int difficulty = 0;
            Console.WriteLine("Which difficulty? \n1) Beginner 2) Easy 3) Medium 4) Hard");
            Console.Write("Difficulty: ");
            string ch = Console.ReadLine()!.ToUpper();
            int.TryParse(ch, out difficulty);
            if (difficulty==0) {
                debugMessage = "level";
            }
            if (difficulty == 1){
                Console.WriteLine("You chose Beginner level");
                SpeedGame(12,12,"P",3,2000,20);
                valid = true;
            }else if (difficulty == 2){
                Console.WriteLine("You chose Easy level");
                SpeedGame(12,12,"D",3,1500,15);
                valid = true;
            }else if (difficulty == 3){
                Console.WriteLine("You chose Medium level");
                SpeedGame(30,30,"P",2,1000,10);
                valid = true;
            }else if (difficulty == 4){
                Console.WriteLine("You chose Hard level");
                SpeedGame(30,30,"D",1,500,5);
                valid = true;
            }else{
                valid = false;
                Console.WriteLine("Invalid "+debugMessage);
            }
        }while(valid==false);
    } 
    static void TestGame(){
        MazeGrid gameboard = new MazeGrid(9,9);
        // Initialising cells in the maze
        int goalx = gameboard.GetEndX();
        int goaly = gameboard.GetEndY();
        gameboard.InitialiseMaze();
        gameboard.CreateDFSMaze();
        int startx = 0;
        int starty = 0;
        // Create player object and navigator object
        Player player = new Player(startx,starty,"Player1"); 
        PathFinder pathFinder = new PathFinder(gameboard,new Graph()); // Assuming PathFinder has a constructor that takes a MazeGrid
        gameboard.AddObject(player);
        // Get startCell and endCell
        Cell startCell = gameboard.GetMazeCell(startx,starty);
        Cell endCell = gameboard.GetMazeCell(goalx,goaly);
        Cell currentCell = startCell;

        // Output the entire maze in game.txt
        OutputTextFile(gameboard.MazePrintWithGmObj(gameboard),"game.txt");
        Console.WriteLine("The goal of this game is to reach G\n");
        Console.WriteLine("Read maze from "+Environment.CurrentDirectory+"/game.txt\n");

        string message = "not moved";
        int radius = 3;
        bool win = false;
        do{
            // Instructions for inputs
            gameboard.RemoveHints();
            Console.WriteLine("Arrow keys to move, E) Check if you are at goal D) To find path to goal , Q)To close Game");
            System.ConsoleKey input = Console.ReadKey().Key;  
            if (input == ConsoleKey.UpArrow&&gameboard.MoveValid(currentCell,0,-1)){
                Moving(0,-1);
            }else if (input == ConsoleKey.DownArrow&&gameboard.MoveValid(currentCell,0,1)){
                Moving(0,1);
            }else if (input == ConsoleKey.LeftArrow&&gameboard.MoveValid(currentCell,-1,0)){
                Moving(-1,0);
            }else if (input == ConsoleKey.RightArrow&&gameboard.MoveValid(currentCell,1,0)){
                Moving(1,0);
            }else if (input == ConsoleKey.Q){
                message = "PAUSE";
            }else if (input == ConsoleKey.E){
                message = GameClear(currentCell);
                if (message=="CLEAR"){
                    win = true;
                }
            }else if (input == ConsoleKey.D){
                    List<object> objs = new List<object>();
                    if (pathFinder.Cell2Node(currentCell)==null){
                        objs = pathFinder.ReturnPathToNearestNode(currentCell);
                    }else{
                        objs = pathFinder.ReturnPath(currentCell,endCell,radius+5,"B",'#');
                    } 
                    message = "Hints given";
                    gameboard.AddObjects(objs);
                
            }else{
                message = "Not moved";
            }
            Console.Clear();
            Console.WriteLine(message);
            if (message!="PAUSE"){
                OutputTextFile(gameboard.PrintCameraAngle(currentCell,radius),"game.txt");
            }
        }while (message!="PAUSE"&&win==false);
        void Moving(int a,int b){
            player.Move(currentCell.X()+a,currentCell.Y()+b);
            currentCell = gameboard.GetMazeCell(currentCell.X()+a,currentCell.Y()+b);
            message = "Moved to "+$"x:{currentCell.X()},y:{currentCell.Y()}";
        }
    }
    static void SpeedGame(int i,int j,string t, int radius, double interval,int hintcount){
        System.Timers.Timer timer= new System.Timers.Timer(interval);
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

        // Starting pos
        int startx = 0;
        int starty = 0;
        
        // Create robot maze
        MazeGrid roboard = new MazeGrid(i,j,gameboard.GetGrid(),goalx,goaly);
        roboard.AddObject(new GameObject(goalx,goaly,"Goal Indicator", 'G'));
        
        // Create player object and navigator object
        Player player = new Player(startx,starty,"Player1"); 

        // Create mob object
        PathFinder pathFinder = new PathFinder(roboard,new Graph()); // Assuming PathFinder has a constructor that takes a MazeGrid
        Mob robot = new Mob(startx,starty, "Robot"); // Mob starts at (0,0)


        // Add objects into both maze
        gameboard.AddObject(player);
        roboard.AddObject(robot);
        // Output the entire maze in speedGame.txt
        OutputTextFile(roboard.MazePrintWithGmObj(roboard),"speedGame.txt");
        // Output the entire maze in game.txt
        OutputTextFile(gameboard.MazePrintWithGmObj(gameboard),"game.txt");

        // Get startCell and endCell
        Cell startCell = gameboard.GetMazeCell(startx,starty);
        Cell endCell = gameboard.GetMazeCell(gameboard.GetEndX(),gameboard.GetEndY());
        Cell currentCell = startCell;

        // Get roboard startCell and endCell
        Cell robotStartCell = roboard.GetMazeCell(startx,starty);
        Cell robotEndCell = roboard.GetMazeCell(goalx,goaly);
        Cell robotCurrentCell = robotStartCell;
        robot.SetObjectToFollow(pathFinder.ReturnPath(robotStartCell,robotEndCell,int.MaxValue,"D",'#'));
        //roboard.AddObjects(robot.GetobjectsToFollow());

        // Starts the user input
        Console.WriteLine("The goal of this game is to reach G\n");
        Console.WriteLine("Read maze from "+Environment.CurrentDirectory+"/game.txt"+"\n");
        Console.WriteLine("Read robot maze from "+Environment.CurrentDirectory+"/speedGame.txt"+"\n");
        Console.WriteLine("Enter anything to start...");
        Console.ReadLine();
        // Set the job for the timer
        timer.Elapsed += (sender, e) => OnTimedEvent(sender!, e, roboard,robot,radius);

        // Enables the timer
        timer.Enabled = true;
        DateTime startTime = DateTime.Now;
        string message = "not moved";
        bool win = false;
        while (message!="PAUSE"&&message!="CLEAR"){
            // Instructions for inputs
            gameboard.RemoveHints();
            Console.WriteLine("Arrow keys to move, E) Check if you are at goal D) To find path to goal Q)Quit");
            System.ConsoleKey input = Console.ReadKey().Key;  
            message = "not moved";
            if (input == ConsoleKey.UpArrow&&gameboard.MoveValid(currentCell,0,-1)){
                Moving(0,-1);
            }else if (input == ConsoleKey.DownArrow&&gameboard.MoveValid(currentCell,0,1)){
                Moving(0,1);
            }else if (input == ConsoleKey.LeftArrow&&gameboard.MoveValid(currentCell,-1,0)){
                Moving(-1,0);
            }else if (input == ConsoleKey.RightArrow&&gameboard.MoveValid(currentCell,1,0)){
                Moving(1,0);
            }else if (input == ConsoleKey.Q){
                message = "PAUSE";
            }else if (input == ConsoleKey.E){
                message = GameClear(currentCell);
                if (message=="CLEAR"){
                    win = true;
                }
            }else if (input == ConsoleKey.D){
                if (hintcount>0){
                    hintcount--;
                    List<object> objs = new List<object>();
                    if (pathFinder.Cell2Node(currentCell)==null){
                        message = $"{hintcount} hints left";
                        objs = pathFinder.ReturnPathToNearestNode(currentCell);
                    }else{
                        message = $"{hintcount} hints left";
                        objs =pathFinder.ReturnPath(currentCell,endCell,radius+5,"B",'#');
                    } 
                    gameboard.AddObjects(objs);
                    
                }else{
                    message = "Hint all used";
                }
            }
            if (robot.X()==endCell.X()&&robot.Y()==endCell.Y()){
                message = "CLEAR";
            }
            Console.Clear();
            OutputTextFile(gameboard.PrintCameraAngle(currentCell,radius),"game.txt");
            Console.WriteLine(message);
        }
        timer.Stop();
        DateTime endTime = DateTime.Now;
        double time = Math.Round(DifferenceBetweenTwoTime(startTime,endTime).TotalSeconds,1);
        Console.WriteLine(time+" seconds");
        if(message=="CLEAR"){
            if (win){
                Console.WriteLine("You Won");
            }else{
                Console.WriteLine("Robot Won");
                Console.WriteLine("Printing your path to goal");
                gameboard.AddObjects(pathFinder.ReturnPathToNearestNode(currentCell));
                gameboard.AddObjects(pathFinder.ReturnPath(currentCell,endCell,int.MaxValue,"B",'#'));
                OutputTextFile(gameboard.MazePrintWithGmObj(gameboard),"game.txt");
                Console.WriteLine("Read maze from "+Environment.CurrentDirectory+"/game.txt"+"\n");
            }
        }else{
            int playerdistance = pathFinder.FindDistanceUntilNode(currentCell);
            int robotdistance = pathFinder.FindDistanceUntilNode(robotCurrentCell);
            Console.WriteLine("You were "+playerdistance+" away from the goal");
            Console.WriteLine("Robot were "+robotdistance+" away from the goal");
            if(playerdistance>robotdistance){
                Console.WriteLine("Robot Won");
                Console.WriteLine("Printing your path to goal");
                gameboard.AddObjects(pathFinder.ReturnPathToNearestNode(currentCell));
                gameboard.AddObjects(pathFinder.ReturnPath(currentCell,endCell,int.MaxValue,"B",'#'));
                OutputTextFile(gameboard.MazePrintWithGmObj(gameboard),"game.txt");
                Console.WriteLine("Read maze from "+Environment.CurrentDirectory+"/game.txt"+"\n");
            }else if (playerdistance<robotdistance){
                Console.WriteLine("You Won");
            }else{
                Console.WriteLine("Draw");
            }
        } 
        void Moving(int a,int b){
            player.Move(currentCell.X()+a,currentCell.Y()+b);
            currentCell = gameboard.GetMazeCell(currentCell.X()+a,currentCell.Y()+b);
            message = "Moved to "+$"x:{currentCell.X()},y:{currentCell.Y()}";
        }
    } 
    static string GameClear(Cell c){
        if (c.IsGoal()){
            return "CLEAR";
        }else{
            return "Not there yet";
        }
    }
    static TimeSpan DifferenceBetweenTwoTime(DateTime s,DateTime e){ //returns the time in seconds
        TimeSpan timeDiff = e.TimeOfDay - s.TimeOfDay;
        return timeDiff;
    }
    static void OnTimedEvent(object sender, ElapsedEventArgs e,MazeGrid roboard, Mob robot, int radius){
        GameObject obj = robot.PopObjToFollow();
        if (obj!=null){
            Cell robotCurrentCell = roboard.GetMazeCell(obj.X(),obj.Y());
            robot.Move(obj.X(),obj.Y());
            OutputTextFile(roboard.PrintCameraAngle(robotCurrentCell,radius),"speedGame.txt");
        }
    }
    static void OutputTextFile(string print,string file){
        string path = Environment.CurrentDirectory+"/"+file;
        //Console.WriteLine(path);
        // Write file using StreamWriter
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(print);
            writer.Close();
        }
    }
    static void CustomGame(){
        bool valid=false;
        string debugMessage = "";
        int[] size = new int[2];
        do {
            Console.WriteLine($"{debugMessage}");
            Console.WriteLine("Input the width and length of the maze from (2-200):");
            Console.Write("X,Y: ");
            string choice = Console.ReadLine()!.ToUpper();
            if (Regex.IsMatch(choice,"[0-9]+,[0-9]+")){
                string[] s = choice.Split(",");
                for(int i = 0; i < s.Length;i++){
                    int.TryParse(s[i], out size[i]);
                }
                valid = true;
            }
            
            for(int i = 0; i < size.Length;i++){
                if (size[i] < 2){
                    debugMessage = "Input less 2";
                    valid = false;
                }else if (size[i] > 200){
                    debugMessage = "Input larger than 200";
                    valid = false;
                }
            }
        }
        while(valid!=true);
        Console.WriteLine("Any) For a depth maze P) For a scattered maze");
        MazeGrid maze;
        string ch = Console.ReadLine()!.ToUpper();
        if (ch=="P"){
            maze = testCreateRandomPrimsMaze(size[0],size[1]);
        }else{
            maze = testCreateRandomDFSMaze(size[0],size[1]);
        }
        Console.WriteLine("Printing maze");
        OutputTextFile(maze.MazeNoNumPrint(),"game.txt");
        Console.WriteLine("Read maze from "+Environment.CurrentDirectory+"/game.txt"+"\n");
        Console.WriteLine("Converting the maze to graph...");
        Graph graph = new Graph(new List<Node>());
        PathFinder pathFinder = new PathFinder(maze,graph);
        Console.WriteLine("Maze converted");
        Console.WriteLine("Enter to continue...");
        Console.ReadLine();

        Cell startCell = maze.GetMazeCell(0,0);
        Cell endCell = maze.GetMazeCell(maze.GetEndX(),maze.GetEndY());
        Node startNode = pathFinder.Cell2Node(startCell);
        Node endNode = pathFinder.Cell2Node(endCell);
        Console.WriteLine($"End X:{maze.GetEndX()}, End Y:{maze.GetEndY()}");
        Console.WriteLine("Total node: "+graph.GetNodes().Count);
        Console.WriteLine("Enter to test Dijkstra's Algorithm...");
        Console.ReadLine();

        Console.WriteLine("\nDijkstra's Algorithm:");
        DateTime startTime = DateTime.Now;
        var solution = graph.DijkstraAlgorithm(startNode,endNode);
        DateTime endTime = DateTime.Now;
        maze.AddObjects(pathFinder.FillGaps(solution.Item1,'#'));
        Console.WriteLine("Travelled "+solution.Item3+" nodes");
        Console.WriteLine("Distance travelled "+solution.Item2);
        Console.WriteLine("Time took Dijkstra's traversal "+DifferenceBetweenTwoTime(startTime,endTime).Milliseconds+" in Milliseconds\n");
        Console.WriteLine("Printing path...");
        OutputTextFile(maze.MazePrintWithGmObj(maze),"dijkstraSolution.txt");
        Console.WriteLine("Printed");
        Console.WriteLine("Read maze from "+Environment.CurrentDirectory+"/dijkstraSolution.txt"+"\n");
        Console.WriteLine("Enter to test Breadth-first search Algorithm...");
        Console.ReadLine();

        maze.RemoveHints();
        Console.WriteLine("Breadth-first search Algorithm:\n");
        startTime = DateTime.Now;
        solution = graph.BreadthFirstTraversal(startNode,endNode);
        endTime = DateTime.Now;
        maze.AddObjects(pathFinder.FillGaps(solution.Item1,'#'));
        Console.WriteLine("Travelled "+solution.Item3+" nodes");
        Console.WriteLine("Distance travelled "+solution.Item2);
        Console.WriteLine("Time took Breadth-first Traversal "+DifferenceBetweenTwoTime(startTime,endTime).Milliseconds+" in Milliseconds\n");
        Console.WriteLine("Printing path...");
        OutputTextFile(maze.MazePrintWithGmObj(maze),"breadthFirstSolution.txt");
        Console.WriteLine("Printed");
        Console.WriteLine("Read maze from "+Environment.CurrentDirectory+"/breadthFirstSolution.txt"+"\n");
    }
    static void testPlayer(){
        MazeGrid m = testCreateRandomPrimsMaze(3,3);
        Player player = new Player(0,0,"Player1"); 
        GameObject g = new GameObject(0,0,"Object",'O');
        m.AddObjects([g]);
        //player.GetInventory
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

        GameObject[] objects = {new GameObject(0,0,"One",'1'),new GameObject(1,0,"Two",'2'),new GameObject(2,0,"Three",'3'),
        new GameObject(0,1,"Four",'4'),new GameObject(1,1,"Five",'5'),new GameObject(2,1,"Six",'6'),
        new GameObject(0,2,"Seven",'7'),new GameObject(1,2,"Eight",'8'),new GameObject(2,2,"Nine",'9')};

        string mazePrint = m.MazeNoNumPrint();
        char[] cells = mazePrint.ToCharArray();
        foreach (GameObject obj in objects){
            if (obj.X()<m.Width()&&obj.X()>=0&&obj.Y()<m.Height()&&obj.Y()>=0){
                int x = Math.Abs(m.GetMazeCell(0,0).X() - obj.X());
                int y = Math.Abs(m.GetMazeCell(0,0).Y() - obj.Y())+1;
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
        MazeGrid mazeBoard = new MazeGrid(width,height);
        mazeBoard.InitialiseMaze();
        mazeBoard.CreateDFSMaze();
        return mazeBoard;
    }
    static MazeGrid testCreateRandomPrimsMaze(int width,int height){
        Console.WriteLine("Class Pathfinder test using random maze and graph structure");
        MazeGrid mazeBoard = new MazeGrid(width,height);
        mazeBoard.InitialiseMaze();
        mazeBoard.CreatePrimsMaze();
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
    static void testGameObjectAndCell(){
        // Create GameObject instances
        GameObject treasure = new GameObject(2, 3, "Treasure",'T');
        GameObject monster = new GameObject(1, 1, "Monster",'M');

        // Create Cell instances
        Cell cell1 = new Cell(0, 0);
        Cell cell2 = new Cell(1, 1, false);
        Cell cell3 = new Cell(2, 3, true);

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

}
}