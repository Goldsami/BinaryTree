using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ClassBinaryTreeTests")]
namespace ClassBinaryTree
{
    public class TreeEnumerator<T> : IEnumerator
        where T : IComparable<T>
    {
        private BinaryTree<T> CurrentTree { get; set; }
        internal TreeNode<T> CurrentNode { get; set; }

        public TreeEnumerator(BinaryTree<T> tree)
        {
            if (tree == null) throw new ArgumentNullException("Tree is empty");
            CurrentTree = tree;
            CurrentNode = null;
        }

        public object Current
        {
            get
            {
                if (CurrentNode == null)
                {
                    throw new NodeDoesntExistsException("Node is empty");
                }
                else
                {
                    return CurrentNode.Value;
                }
            }
        }

        public bool MoveNext()
        {
            if (CurrentNode == null)
            {
                CurrentNode = CurrentTree.rootNode.GetLowestNode();
                return true;
            }
            else if (CurrentNode.RightNode == null)
            {
                var lastNode = CurrentNode;
                do
                {
                    if (CurrentNode.ParentNode == null) return false;
                    CurrentNode = CurrentNode.ParentNode;
                }
                while (BinaryTree<T>.CompareValues(CurrentNode.Value, lastNode.Value, CurrentTree.comparer) <= 0);
                return true;
            }
            else
            {
                CurrentNode = CurrentNode.RightNode.GetLowestNode();
                return true;
            }
        }

        public void Reset()
        {
            CurrentNode = null;
        }
    }
}
