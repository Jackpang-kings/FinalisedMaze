using System;
using System.Collections.Generic;

namespace MazeGame{
    class MazeGrid {
        public Cell[,] _mazeGrid;
        int width, height;
    
    public MazeGrid(int w,int h) {
        width = w;
        height = h;
        _mazeGrid = new Cell[width,height];
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
            nextcell = NeighbourCell(currCell,false);
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
            if (NeighbourCell(currentCell,true)==null){
                prevCell = currentCell;
            }else{
                prevCell = NeighbourCell(currentCell,true);
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
    public Cell NeighbourCell(Cell currCell,bool visited){
        List<Cell> adjCell = RetrieveAdjacentCells(currCell.neighbourCells,visited);
        // Pick random cell from adjCell
        if (adjCell.Count==0){
            return null;
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
    public bool MoveValid(Cell c,string direction){
        int x = c.X();
        int y = c.Y();
        bool valid = false;
        if (c.GetGameObject().IsMovable()){
            if (direction=="UP"){
                if (!c.FrontWall&&y-1>=0){
                    valid = true;
                }
            }else if (direction=="DOWN"){
                if (y+1<height&&!c.BackWall){
                    valid = true;
                }
            }else if (direction=="LEFT"){
                if (!c.LeftWall&&x-1>=0){
                    valid = true;
                }
            }else if (direction=="RIGHT"){
                if (!c.RightWall&&x+1<width){
                    valid = true;
                }
            }
        }
	    
        return valid;
    }
    public Cell Move(Cell currentCell,string direction){
        int x = currentCell.X();
        int y = currentCell.Y();
        GameObject object2Move = currentCell.GetGameObject();
        Cell nextCell;
        if (direction=="UP"){
            nextCell = _mazeGrid[x,y-1];
            object2Move.y = y-1;
            Swap();
        }else if (direction=="DOWN"){
            nextCell = _mazeGrid[x,y+1];
            object2Move.y = y+1;
            Swap();
        }else if (direction=="LEFT"){
            nextCell = _mazeGrid[x-1,y];
            object2Move.x = x-1;
            Swap();
        }else if (direction=="RIGHT"){
            nextCell = _mazeGrid[x+1,y];
            object2Move.x = x+1;
            Swap();
        }else{
            return currentCell;
        }
        void Swap(){
            GameObject holder = nextCell.GetGameObject();
            nextCell.SetGameObject(ref object2Move);
            currentCell.SetGameObject(ref holder);
        }
        return nextCell;
    }
    public void CreateDFSMaze(){
        GetMazeCell(0,0).FrontWall = false;
        GetMazeCell(Width()-1,Height()-1).BackWall = false;
        DFSGenerateMaze(null,GetMazeCell(PickRandomNum(width)/2,PickRandomNum(height)/2));
        SetAllCellNotVisited();
        SetConnectedCells();
    }
    public void CreatePrimsMaze(){
        GetMazeCell(0,0).FrontWall = false;
        GetMazeCell(Width()-1,Height()-1).BackWall = false;
        PrimsMaze(GetMazeCell(PickRandomNum(width)/2,PickRandomNum(height)/2));
        SetAllCellNotVisited();
        SetConnectedCells();
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
    public string MazePrintWithGmObj(){
        string mazeprintmessage ="";
        mazeprintmessage += PrintCellFrontWall(GetMazeRows(0));
        for (int j = 0;j<Height();j++){
            Cell[] rowOfCells = GetMazeRows(j);
            mazeprintmessage += PrintCellLeftRightGmObjWall(rowOfCells);
            mazeprintmessage += PrintCellBackWall(rowOfCells);
        }
        return mazeprintmessage;
    }
    public string PrintCellLeftRightGmObjWall(Cell[] cells){
        string message = "";
        foreach (Cell cell in cells){
            if (cell.LeftWall){
                message += "| ";
            }else{
                message += "  ";
            }
            if (cell.gameObject!=null){
                message += $"{cell.gameObject.GetLabel()}";
            }else{
                message += $" ";
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
    public int CheckNumOfNodes(){
        int size = 0;
        int w = Width();
        int h = Height();
        foreach(Cell cell in _mazeGrid){
            if (cell != null){
                //check if its a junction or a turning point or a dead end or a starting point or a end point
                if (cell.IsNode(w,h)) {
                    size++;
                }
            }
        }
        return size;
    }
}
}