using System;
using System.Windows;
using System.Windows.Controls;

namespace Graphs
{
    public struct Coordinate : IEquatable<Coordinate>
    {
        public int Top { get; }
        public int Left { get; }

        public Coordinate(int top, int left)
        {
            Top = top;
            Left = left;
        }

        public bool Equals(Coordinate other)
        {
            return Top == other.Top && Left == other.Left;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Coordinate && Equals((Coordinate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Top * 397) ^ Left;
            }
        }

        public override string ToString()
        {
            return $"({Left},{Top})";
        }

        public void SetToValueCanvas(DependencyObject obj)
        {
            obj.SetValue(Canvas.LeftProperty, Left);
            obj.SetValue(Canvas.TopProperty, Top);
        }
    }
}
