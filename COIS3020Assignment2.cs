

using System;
using System.Data.SqlTypes;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

public class COIS3020Assignment2
{
    public static void Main(string[] args)
    {
        string testS = "";
        for (int i = 0; i < 100; i++)
            testS += (i + ",");
        string s = "abcdefghijklmnopqrstuvwxyz0_abcdefghijklmnopqrstuvwxyz0";
        Console.WriteLine(s.Length);
        Rope r = new Rope(s);

//        r.Delete(5,35);
        r.PrintRope(5);
        
        r.Insert("YIPPE", 5);
        r.PrintRope(5);
        Console.WriteLine(r.ToString() + " ");
        r.Reverse();
        r.PrintRope(5);
        Console.WriteLine(r.ToString() + " " + r.IndexOf('d') + " " + r.CharAt(15) + " " + r.Length());
        //Console.WriteLine(r.Find("0zyxwvutsrqponmlkjihgfedcba_0zyxwvutsrqponmlkjihgfedcba"));

    }
}

public class Rope
{
    private Node root = null;
    private const int MAX_SUBSTRING_LENGTH = 10;
    private class Node
    {
        internal int length { get; set; }
        internal string s { get; set; }
        internal Node left { get; set; }
        internal Node right { get; set; }


    }

    //Create a balanced rope from a given string S
    public Rope(string s)
    {
        root = Build(s, 0, s.Length - 1);
    }






    // Insert string S at index i
    public void Insert(string s, int i)
    {
        Node addedSegment = Build(s, 0, s.Length - 1);
        this.PrintRope(5);
        Node endSegment = Split(root, i);
        Node startSegment = Concatenate(root, addedSegment);
        root = Concatenate(startSegment, endSegment);

    }






    //Delete the substring S[i, j]
    public void Delete(int i, int j)
    {
        Node deletedSegment = Split(root, i);
        this.PrintRope(5);
        Node endSegment = Split(deletedSegment, j);
        root = Concatenate(root, endSegment);
    }






    //Return the substring S[i, j]
    public string Substring(int i, int j)
    {
        return null;
    }







    // Return the index of the first occurrence of S; -1 otherwise
    public int Find(string s)
    {
        int i = -1;
        int ropeIndex = 0;
        int strIndex = 0;
        string buffer = "";
        bool found = false;

        if (root != null && s != null)
            Find(root, ref s);

        void Find(Node root, ref string s)
        {

            if (!found)
            {
                //Traverse left root if not empty
                if (root.left != null)
                    Find(root.left, ref s);


                //Check if root contains character
                if (s.Length > 0)
                    if (root.s.Contains(s[0]))
                    {
                        int j = 0;
                        if (buffer.Length == 0)
                            i = root.s.IndexOf(s[0]) + ropeIndex;
                        for (j = root.s.IndexOf(s[0]); j < root.s.Length && strIndex < s.Length; j++)
                        {
                            if (root.s[j] == s[strIndex])
                            {
                                buffer += s[strIndex];
                                strIndex++;
                            }
                            else
                            {
                                found = false;
                                buffer = "";
                                strIndex = 0;
                                i = -1;
                            }
                            //Console.WriteLine("Buffer: " + buffer);

                        }
                        s = s.Substring(strIndex);
                        strIndex = 0;
                        // Console.WriteLine(s + " " + root.s);

                        if (s.Length == 0)
                            found = true;
                    }

                //Increment index by length of substring at root so that it can be tracked when traversing
                //Will increment by 0 at a non-leaf node
                ropeIndex += root.s.Length;

                //Traverse right root if not empty
                if (root.right != null && s.Length > 0)
                    Find(root.right, ref s);
            }
        }
        //Console.WriteLine(buffer);
        return i;
    }






    // Return the character at index i
    // Will throw exception if index out of bounds
    public char CharAt(int i)
    {

        char c = '\0';


        //Throw out of bounds error if given index greater than length of root
        //Else start recursive check
        if (i > root.length || i < 0)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }
        else
        {
            CharAt(root, i);
        }

        //Recursive Supporting method for finding char at index
        //Uses the length property of each node to traverse down the tree in O(log n) time
        void CharAt(Node root, int i)
        {

            //Case 1: No children (leaf node)
            if (root.left == null && root.right == null)
            {
                c = root.s[i];
            }

            //Case 2: Two children
            else
            {
                if (i < root.left.length)
                {
                    CharAt(root.left, i);
                }
                else
                {
                    CharAt(root.right, i - root.left.length);
                }
            }



        }
        return c;
    }






    // Return the index of the first occurrence of character c
    //Perform an Inorder traversal of the rope to find the first instance of char c
    public int IndexOf(char c)
    {

        int i = -1;
        int index = 0;
        bool found = false;

        if (root != null)
            IndexOf(root, c);

        //Recursive call for InOrder traversal to find index of char
        //If not found will return -1
        void IndexOf(Node root, char c)
        {
            //No need to check if root is null as code ensures that it wont traverse down empty nodes
            //Special case for empty root is already checked at base of recursive call
            if (!found)
            {
                //Traverse left root if not empty
                if (root.left != null)
                    IndexOf(root.left, c);

                //Check if root contains character
                if (root.s.Contains(c))
                {
                    found = true;
                    i = root.s.IndexOf(c) + index;
                }

                //Increment index by length of substring at root so that it can be tracked when traversing
                //Will increment by 0 at a non-leaf node
                index += root.s.Length;

                //Traverse right root if not empty
                if (root.right != null)
                    IndexOf(root.right, c);
            }
        }
        return i;
    }







    // Reverse the string represented by the current rope
    public void Reverse()
    {
        root = Reverse(root);

        Node Reverse(Node root)
        {
            if (root != null)
            {
                //If root is a leaf node reverse the substring inside
                if (root.left == null && root.right == null)
                {
                    char[] charArray = root.s.ToCharArray();
                    Array.Reverse(charArray);
                    root.s = new string(charArray);
                }

                //Else reverse the 2 children
                else
                {
                    Node temp = root.left;
                    root.left = root.right;
                    root.right = temp;
                    root.left = Reverse(root.left);
                    root.right = Reverse(root.right);
                }

            }
            return root;
        }
    }







    //Return length of the string (It is stored at the root)
    public int Length()
    {
        return root.length;
    }






    //Return the string represented by the current rope
    public string ToString()
    {
        string ret = "";
        ToString(root);

        void ToString(Node root)
        {
            if (root != null)
            {
                ToString(root.left);
                ret += root.s;
                ToString(root.right);
            }
        }

        return ret;
    }






    // Print the augmented binary tree of the current rope
    //Courtesy of Professor Brian Patrick from COIS2020
    public void PrintRope(int indent)
    {
        Console.WriteLine("Printing rope:");
        if (indent >= 0)
            PrintRope(root, indent);

    }
    private void PrintRope(Node root, int indent)
    {
        if (root != null)
        {
            PrintRope(root.right, indent + 3);
            Console.WriteLine(new String(' ', indent) + root.s + " " + root.length);

            PrintRope(root.left, indent + 3);
        }

    }









    //SUPPORTING METHODS

    //Recursively build a balanced rope for S[i, j] and return its root
    private Node Build(string s, int i, int j)
    {
        Node root = null;

        if (s != null)
        {
            root = Build(s, i, j, root);


            //Supportive Method for Build which recursively calls
            Node Build(string s, int i, int j, Node root)
            {
                root = new Node();
                root.s = "";

                //Create substring with current version of s
                string subS = s.Substring(i, j + 1);
                root.length = subS.Length;


                //If substring is small enough to store a string treat it as a leaf
                if (subS.Length < MAX_SUBSTRING_LENGTH)
                {
                    root.s = subS;
                }

                //If string not short enought to be placed in a leaf node, create 2 children and place them in
                else
                {
                    //Instantiate children
                    root.right = new Node();
                    root.left = new Node();

                    //Determine the lengths of left and right children (Accounting for odd numbered root lengths)
                    root.right.length = root.length / 2;
                    root.left.length = root.length - root.right.length;

                    //Make their internal substrings empty to flag them as an internal node
                    root.left.s = "";
                    root.right.s = "";

                    //Recursively Build the left and right subtrees with their positions going
                    root.left = Build(s, i, root.left.length - 1, root.left);
                    root.right = Build(s, i + root.left.length, root.right.length - 1, root.right);

                }
                return root;
            }
        }

        return root;

    }






    //Return the root of the rope constructed by concatenating two ropes with roots p and q
    private Node Concatenate(Node p, Node q)
    {
        Node newRoot = new Node();


        //Create a new node with p and q as its children and calculate length
        newRoot.left = p;
        newRoot.right = q;
        newRoot.length = p.length + q.length;
        newRoot.s = "";


        return newRoot;
    }






    //Split the rope with root p at index i and return the root of the right subtree

    private Node Split(Node p, int i)
    {
        int n = i;
        bool isRooted = false;
        Node q = new Node();
        q.length = 0;
        if (p != null)
        {
            Console.WriteLine("start");
            p.length = p.length - i;
            Split(p, i);


            //p = Adjust(p);



        }

        void Split(Node p, int i)
        {
            if (p != null)
            {
                // Case 1: Internal Node
                if (p.left != null && p.right != null)
                {
                    Console.WriteLine(i);
                    if (i < p.left.length)
                    {
                        q = Concatenate(p.right, q);
                        p.right = null;



                        // Re-adjust New tree if necc
                        if (q.length == q.left.length && q.right.length == 0)
                        {
                            q.length = q.left.length;
                            q = q.left;
                        }
                        else if (q.length == q.right.length && q.left.length == 0)
                        {
                            q.length = q.right.length;
                            q = q.right;
                        }
                        p.length = p.left.length;


                        Split(p.left, i);

                    }

                    //Traverse down right side and reduce index when going down
                    else
                    {

                        Split(p.right, i - p.left.length);

                    }


                    //Adjust root on way back up
                    if (p.right == null && p.left != null)
                    {
                        p.length = p.left.length;
                        p.right = p.left.right;
                        p.left = p.left.left;

                    }


                    else if (p.left == null && p.right != null)
                    {
                        p.length = p.right.length;
                        p.left = p.right.left;
                        p.right = p.right.right;
                    }


                    else if (p.left != null && p.right != null)
                    {
                        
                            p.length = p.left.length + p.right.length;
                    }



                }
                //Case 2: Leaf Node
                else
                {
                    Node tempNode = new Node();
                    string tempS = p.s.Substring(i);
                    p.s = p.s.Remove(i);
                    p.length = p.s.Length;
                    tempNode.s = tempS;
                    tempNode.length = tempS.Length;
                    q = Concatenate(tempNode, q);
                }
            }




        }

        return q;
    }






    //
    //private Node Split(Node p, int i)
    //    {
    //        Node newRoot = p;
    //        Node q = newRoot;
    //        Console.WriteLine(i);


    //        //Case 1: Internal Node
    //        if (p.left != null && p.right != null)
    //        {
    //            if (i < p.left.length)
    //            {
    //                newRoot = Concatenate(p.right, newRoot);
    //                //newRoot = p.right;
    //                p.right = null;
    //                p.length = p.left.length;
    //                newRoot = Split(p.left, i);


    //            }
    //            else { 
    //                newRoot = Split(p.right, i - p.left.length);
    //            }

    //        }
    //        //Case 2: Leaf Node
    //        else
    //        {

    //            Node tempNode = new Node();
    //            string tempS = p.s.Substring(i);
    //            p.s.Remove(i);
    //            p.length = p.s.Length;
    //            tempNode.s = tempS;
    //            tempNode.length = tempS.Length;
    //            newRoot = Concatenate(tempNode, newRoot);
    //        }

    //        return newRoot;



    // Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al.
    private Node Rebalance()
    {
        return null;
    }





}


//References:
//Hans-J. Boehm, Russ Atkinson, and Michael Plass, Ropes: an Alternative to Strings, Software - Practice and Experience, Volume 25(12), December 1995, pp 1315-1330
