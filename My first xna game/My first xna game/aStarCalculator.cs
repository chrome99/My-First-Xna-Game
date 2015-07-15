using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class aStarCalculator
    {
        private Map map;
        private List<aStarNode> nodesList;
        private List<aStarNode> openList;
        private List<aStarNode> closedList;
        private List<Text> heuristicTextList = new List<Text>();
        private List<Text> movementCostTextList = new List<Text>();
        private List<Picture> parentImageList = new List<Picture>();
        private bool debugHeuristic;
        private bool debugParent;
        private bool debugMovmentCost;

        public aStarCalculator(Map map, bool debugHeuristic = false, bool debugMovmentCost = false, bool debugParent = false)
        {
            this.map = map;
            this.debugHeuristic = debugHeuristic;
            this.debugParent = debugParent;
            this.debugMovmentCost = debugMovmentCost;

            nodesList = new List<aStarNode>();
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    aStarNode node = new aStarNode();
                    node.position.X = x;
                    node.position.Y = y;
                    node.passable = map.CheckTilePassability(x, y);
                    nodesList.Add(node);
                }
            }
            openList = new List<aStarNode>();
            closedList = new List<aStarNode>();
        }

        public List<Vector2> FindWayTo(Vector2 startingPosition, Vector2 destination, bool diagonal)
        {
            startingPosition = new Vector2((int)(startingPosition / Tile.size).X, (int)(startingPosition / Tile.size).Y);
            destination = new Vector2((int)(destination / Tile.size).X, (int)(destination / Tile.size).Y);

            if (startingPosition == destination) { return null; }

            if (debugHeuristic)
            {
                heuristicTextList.Clear();
            }
            if (debugMovmentCost)
            {
                movementCostTextList.Clear();
            }
            if (debugParent)
            {
                parentImageList.Clear();
            }

            for (int i = 0; i < nodesList.Count; i++)
            {
                nodesList[i].position.X = i % map.width;
                nodesList[i].position.Y = i / map.width;
                nodesList[i].passable = map.CheckTilePassability(i % map.width, i / map.width);
                nodesList[i].heuristic = FindHeuristicValue(nodesList[i], destination);
                nodesList[i].movementCost = 0;
                nodesList[i].parent = null;
                if (debugHeuristic)
                {
                    Text text = new Text(Game.content.Load<SpriteFont>("Fonts\\Debug1 small"), nodesList[i].position * Tile.size + new Vector2(20, 20), Color.White, nodesList[i].heuristic.ToString());
                    heuristicTextList.Add(text);
                }
            }

            aStarNode startingNode = nodesList[(int)(startingPosition.Y * map.width + startingPosition.X)];
            startingNode.parent = null;
            closedList.Add(startingNode);

            foreach (aStarNode node in GetStarNodes(startingNode, diagonal))
            {
                openList.Add(node);
            }

            aStarNode result = null;

            for (int i = 0; i < openList.Count; )
            {
                openList.Sort(CompareAStarNode);
                aStarNode node = openList[i];
                if (node.heuristic == 10)
                {
                    result = node;
                    break;
                }
                closedList.Add(node);
                openList.Remove(node);
                foreach (aStarNode node2 in GetStarNodes(node, diagonal))
                {
                    if (!closedList.Contains(node2) && !openList.Contains(node2))
                    {
                        openList.Add(node2);
                    }
                }
            }
            if (debugMovmentCost || debugParent)
            {
                foreach (aStarNode node in nodesList)
                {
                    if (node.movementCost != 0 && debugMovmentCost)
                    {
                        Text text = new Text(Game.content.Load<SpriteFont>("Fonts\\Debug1 small"), node.position * Tile.size, Color.Gold, node.movementCost.ToString());
                        movementCostTextList.Add(text);
                    }
                    if (node.parent != null && debugParent)
                    {
                        Picture pic = new Picture(Game.content.Load<Texture2D>("Textures\\Spritesheets\\system arrow"), node.position * Tile.size, null);
                        int setDrawingRectY = 0;
                        int setDrawingRectX = 0;
                        if (node.parent.position.X < node.position.X)
                        {
                            setDrawingRectY = 1;
                            setDrawingRectX = 1;
                        }
                        if (node.parent.position.X > node.position.X)
                        {
                            setDrawingRectY = 2;
                            setDrawingRectX = 2;
                        }
                        if (node.parent.position.Y > node.position.Y)
                        {
                            setDrawingRectY = 0;
                        }
                        if (node.parent.position.Y < node.position.Y)
                        {
                            setDrawingRectY = 3;
                        }
                        pic.drawingRect = new Rectangle(Tile.size * setDrawingRectX, Tile.size * setDrawingRectY, Tile.size, Tile.size);
                        parentImageList.Add(pic);
                    }
                }
            }

            List<Vector2> path = new List<Vector2>();
            path.Add(destination * Tile.size);
            TranslateNodeToVector2(result, path);

            openList.Clear();
            closedList.Clear();

            path.Reverse();

            return path;
        }

        private int CompareAStarNode(aStarNode x, aStarNode y)
        {
            if (x.nodeValue == y.nodeValue) return 0;
            else if (x.nodeValue < y.nodeValue) return -1;
            else return 1;
        }

        private void TranslateNodeToVector2(aStarNode node, List<Vector2> path)
        {
            if (node.parent != null)
            {
                path.Add(node.position * Tile.size);
                TranslateNodeToVector2(node.parent, path);
            }
        }

        private int FindHeuristicValue(aStarNode node, Vector2 destination)
        {
            int result = 0;
            if (node.position.X < destination.X)
            {
                result += (int)(destination.X - node.position.X);
            }
            else if (node.position.X > destination.X)
            {
                result += (int)(node.position.X - destination.X);
            }

            if (node.position.Y < destination.Y)
            {
                result += (int)(destination.Y - node.position.Y);
            }
            else if (node.position.Y > destination.Y)
            {
                result += (int)(node.position.Y - destination.Y);
            }

            if (result == 0)
            {
                return -1;
            }
            else
            {
                return result * 10;
            }
        }

        private List<aStarNode> GetStarNodes(aStarNode starCenter, bool diagonal)
        {
            List<aStarNode> result = new List<aStarNode>();

            int starCenterIndex = nodesList.IndexOf(starCenter);
            int nodesListCount = nodesList.Count;

            aStarNode asd = nodesList.Find(x => !x.passable);

            int index = starCenterIndex - 1;
            aStarNode leftNode = null;
            if (index > -1 && index < nodesListCount)
            {
                leftNode = nodesList[index];
                if (leftNode.passable)
                {
                    if (!openList.Contains(leftNode) && !closedList.Contains(leftNode) && leftNode.parent == null)
                    {
                        leftNode.parent = starCenter;
                        leftNode.movementCost = 10 + leftNode.parent.movementCost;
                    }
                    result.Add(leftNode);
                }
            }

            index = starCenterIndex + 1;
            aStarNode rightNode = null;
            if (index > -1 && index < nodesListCount)
            {
                rightNode = nodesList[index];
                if (rightNode.passable)
                {
                    if (!openList.Contains(rightNode) && !closedList.Contains(rightNode) && rightNode.parent == null)
                    {
                        rightNode.parent = starCenter;
                        rightNode.movementCost = 10 + rightNode.parent.movementCost;
                    }
                    result.Add(rightNode);
                }
            }

            index = starCenterIndex - map.width;
            aStarNode upNode = null;
            if (index > -1 && index < nodesListCount)
            {
                upNode = nodesList[index];
                if (upNode.passable)
                {
                    if (!openList.Contains(upNode) && !closedList.Contains(upNode) && upNode.parent == null)
                    {
                        upNode.parent = starCenter;
                        upNode.movementCost = 10 + upNode.parent.movementCost;
                    }
                    result.Add(upNode);
                }
            }

            index = starCenterIndex + map.width;
            aStarNode downNode = null;
            if (index > -1 && index < nodesListCount)
            {
                downNode = nodesList[index];
                if (downNode.passable)
                {
                    if (!openList.Contains(downNode) && !closedList.Contains(downNode) && downNode.parent == null)
                    {
                        downNode.parent = starCenter;
                        downNode.movementCost = 10 + downNode.parent.movementCost;
                    }
                    result.Add(downNode);
                }
            }

            if (!diagonal)
            {
                return result;
            }

            index = starCenterIndex - 1 - map.width;
            aStarNode leftUpNode;
            if (index > -1 && index < nodesListCount)
            {
                leftUpNode = nodesList[index];
                if (leftUpNode.passable && checkDiagonals(leftNode, upNode))
                {
                    if (!openList.Contains(leftUpNode) && !closedList.Contains(leftUpNode) && leftUpNode.parent == null)
                    {
                        leftUpNode.parent = starCenter;
                        leftUpNode.movementCost = 14 + leftUpNode.parent.movementCost;
                    }
                    result.Add(leftUpNode);
                }
            }

            index = starCenterIndex + 1 - map.width;
            aStarNode rightUpNode;
            if (index > -1 && index < nodesListCount)
            {
                rightUpNode = nodesList[index];
                if (rightUpNode.passable && checkDiagonals(rightNode, upNode))
                {
                    if (!openList.Contains(rightUpNode) && !closedList.Contains(rightUpNode) && rightUpNode.parent == null)
                    {
                        rightUpNode.parent = starCenter;
                        rightUpNode.movementCost = 14 + rightUpNode.parent.movementCost;
                    }
                    result.Add(rightUpNode);
                }
            }

            index = starCenterIndex - 1 + map.width;
            aStarNode leftDownNode;
            if (index > -1 && index < nodesListCount)
            {
                leftDownNode = nodesList[index];
                if (leftDownNode.passable && checkDiagonals(leftNode, downNode))
                {
                    if (!openList.Contains(leftDownNode) && !closedList.Contains(leftDownNode) && leftDownNode.parent == null)
                    {
                        leftDownNode.parent = starCenter;
                        leftDownNode.movementCost = 14 + leftDownNode.parent.movementCost;
                    }
                    result.Add(leftDownNode);
                }
            }

            index = starCenterIndex + 1 + map.width;
            aStarNode rightDownNode;
            if (index > -1 && index < nodesListCount)
            {
                rightDownNode = nodesList[index];
                if (rightDownNode.passable && checkDiagonals(rightNode, downNode))
                {
                    if (!openList.Contains(rightDownNode) && !closedList.Contains(rightDownNode) && rightDownNode.parent == null)
                    {
                        rightDownNode.parent = starCenter;
                        rightDownNode.movementCost = 14 + rightDownNode.parent.movementCost;
                    }
                    result.Add(rightDownNode);
                }
            }

            return result;
        }

        private bool checkDiagonals(aStarNode diagonal1, aStarNode diagonal2)
        {
            if (diagonal1 == null)
            {
                if (diagonal2 == null)
                {
                    return true;
                }
                else
                {
                    if (diagonal2.passable)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (diagonal1.passable)
                {
                    if (diagonal2 == null)
                    {
                        return true;
                    }
                    else
                    {
                        if (diagonal2.passable)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (debugHeuristic)
            {
                foreach (Text text in heuristicTextList)
                {
                    text.DrawWithoutSource(spriteBatch, offsetRect);
                }
            }

            if (debugMovmentCost)
            {
                foreach (Text text in movementCostTextList)
                {
                    text.DrawWithoutSource(spriteBatch, offsetRect);
                }
            }

            if (debugParent)
            {
                foreach (Picture picture in parentImageList)
                {
                    picture.DrawWithoutSource(spriteBatch, offsetRect);
                }
            }
        }
    }

    class aStarNode
    {
        public Vector2 position;
        public bool passable = false;

        public int heuristic; //h value
        public int movementCost; //g value
        public int nodeValue
        {
            get { return heuristic + movementCost; }
        }
        public aStarNode parent; //parent
    }
}
