using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBinaryTree
{
    public class BinaryTree<T> : IEnumerable
        where T : IComparable<T>
    {
        internal TreeNode<T> rootNode;
        internal IComparer<T> comparer;

        public delegate void onInsertDelegate(BinaryTree<T> sender, T data);
        public event onInsertDelegate onInsert;

        public delegate bool onDeleteDelegate(BinaryTree<T> sender, T data);
        public event onDeleteDelegate onDelete;

        public BinaryTree()
        {
            this.rootNode = null;
            this.comparer = null;
        }

        /// <summary>
        /// Initialize tree and add comparator for elements.
        /// </summary>
        /// <param name="comparer">Comparator for elements.</param>
        public BinaryTree(IComparer<T> comparer)
        {
            this.rootNode = null;
            this.comparer = comparer;
        }

        /// <summary>
        /// Compare two elements by IComparable if <see cref="comparer"/>==null. Else campare by IComparer.
        /// </summary>        
        internal static int CompareValues(T value1, T value2, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                return value1.CompareTo(value2);
            }
            else
            {
                return comparer.Compare(value1, value2);
            }
        }

        /// <summary>
        /// Inserts element to a tree.
        /// </summary>
        /// <param name="data">Inserted value.</param>
        public void Insert(T data)
        {
            if (rootNode == null)
            {
                rootNode = new TreeNode<T>(data);
            }
            else
            {
                rootNode.InsertUnderNode(data, comparer);
            }
            onInsert?.Invoke(this, data);
        }

        /// <summary>
        /// Finds element in current tree.
        /// </summary>
        /// <param name="value">Sought value.</param>
        public List<TreeNode<T>> Find(T value, IComparer<T> comp) => rootNode.FindUnderNode(value, comparer);

        /// <summary>
        /// Deletes element in current tree.
        /// </summary>
        /// <param name="value">Deleted value.</param>
        public void Delete(T data)
        {
            List<TreeNode<T>> deleteList = Find(data, comparer);
            foreach (TreeNode<T> node in deleteList)
            {
                DeleteNode(node);
            }
            onDelete?.Invoke(this, data);
        }

        /// <summary>
        /// Deletes node in current tree.
        /// </summary>
        /// <param name="node">Deleted node.</param>
        public void DeleteNode(TreeNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("Cannot delete null node");

            if (node.ParentNode == null)
            {
                if (node.RightNode == null) rootNode = node.LeftNode;
                else if (node.LeftNode == null) rootNode = node.RightNode;
                else
                {
                    var tempNode = node.RightNode.GetLowestNode();
                    tempNode.LeftNode = node.LeftNode;
                    node.LeftNode.ParentNode = tempNode;
                    rootNode = node.RightNode;
                }
            }
            else node.DeleteCurrentNode();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TreeEnumerator<T>(this);
        }
    }
}
