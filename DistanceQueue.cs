using System;
using System.Collections.Generic;

namespace MazeGame{
    class DistanceQueue{
        public List<(Node Item, int Priority)> queue;

        public DistanceQueue()
        {
            queue = new List<(Node Item, int Priority)>();
        }

        public void Enqueue(Node item,int priority)
        {
            queue.Add((item, priority));
            queue.Sort((x, y) => x.Priority.CompareTo(y.Priority)); // Sort by priority (ascending)
        }

        public Node Dequeue()
        {
            if(IsEmpty())
                throw new InvalidOperationException("The priority queue is empty.");

            var highestPriorityItem = queue[0];
            queue.RemoveAt(0);
            return highestPriorityItem.Item;
        }

        public Node Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("The priority queue is empty.");

            return queue[0].Item;
        }

        public bool IsEmpty()
        {
            return queue.Count == 0;
        }
        public void UpdateQueue(Node n,int dist){
            for (int i = 0;i < queue.Count;i++){
                if (queue[i].Item == n){
                    (Node node,int Priority) newDist = queue[i];
                    newDist.Priority = dist;
                    queue[i] = newDist;
                }
            }
            queue.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }
        public bool CheckNodeInQueue(Node n){
            for (int i = 0;i < queue.Count;i++){
                if (queue[i].Item==n){
                    return true;
                }
            }
            return false;
        }
}
}
