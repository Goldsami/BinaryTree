using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ClassBinaryTreeTests")]
namespace ClassBinaryTree
{
    public class TreeNode<T>
        where T : IComparable<T>
    {
        private T _value;
        public T Value
        {
            get { return _value; }
            private set
            {
                if (value != null) _value = value;
                else throw new ArgumentNullException(nameof(value), "Value cannot be null.");
            }
        }
        public TreeNode<T> ParentNode { get; internal set; }
        public TreeNode<T> LeftNode { get; internal set; }
        public TreeNode<T> RightNode { get; internal set; }

        /// <summary>
        /// Initialize node.
        /// </summary>
        /// <param name="data">Stored value.</param>
        /// <param name="parent">Link to parent node.</param>
        public TreeNode(T data, TreeNode<T> parent)
        {
            this.Value = data;
            this.ParentNode = parent;
            this.LeftNode = null;
            this.RightNode = null;
        }

        public TreeNode(T data)
        {
            this.Value = data;
            this.ParentNode = null;
            this.LeftNode = null;
            this.RightNode = null;
        }

        /// <summary>
        /// Figures whether this node is left or right for parent node.
        /// </summary>
        /// <returns>Returns -1 if current node is left one for parent, 0 if got no parent, 1 if right one.</returns>
        internal int NodeForParent()
        {
            if (this == null)
            {
                throw new NodeDoesntExistsException("Node doesn't exist");
            }
            if (ParentNode == null) return 0;
            return ParentNode.LeftNode == this ? -1 : 1;
        }

        /// <summary>
        /// Insert value under current node.
        /// </summary>
        /// <param name="data">Stored value.</param>
        internal void InsertUnderNode(T data, IComparer<T> comparer)
        {
            if (BinaryTree<T>.CompareValues(data, Value, comparer) < 0)
            {
                if (LeftNode == null)
                {
                    LeftNode = new TreeNode<T>(data, this);
                    return;
                }
                else LeftNode.InsertUnderNode(data, comparer);
            }
            else
            {
                if (RightNode == null)
                {
                    RightNode = new TreeNode<T>(data, this);
                    return;
                }
                else RightNode.InsertUnderNode(data, comparer);
            }
        }

        internal void DeleteCurrentNode()
        {
            int nodeForParent = this.NodeForParent();

            if (LeftNode == null && RightNode == null)
            {
                switch (nodeForParent)
                {
                    case -1:
                        ParentNode.LeftNode = null;
                        break;
                    case 1:
                        ParentNode.RightNode = null;
                        break;
                }
                return;
            }

            else if (LeftNode == null)
            {
                switch (nodeForParent)
                {
                    case -1:
                        ParentNode.LeftNode = RightNode;
                        break;
                    case 1:
                        ParentNode.RightNode = RightNode;
                        break;
                }
                return;
            }

            else if (RightNode == null)
            {
                switch (nodeForParent)
                {
                    case -1:
                        ParentNode.LeftNode = LeftNode;
                        break;
                    case 1:
                        ParentNode.RightNode = LeftNode;
                        break;
                }
                return;
            }

            else
            {
                switch (nodeForParent)
                {
                    case -1:
                        ParentNode.LeftNode = RightNode;
                        RightNode.GetLowestNode().LeftNode = LeftNode;
                        break;
                    case 1:
                        ParentNode.RightNode = RightNode;
                        RightNode.GetLowestNode().LeftNode = LeftNode;
                        break;
                }
                return;
            }
        }

        /// <summary>
        ///Search for value under current node incusive.
        /// </summary>
        /// <param name="data">Sought value.</param>
        /// <returns>Return list of nodes which satisfy the query</returns>
        internal List<TreeNode<T>> FindUnderNode(T data, IComparer<T> comparer)
        {
            var tempList = new List<TreeNode<T>>();
            if (BinaryTree<T>.CompareValues(data, Value, comparer) == 0)
            {
                tempList.Add(this);
                if (RightNode != null)
                {
                    tempList.AddRange(RightNode.FindUnderNode(data, comparer));
                }
                return tempList;
            }
            else if (BinaryTree<T>.CompareValues(data, Value, comparer) < 0 && LeftNode != null)
            {
                tempList.AddRange(LeftNode.FindUnderNode(data, comparer));
                return tempList;
            }
            else if (BinaryTree<T>.CompareValues(data, Value, comparer) > 0 && RightNode != null)
            {
                tempList.AddRange(RightNode.FindUnderNode(data, comparer));
                return tempList;
            }
            else
            {
                return tempList;
            }
        }

        /// <summary>
        ///Search the node with the smallest value under current.
        /// </summary>        
        /// <returns>Returns the node with the smallest value.</returns>
        internal TreeNode<T> GetLowestNode()
        {
            var tempNode = this;
            while (tempNode.LeftNode != null)
            {
                tempNode = tempNode.LeftNode;
            }
            return tempNode;
        }
    }
}
