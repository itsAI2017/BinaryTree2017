using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Example based on  https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx
namespace BinaryTree
{
    class Program
    {
        public enum CHILDNODE { LEFT,RIGHT}

        static void Main(string[] args)
        {
            BinaryTree<NodeString> t = new BinaryTree<NodeString>();

            t.Root = new BinaryTreeNode<NodeString>(
                new NodeString { ID = "a", val = 0 },
                null,null);


        }
        public void InsertAfter(NodeString node, BinaryTree<NodeString> tree)
        {

        }

    }
}
