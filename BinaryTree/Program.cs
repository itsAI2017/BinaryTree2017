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
        public enum ATCHILDNODE { NO,LEFT,RIGHT}

        public List<NodeString> Nodes = new List<NodeString>
        {
            new NodeString {ID="a", val = 0 },
            new NodeString {ID="b", val = 2 },
            new NodeString {ID="g", val = 5 },
            new NodeString {ID="c", val = 1 },
            new NodeString {ID="e", val = 4 },
            new NodeString {ID="g", val = 0 },
            new NodeString {ID="g", val = 1 },
            new NodeString {ID="d", val = 1 },
            new NodeString {ID="x", val = 0 },


        };

        static void Main(string[] args)
        {
            BinaryTree<NodeString> t = new BinaryTree<NodeString>();

            t.Root = new BinaryTreeNode<NodeString>(
                new NodeString { ID = "a", val = 0 },
                null,null);
            
            CreateNode(t, "a", new NodeString { ID = "b", val = 2 }, ATCHILDNODE.LEFT);
            CreateNode(t, "a", new NodeString { ID = "c", val = 1 }, ATCHILDNODE.RIGHT);
            CreateNode(t, "b", new NodeString { ID = "e", val = 4 }, ATCHILDNODE.LEFT);
            CreateNode(t, "b", new NodeString { ID = "g", val = 5 }, ATCHILDNODE.RIGHT);
            CreateNode(t, "c", new NodeString { ID = "d", val = 1 }, ATCHILDNODE.LEFT);
            CreateNode(t, "c", new NodeString { ID = "x", val = 1 }, ATCHILDNODE.RIGHT);
            CreateNode(t, "x", new NodeString { ID = "g", val = 1 }, ATCHILDNODE.LEFT);
            CreateNode(t, "e", new NodeString { ID = "g", val = 1 }, ATCHILDNODE.LEFT);

            traverseTree(t.Root);
                Console.ReadKey();

            
        }

        private static void CreateNode(BinaryTree<NodeString> tree, string afterID, NodeString newNode, ATCHILDNODE child )
        {
            Console.WriteLine("Finding {0} to place {1}", afterID,newNode.ID);

            var visited = new Stack<BinaryTreeNode<NodeString>>();
            visited.Push(tree.Root);
            BinaryTreeNode<NodeString> node = findNode(tree.Root, visited, afterID );
            if (node != null)
            {
                    switch (child)
                    {
                        case ATCHILDNODE.LEFT:
                        Console.WriteLine("Placed {0} Left of {1}", newNode.ID,node.Value.ID);
                        node.Left = new BinaryTreeNode<NodeString>(newNode);
                        break;
                            
                        case ATCHILDNODE.RIGHT:
                        Console.WriteLine("Placed {0} right of {1}", newNode.ID, node.Value.ID);
                        node.Right = new BinaryTreeNode<NodeString>(newNode);
                            break;
                     
                    }
           }
                
                       else { Console.WriteLine("Node {0} not found", afterID); }
            
        }

        private static BinaryTreeNode<NodeString> findNode(BinaryTreeNode<NodeString> current, 
                            Stack<BinaryTreeNode<NodeString>> visited, string v)
        {
            if (current != null)
            {
                Console.WriteLine("Visiting {0}", current.Value.ID);
                if (current.Value.ID == v)
                    return current;
                else
                {
                    if (!visited.Contains(current))
                        visited.Push(current);
                    current = findNode(current.Left, visited, v);
                }
            }
            else if (current == null)
            {
                if (visited.Count() > 0)
                {
                    var neighbour = visited.Peek();
                    Console.WriteLine("backtracking into {0}", neighbour.Value.ID);
                    // if the neighbour to the right of the last node visited is worth looking at 
                        current = visited.Pop().Right;
                    return findNode(current, visited, v); // Continue regardless
                }
                else return null;
            }
            return current;
        }

        static public void traverseTree (BinaryTreeNode<NodeString> node)
        {
            if(node != null)
            {
                Console.WriteLine("{0}, {1}", node.Value.ID, node.Value.val);
                traverseTree(node.Left);
                traverseTree(node.Right);
            }
        }
    }
}
