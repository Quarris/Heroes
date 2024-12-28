using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Heroes.Client;
using Heroes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MLEM.Extensions;
using MLEM.Pathfinding;
using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace Heroes.World;
public class Map
{
    public Random Random { get; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Dictionary<Point, Ground> GroundTiles { get; set; }
    public Dictionary<Point, ResourceInstance> Resources { get; set; }
    public Player Player { get; }

    public AStar2 Pathfinding;
    public Point? TargetPos { get; set; }
    private Stack<Point> _currentPath = null;

    public Map(int width, int height) : this(width, height, new Random().Next())
    {

    }

    public Map(int width, int height, int seed)
    {
        Random = new Random(seed);
        Width = width;
        Height = height;
        GroundTiles = new Dictionary<Point, Ground>();
        Resources = new Dictionary<Point, ResourceInstance>();
        Player = new Player(Color.Red);
        Pathfinding = new AStar2(TravelCost, true);
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Random.Next(4) == 0)
                {
                    GroundTiles[new Point(i, j)] = ObjectStorage.Stone;
                }
                else
                {
                    GroundTiles[new Point(i, j)] = ObjectStorage.Grass;
                }

            }
        }

        for (int i = 0; i < 5; i++)
        {
            var pos = new Point(Random.Next(Width), Random.Next(Height));
            Resources[pos] = new ResourceInstance(ObjectStorage.Crystals, 1 + Random.Next(5));
        }
    }

    public void Update(ClientState client, GameTime gameTime)
    {
        if (Inputs.Keyboard.WasKeyJustUp(Keys.W)) Player.Position += new Point(0, -1);
        if (Inputs.Keyboard.WasKeyJustUp(Keys.A)) Player.Position += new Point(-1, 0);
        if (Inputs.Keyboard.WasKeyJustUp(Keys.S)) Player.Position += new Point(0, 1);
        if (Inputs.Keyboard.WasKeyJustUp(Keys.D)) Player.Position += new Point(1, 0);

        if (InBounds(client.HoveredPoint))
        {
            Point clickedPos = client.HoveredPoint;
            Ground clickedTile = GroundTiles[clickedPos];
            if (Inputs.Mouse.WasButtonJustDown(MouseButton.Right))
            {
                GroundTiles[clickedPos] = clickedTile == ObjectStorage.Grass ? ObjectStorage.Road : ObjectStorage.Grass;
            }

            if (Inputs.Mouse.WasButtonJustDown(MouseButton.Left))
            {
                Stack<Point> path = Pathfinding.FindPath(Player.Position, clickedPos);
                if (clickedPos != Player.Position && path != null)
                {
                    if (!TargetPos.HasValue || TargetPos != clickedPos)
                    {
                        TargetPos = clickedPos;
                        _currentPath = path;
                    }
                    else
                    {
                        Player.Position = TargetPos.Value;
                        client.Camera.Follow(Player.Position.ToVector2());
                        TargetPos = null;
                        _currentPath = null;
                    }
                }
            }

            if (Resources.ContainsKey(client.HoveredPoint))
            {
                Mouse.SetCursor(MouseCursor.FromTexture2D(ObjectStorage.Cursor, 8, 8));
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }
        }
        else
        {
            Mouse.SetCursor(MouseCursor.No);
        }
    }

    private float TravelCost(Point from, Point to)
    {
        if (!InBounds(to))
        {
            return float.MaxValue;
        }

        Ground targetGround = GroundTiles[to];
        if (!targetGround.Properties.CanPathOnto)
        {
            return float.MaxValue;
        }

        if (from.X != to.X && from.Y != to.Y) // Diagonal
        {
            Point direction = to - from;
            Point horizontalAdjacent = new(from.X + direction.X, from.Y);
            Point verticalAdjacent = new(from.X, from.Y + direction.Y);
            if (!GroundTiles[horizontalAdjacent].Properties.CanPathOnto && !GroundTiles[verticalAdjacent].Properties.CanPathOnto)
            {
                return float.MaxValue;
            }

            return 1.41f * targetGround.Properties.PathingCost;
        }

        return targetGround.Properties.PathingCost;
    }

    public void Draw(ClientState client, GameTime gameTime)
    {
        foreach (var position in GroundTiles.Keys)
        {
            var groundTile = GroundTiles[position];
            groundTile.Draw(client.Sprites, position, gameTime);
            client.Sprites.DrawRectangle(position.ToVector2() * 16, new Size2(16, 16), Color.Black, 0.5f);
        }

        foreach (var position in Resources.Keys)
        {
            var resource = Resources[position];
            resource.ResourceType.Draw(client.Sprites, position, gameTime);
            client.Sprites.DrawRectangle(position.ToVector2() * 16, new Size2(16, 16), Color.Black, 0.5f);
        }

        if (_currentPath != null)
        {
            foreach (var point in _currentPath)
            {
                client.Sprites.DrawPoint(point.X * 16 + 8, point.Y * 16 + 8, Color.GreenYellow, 4);
            }

            client.Sprites.DrawPoint(_currentPath.Last().X * 16 + 8, _currentPath.Last().Y * 16 + 8, Color.GreenYellow, 8);
        }

        Player.Draw(client.Sprites, gameTime);
    }

    public bool InBounds(Point point)
    {
        return point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;
    }
}