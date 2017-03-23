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

        static public List<NodeString> Hnodes = new List<NodeString>
        {
            new NodeString {ID="a", val = 9 },
            new NodeString {ID="b", val = 5 },
            new NodeString {ID="c", val = 2 },
            new NodeString {ID="e", val = 4 },
            new NodeString {ID="d", val = 1 },
            new NodeString {ID="x", val = 2 },
            new NodeString {ID="g", val = 0 },

        };

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
            //CreateNode(t, "e", new NodeString { ID = "g", val = 1 }, ATCHILDNODE.RIGHT);

            var visited = new Stack<BinaryTreeNode<NodeString>>();
            visited.Push(t.Root);
            List<BinaryTreeNode<NodeString>> solutionPath = new List<BinaryTreeNode<NodeString>>();

            BinaryTreeNode<NodeString> Goal = dfs(t.Root, visited, "g", solutionPath);

            foreach (var item in solutionPath)
                Console.WriteLine("Node {0} at a cost of {1} ", 
                                    item.Value.ID, 
                                    item.Value.val);

            //traverseTree(t.Root);
            Console.ReadKey();

            
        }

        private static BinaryTreeNode<NodeString> bestfirst(BinaryTreeNode<NodeString> current,
                    Stack<BinaryTreeNode<NodeString>> visited,
                    string goal, List<BinaryTreeNode<NodeString>> solutionPath)
        {
            if (current != null)
            {

                Console.WriteLine("Visiting {0}", current.Value.ID);
                if (current.Value.ID == goal)
                {
                    solutionPath.Add(current);
                    return current;
                }
                else
                {
                    if (!visited.Contains(current))
                    {
                        solutionPath.Add(current);
                        visited.Push(current);
                    }
                    
                    current = Compare(current.Left, current.Right);
                    current = bestfirst(current, visited, goal, solutionPath);
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
                    if (solutionPath.Count > 0)
                        solutionPath.Remove(current);
                    current = bestfirst(current, visited, goal, solutionPath); // Continue regardless
                }
                else return null;
            }
            return current;
        }

        private static BinaryTreeNode<NodeString> Compare(BinaryTreeNode<NodeString> left, BinaryTreeNode<NodeString> right)
        {
            if (left == null && right != null)
                return right;
            else if (left != null && right == null)
                return left;
            else if(left != null && right != null)
            {
                var lefth = Hnodes.First(l => l.ID == left.Value.ID).val;
                var righth = Hnodes.First(l => l.ID == right.Value.ID).val;
                if (lefth <= righth)
                    return left;
                else return right;
            }
            return null;
        }

        private static BinaryTreeNode<NodeString> dfs(BinaryTreeNode<NodeString> current,
                            Stack<BinaryTreeNode<NodeString>> visited, 
                            string goal, List<BinaryTreeNode<NodeString>> solutionPath )
        {
            if (current != null)
            {
                Console.WriteLine("Visiting {0}", current.Value.ID);
                if (current.Value.ID == goal)
                {
                    solutionPath.Add(current);
                    return current;
                }
                else
                {
                    if (!visited.Contains(current))
                    {
                        solutionPath.Add(current);
                        visited.Push(current);
                    }
                    current = dfs(current.Left, visited, goal, solutionPath);
                }
            }
            else if (current == null)
            {
                if (visited.Count() > 0)
                {
                    var candidate = visited.Peek();
                    Console.WriteLine("backtracking into {0}", candidate.Value.ID);
                    // if there is no neighbour to the right then this node will not
                    // form part of the solution
                    // look ahead to the right to see if the node about to be popped 
                    // will be part of the solution or abandoned
                    if (candidate.Right == null)
                        solutionPath.Remove(candidate);
                    // pop the right and set it to current. If it's null we'll be popping again 
                    // if there was a left then we wouldn't be here anyway
                    current = visited.Pop().Right;
                    current = dfs(current, visited, goal, solutionPath); // Continue regardless
                }
                else return null;
            }
            return current;
        }
        
        private static void CreateNode(BinaryTree<NodeString> tree, string afterID, NodeString newNode, ATCHILDNODE child )
        {
            //Console.WriteLine("Finding {0} to place {1}", afterID,newNode.ID);

            var visited = new Stack<BinaryTreeNode<NodeString>>();
            visited.Push(tree.Root);
            BinaryTreeNode<NodeString> node = findNode(tree.Root, visited, afterID );
            if (node != null)
            {
                    switch (child)
                    {
                        case ATCHILDNODE.LEFT:
                        //Console.WriteLine("Placed {0} Left of {1}", newNode.ID,node.Value.ID);
                        node.Left = new BinaryTreeNode<NodeString>(newNode);
                        break;
                            
                        case ATCHILDNODE.RIGHT:
                        //Console.WriteLine("Placed {0} right of {1}", newNode.ID, node.Value.ID);
                        node.Right = new BinaryTreeNode<NodeString>(newNode);
                            break;
                     
                    }
           }
                
                       //else { Console.WriteLine("Node {0} not found", afterID); }
            
        }

        private static BinaryTreeNode<NodeString> findNode(BinaryTreeNode<NodeString> current, 
                            Stack<BinaryTreeNode<NodeString>> visited, string v)
        {
            if (current != null)
            {
                //Console.WriteLine("Visiting {0}", current.Value.ID);
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
                    //Console.WriteLine("backtracking into {0}", neighbour.Value.ID);
                    // if the neighbour to the right of the last node visited is worth looking at 
                        current = visited.Pop().Right;
                    current = findNode(current, visited, v); // Continue regardless
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
