using System;
using System.Collections;
using System.Collections.Generic;

namespace MazeGame{
    class MazeGrid {
        public Cell[,] _mazeGrid;
        public List<object> gameObjects;
        int width, height;
        int endX,endY;
    
    public MazeGrid(int w,int h) {
        width = w;
        height = h;
        _mazeGrid = new Cell[width,height];
        gameObjects = new List<object>();
        endX = PickRandomNum(w/3*2,w-1);
        endY = PickRandomNum(h/3*2,h-1);
    }
    public MazeGrid(int w,int h,Cell[,] cells,int a,int b){
        width = w;
        height = h;
        _mazeGrid = cells;
        endX =a; 
        endY =b;
        gameObjects = new List<object>();
    }
    public MazeGrid(int w,int h,List<object> d){
        width = w;
        height = h;
        _mazeGrid = new Cell[width,height];
        gameObjects = d;
    }
    public int GetEndX(){
        return endX;
    }
    public int GetEndY(){
        return endY;
    }
    public void SetEndX(int a){
        endX = a;
    }
    public void SetEndY(int b){
        endX = b;
    }
    public Cell[] GetMazeRows(int j){
        Cell[] rowOfCells = new Cell[width];
        for (int i = 0;i < width;i++){
            rowOfCells[i] = _mazeGrid[i,j];
        }
        return rowOfCells;
    }
    public void InitialiseMaze(){
        for (int i = 0;i<width;i++){
            for (int j = 0;j<height;j++){
                _mazeGrid[i,j] = new Cell(i,j,false);
            }
        }
        // Add neighbour cells to cell
        for (int i = 0;i<width;i++){
            for (int j = 0;j<height;j++){
                if (i+1<width){
                    _mazeGrid[i,j].neighbourCells.Add(_mazeGrid[i+1,j]);
                }
                if (i-1>=0){
                    _mazeGrid[i,j].neighbourCells.Add(_mazeGrid[i-1,j]);
                }
                if (j+1<height){
                    _mazeGrid[i,j].neighbourCells.Add(_mazeGrid[i,j+1]);
                }
                if (j-1>=0){
                    _mazeGrid[i,j].neighbourCells.Add(_mazeGrid[i,j-1]);
                }
            }
        }
    }
    public int Width(){
        return width;
    }
    public int Height(){
        return height;
    }
    public void SetMazeCell(int i, int j,Cell cell){
        _mazeGrid[i,j] = cell;
    }
    public Cell GetMazeCell(int i, int j){
        return _mazeGrid[i,j];
    }
    public void DFSGenerateMaze(Cell prevCell,Cell currCell) {
        currCell.Visited(true);
        ClearWall(prevCell,currCell);
        Cell nextcell;
        do{
            nextcell = NeighbourCell(currCell.neighbourCells,false);
            if (nextcell!=null){
                DFSGenerateMaze(currCell,nextcell);
            }
        }while (nextcell != null);
    } 
    public void PrimsMaze(Cell startCell){
        List<Cell> frontierCells = new List<Cell>();
        Cell prevCell;
        frontierCells.Add(startCell);
        while (frontierCells.Count>0){
            //Pick random Cell in frontiers
            Cell currentCell = PickAndRemoveCellinList(frontierCells);

            //Mark cell as Visited
            currentCell.Visited(true);

            //Update previous Cell
            if (NeighbourCell(currentCell.neighbourCells,true)==null){
                prevCell = currentCell;
            }else{
                prevCell = NeighbourCell(currentCell.neighbourCells,true);
            }
            if (prevCell!=currentCell){
                ClearWall(prevCell,currentCell);
            }

            //Add new frontiers cell that are not visited yet
            foreach (Cell c in RetrieveAdjacentCells(currentCell.neighbourCells,false)){
                if (!frontierCells.Contains(c)){
                    frontierCells.Add(c);
                }
            }
        }
    }
    public Cell PickAndRemoveCellinList(List<Cell> cells){
        int val = PickRandomNum(cells.Count);
        Cell c = cells[val];
        cells.RemoveAt(val);        // Remove the item  
        return c;
    }
    public int PickRandomNum(int size){
        Random random = new Random();
        return random.Next(size);       // Pick a random index
    }
    public int PickRandomNum(int upperBound,int lowerBound){
        Random random = new Random();
        return random.Next(upperBound,lowerBound);
    }
    public void SetAllCellNotVisited(){
        foreach (Cell cell in _mazeGrid){
            cell.Visited(false);
        }
    }
    public void ClearWall(Cell prevCell,Cell currCell) {
        if (prevCell!=null){
            if (prevCell.X() > currCell.X()){
                prevCell.LeftWall = false;
                currCell.RightWall = false;
            }else if (prevCell.X() < currCell.X()){
                prevCell.RightWall = false;
                currCell.LeftWall = false;
            }else if (prevCell.Y() > currCell.Y()){
                prevCell.FrontWall = false;
                currCell.BackWall = false;
            }else if (prevCell.Y() < currCell.Y()){
                prevCell.BackWall = false;
                currCell.FrontWall = false;
            }
        }
    }
    public List<Cell> RetrieveAdjacentCells(List<Cell> cells,bool visited){
        List<Cell> adjcells = new List<Cell>();
        foreach (Cell cell in cells){
            if (cell.IsVisited() == visited){
                adjcells.Add(cell);
            }
        }
        return adjcells; 
    }
    public Cell NeighbourCell(List<Cell> cells,bool visited){
        List<Cell> adjCell = RetrieveAdjacentCells(cells,visited);
        // Pick random cell from adjCell
        if (adjCell.Count==0){
            return null!;
        }
        Cell c = PickAndRemoveCellinList(adjCell);
        return c;
    }
    public string PrintConnectedNeighbours(){
        string message = "";
        foreach (Cell cell in _mazeGrid){
            message+=$"{cell.X()},{cell.Y()}:";
            for (int i = 0;i<cell.connectedCells.Count;i++){
                message+=$"({cell.connectedCells[i].X()},{cell.connectedCells[i].Y()}) | ";
            }
            message+=$"\n";
        }
        return message;
    }
    public void SetConnectedCells(){
        foreach (Cell currcell in _mazeGrid){
            if (!currcell.FrontWall && currcell.Y()-1 >= 0){
                currcell.AddConnectedCell(GetMazeCell(currcell.X(), currcell.Y()-1));
            }
            if (!currcell.BackWall && currcell.Y()+1 < height){
                currcell.AddConnectedCell(GetMazeCell(currcell.X(), currcell.Y()+1));
            }
            if (!currcell.LeftWall && currcell.X()-1 >= 0){
                currcell.AddConnectedCell(GetMazeCell(currcell.X()-1, currcell.Y()));
            }
            if (!currcell.RightWall && currcell.X()+1 < width){
                currcell.AddConnectedCell(GetMazeCell(currcell.X()+1, currcell.Y()));
            }
        }
    }
    public bool MoveValid(Cell c,int a,int b){
        int x = c.X()+a;
        int y = c.Y()+b;
        if (x>=0&&x<width&&y>=0&&y<height){
            if (a<0&&!c.LeftWall){
                return true;
            }else if (a>0&&!c.RightWall){
                return true;
            }else if (b<0&&!c.FrontWall){
                return true;
            }else if (b>0&&!c.BackWall){
                return true;
            }
        }
        return false;
    }
    public void CreateDFSMaze(){
        GetMazeCell(endX,endY).Goal(true);
        AddObject(new GameObject(endX,endY,"Goal Indicator", 'G',false));
        DFSGenerateMaze(null!,GetMazeCell(PickRandomNum(width-1),PickRandomNum(height-1)));
        SetAllCellNotVisited();
        SetConnectedCells();
    }
    public void CreatePrimsMaze(){
        GetMazeCell(endX,endY).Goal(true);
        AddObject(new GameObject(endX,endY,"Goal Indicator", 'G',false));
        PrimsMaze(GetMazeCell(PickRandomNum(width-1)/2,PickRandomNum(height-1)));
        SetAllCellNotVisited();
        SetConnectedCells();
    }
    public void RemoveHints(){
        for (int i = gameObjects.Count-1;i>1;i--){
            gameObjects.RemoveAt(i);
        }
    }
    public string MazePrint() {
        string mazeprintmessage ="   ";
        for (int i = 0;i<Width();i++){
            mazeprintmessage += $"{Convert.ToString(i),3}  ";
        }
        mazeprintmessage += "\n".PadRight(4);
        mazeprintmessage += PrintCellFrontWall(GetMazeRows(0));
        for (int j = 0;j<Height();j++){
            Cell[] rowOfCells = GetMazeRows(j);
            mazeprintmessage += $"{j,3}";
            mazeprintmessage += PrintCellLeftRightWall(rowOfCells);
            mazeprintmessage += $"".PadLeft(3);
            mazeprintmessage += PrintCellBackWall(rowOfCells);
        }
        return mazeprintmessage;
    }
    public string MazeNoNumPrint() {
        string mazeprintmessage ="";
        mazeprintmessage += PrintCellFrontWall(GetMazeRows(0));
        for (int j = 0;j<Height();j++){
            Cell[] rowOfCells = GetMazeRows(j);
            mazeprintmessage += PrintCellLeftRightWall(rowOfCells);
            mazeprintmessage += PrintCellBackWall(rowOfCells);
        }
        return mazeprintmessage;
    }
    public string MazePrintWithGmObj(MazeGrid maze){
        string mazeprintmessage ="";
        mazeprintmessage += maze.PrintCellFrontWall(maze.GetMazeRows(0));
        for (int j = 0;j<maze.Height();j++){
            Cell[] rowOfCells = maze.GetMazeRows(j);
            mazeprintmessage += maze.PrintCellLeftRightWall(rowOfCells);
            mazeprintmessage += maze.PrintCellBackWall(rowOfCells);
        }
        
        return PrintAddObjects(maze,mazeprintmessage);
    }
    public string PrintCellLeftRightGmObjWall(Cell[] cells){
        string message = "";
        foreach (Cell cell in cells){
            if (cell.LeftWall){
                message += "| ";
            }else{
                message += "  ";
            }
            if (cell.RightWall){
                message += " |";
            }else{
                message += "  ";
            }
        }
        message+="\n";
        return message;
    }
    public string PrintCellFrontWall(Cell[] cells){
        string message = "";
        foreach (Cell cell in cells){
            if (cell.FrontWall){
            message += "+---+";
            }else{
            message += "+   +";
            }
        }
        message +="\n";
        return message;
    }
    public string PrintCellLeftRightWall(Cell[] cells){ 
        string message = "";
        foreach (Cell cell in cells){
            if (cell.LeftWall){
                message += "| ";
            }else{
                message += "  ";
            }
            message += " ";
            if (cell.RightWall){
                message += " |";
            }else{
                message += "  ";
            }
        }
        message+="\n";
        return message;
    }
    public string PrintCellBackWall(Cell[] cells){
        string message = "";
        foreach (Cell cell in cells){
            if (cell.BackWall){
                message += "+---+";
            }else{
                message += "+   +";
            }
        }
        message+="\n";
        return message;       
    }
    public int FindLimit(int a,bool sign,int radius,bool direction){
        int limit;
        int count = 0;
        if (direction == true){
            // x
            limit = width-1;
        }else{
            // y
            limit = height-1;
        }
        // Determine adding or subtracting
        if (sign==true){
            // adding
            while(a<limit&&count<radius){
                a++;
                count++;
            }
        }else{
            // subtracting
            while(a>0&&count<radius){
                a--;
                count++;
            }
        }
        return a;
    }
    public string PrintCameraAngle(Cell currentCell,int radius){
        // Find boundaries
        int x = currentCell.X();
        int y= currentCell.Y();
        int xRight = FindLimit(x,true,radius,true);
        int yUpper = FindLimit(y,false,radius,false);
        int xLeft = FindLimit(x,false,radius,true);
        int yLower = FindLimit(y,true,radius,false);

        int xSize = Math.Abs(xRight-xLeft)+1;
        int ySize = Math.Abs(yLower-yUpper)+1;

        MazeGrid cameraAngle = new MazeGrid(xSize,ySize,gameObjects);
        int a = 0;
        int b = 0;

        // Put original maze into cameraAngle maze
        for (int i = xLeft; i <= xRight; i++){
            for (int j = yUpper; j <= yLower;j++){
                cameraAngle._mazeGrid[a,b] = _mazeGrid[i,j];
                b++;
            }
            a++;
            b=0;
        }
        return MazePrintWithGmObj(cameraAngle);

    }
    public string PrintAddObjects(MazeGrid maze, string mazePrint){
        char[] cells = mazePrint.ToCharArray();
        if (gameObjects!=null) {
            for (int i = gameObjects.Count-1; i >= 0;i--){
                object obj = gameObjects[i];
                int x = 0;
                int y = 0;
                if (obj is Navigator){
                    Navigator gameObj = (Navigator)obj;
                    x = gameObj.x-maze.GetMazeCell(0,0).X();
                    y = gameObj.y-maze.GetMazeCell(0,0).Y();
                    if (x<maze.Width()&&x>=0&&y<maze.Height()&&y>=0){
                        int row = (maze.Width()*5+1)*(1+2*y)+2+x*5;
                        cells[row] = gameObj.GetLabel();
                    }
                }else if (obj is Player){
                    Player gameObj = (Player)obj;
                    x = gameObj.x-maze.GetMazeCell(0,0).X();
                    y = gameObj.y-maze.GetMazeCell(0,0).Y();
                    if (x<maze.Width()&&x>=0&&y<maze.Height()&&y>=0){
                        int row = (maze.Width()*5+1)*(1+2*y)+2+x*5;
                        cells[row] = gameObj.GetLabel();
                    }
                }else if (obj is Mob){
                    Mob mob = (Mob)obj;
                    x = mob.x-maze.GetMazeCell(0,0).X();
                    y = mob.y-maze.GetMazeCell(0,0).Y();
                    if (x<maze.Width()&&x>=0&&y<maze.Height()&&y>=0){
                        int row = (maze.Width()*5+1)*(1+2*y)+2+x*5;
                        cells[row] = mob.GetLabel();
                    }
                }else if (obj is GameObject){
                    GameObject gameObj = (GameObject)obj;
                    x = gameObj.x-maze.GetMazeCell(0,0).X();
                    y = gameObj.y-maze.GetMazeCell(0,0).Y();
                    if (x<maze.Width()&&x>=0&&y<maze.Height()&&y>=0){
                        int row = (maze.Width()*5+1)*(1+2*y)+2+x*5;
                        cells[row] = gameObj.GetLabel();
                    }
                }
            }
        }
        string newPrint = "";
        foreach (char c in cells){
            newPrint+=c;
        }
        return newPrint;
    } 
    public void AddObject(object obj){
        gameObjects.Add(obj);
    }
    public void AddObjects(List<object> list){
        foreach (object obj in list){
            AddObject(obj);
        }
    }
    public int CheckNumOfNodes(){
        int size = 0;
        foreach(Cell cell in _mazeGrid){
            if (cell != null){
                //check if its a junction or a turning point or a dead end or a starting point or a end point
                if (cell.IsNode()) {
                    size++;
                }
            }
        }
        return size;
    }
}
}