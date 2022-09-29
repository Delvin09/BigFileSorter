using Sorter.Chunks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorter.Misc
{
    internal class ChunksQueue
    {
        class Node : IComparable<Node>
        {
            public Node(ChunkFile chunk)
            {
                this.Chunk = chunk;
            }

            public ChunkFile Chunk;

            public Node Left;

            public Node Right;

            public int Height = 1;

            public int CompareTo(Node other)
            {
                return Chunk.Current.CompareTo(other.Chunk.Current);
            }
        }

        private Node _current;
        private Node _root;

        public int Count { get; private set; } = 0;

        private int Height(Node node)
        {
            return node != null ? node.Height : 0;
        }

        private int BalanceFactor(Node node)
        {
            return Height(node.Right) - Height(node.Left);
        }

        private void FixHeight(Node node)
        {
            var hl = Height(node.Left);
            var hr = Height(node.Right);
            node.Height = (hl > hr ? hl : hr) + 1;
        }

        private Node RotateRight(Node node)
        {
            var q = node.Left;
            node.Left = q.Right;
            q.Right = node;
            if (node == _root) _root = q;
            FixHeight(node);
            FixHeight(q);
            return q;
        }

        private Node RotateLeft(Node node)
        {
            var p = node.Right;
            node.Right = p.Left;
            p.Left = node;
            if (node == _root)
                _root = p;
            FixHeight(node);
            FixHeight(p);
            return p;
        }

        private Node Balance(Node node)
        {
            FixHeight(node);
            if (BalanceFactor(node) == 2)
            {
                if (BalanceFactor(node.Right) < 0)
                    node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }
            if (BalanceFactor(node) == -2)
            {
                if (BalanceFactor(node.Left) > 0)
                    node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }
            return node;
        }

        private Node Insert(Node currentNode, Node newNode)
        {
            if (currentNode == null)
                return newNode;

            if (newNode.CompareTo(currentNode) <= 0)
            {
                currentNode.Left = Insert(currentNode.Left, newNode);
            }
            else
                currentNode.Right = Insert(currentNode.Right, newNode);

            return Balance(currentNode);
        }

        private Node FindMin(Node node)
        {
            return node.Left != null ? FindMin(node.Left) : node;
        }

        private Node RemoveMin(Node node)
        {
            if (node.Left == null)
                return node.Right;
            node.Left = RemoveMin(node.Left);
            return Balance(node);
        }

        private Node Remove(Node node, Node value)
        {
            if (node == null) return null;
            var comp = value.CompareTo(node);
            if (comp < 0 || value != node)
                node.Left = Remove(node.Left, value);
            else if (comp > 0 )
                node.Right = Remove(node.Right, value);
            else
            {
                Node q = node.Left;
                Node r = node.Right;
                if (r == null) return q;
                Node min = FindMin(r);
                min.Right = RemoveMin(r);
                min.Left = q;
                return Balance(min);
            }

            return Balance(node);
        }


        public ChunksQueue(IEnumerable<ChunkFile> chunks)
        {
            foreach (var chunk in chunks)
                Enqueue(chunk);

            FindCurrent();
        }

        private void FindCurrent()
        {
            if (_root == null)
            {
                _current = null;
                return;
            }

            Node current = _root;
            while (current.Left != null)
            {
                current = current.Left;
            }
            _current = current;
        }

        private void Enqueue(ChunkFile chunk)
        {
            Enqueue(new Node(chunk));
        }

        private void Enqueue(Node node)
        {
            if (_root == null)
                _root = node;
            else
                Insert(_root, node);
            Count++;
        }

        public ChunkFile Peek()
        {
            return _current.Chunk;
        }

        internal bool MoveNext()
        {
            _root = Remove(_root, _current);
            var result = _current.Chunk.MoveNext();
            if (result)
            {
                _current.Height = 1;
                _current.Left = _current.Right = null;
                Insert(_root, _current);
            }
            else
                Count--;

            FindCurrent();
            return result;
        }
    }
}
