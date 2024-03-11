

/* Igor Jardim-Martins 0754341
 * Prima Morakhia 0753456
 * COIS3020 Assignment 2: Ropes
 * 
 * NOTE: For the optimizations, one is done in split
 * and the other happens in the rebalance
 * */
using System;


public class COIS3020Assignment2
{
    public static void Main(string[] args)
    {


        // TEST CONSTRUCTOR
        Console.WriteLine("\n\n\nConstructor test:");
        string s = "abcdefghijklmnopqrstuvwxyz0123456789";
        Console.WriteLine(s);
        Rope r = new Rope(s);
        r.PrintRope(5);

        //TEST INSERT
        Console.WriteLine("\n\n\nInsert test:");
        r.Insert("INSERT", 24);
        Console.WriteLine(r.ToString() + "");
        r.PrintRope(5);

        //TEST SUBSTRING
        Console.WriteLine("\n\n\nSubstring test:\n");
        Console.WriteLine("Substring at (1,1): " + r.Substring(1,1) + "\n");
        Console.WriteLine("Substring from 0 to full length: " + r.Substring(0, r.Length()-1) + "\n");
        Console.WriteLine("Substring at (5,15): " + r.Substring(5, 15) + "\n");

        //TEST FIND,INDEXOF,CHARAT
        Console.WriteLine("\n\n\nFind test (Find, IndexOf, CharAt:");
        Console.WriteLine("Find start of 0123456789: " + r.Find("0123456789") + "\n");
        Console.WriteLine("Find start of DNE: " + r.Find("DNE") + "\n");
        Console.WriteLine("Find start of 01234test56789: " + r.Find("01234test56789") + "\n\n");
        Console.WriteLine("Find index of a: " + r.IndexOf('a') + "\n");
        Console.WriteLine("Find index of z: " + r.IndexOf('z') + "\n");
        Console.WriteLine("Find index of D: " + r.IndexOf('D') + "\n\n");
        Console.WriteLine("Find char at 5: " + r.CharAt(5) + "\n");


        //TEST DELETE
        Console.WriteLine("\n\n\nDelete test: ");
        Console.WriteLine("Deleting from (5,15) and last index ");
        r.Delete(5,15);
        r.Delete(r.Length() - 1, r.Length() - 1);
        r.PrintRope(5);


        //TEST REVERSE
        Console.WriteLine("\n\n\nReverse test: ");
        Console.WriteLine(r.ToString());
        r.Reverse();
        r.PrintRope(5);
        Console.WriteLine(r.ToString());

        //NOTE: Since no tree has had a depth difference of larger than 2, Rebalance is working as intended
        //      Also by having Insert and Delete fully functional, SPlit and Concatenate also pass testing

        //NOTE: Passing an index outside the rope will lead to an out of bounds exception
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
        if (i > root.length || i < 0)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }
        //Note: Current root is the front of the rope
        Rope addedRope = new Rope(s);
        Node backRope = Split(root, i - 1);
        root = Concatenate(root, addedRope.root);
        root = Concatenate(root, backRope);
        root = Rebalance();

    }






    //Delete the substring S[i, j]
    public void Delete(int i, int j)
    {
        if ((i > root.length || i < 0) || (j > root.length || j < 0))
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }
        //Isolate segemnt to delete and simply rebuild rope without segement to delete
        // Rebalance rope too
        Node endRope = Split(root, j+1);
        Node deletedRope = Split(root, i);

        //Special Case: Requesting substring where end is located on the rightmost node
        if (endRope.left != null && endRope.right != null)
        {
            if (endRope.left.s.Length > 0 && endRope.right.length == 0)
            {
                endRope = endRope.left;
            }
        }
        root = Concatenate(root, endRope);
        root = Rebalance();

    }






    //Return the substring S[i, j]
    public string Substring(int i, int j)
    {
        if ((i > root.length || i < 0) || (j > root.length || j < 0))
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }
        string s = "";


        // Split rope twice to bring out middle segemnet where string is located
        Node endRope = Split(root, j + 1);
        Node middleRope = Split(root, i);

        // Assign root to a temporary pointer and use the 
        // substring located segment as the root so toString can be called to it and assigned
        Node temp = root;
        root = middleRope;
        s = this.ToString();


        //Special Case: Requesting substring where end is located on the rightmost node
        if (endRope.left != null && endRope.right != null)
        {
            if (endRope.left.s.Length > 0 && endRope.right.length == 0)
            {
                endRope = endRope.left;
            }
        }



        // Rebuild original rope and rebalance
        root = temp;
        root = Concatenate(root, middleRope);
        root = Concatenate(root, endRope);
        root = Rebalance();
        return s;

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
                        // If first char is found update return value
                        if (buffer.Length == 0)
                            i = root.s.IndexOf(s[0]) + ropeIndex;

                        // Go through string inside node and match with provided string to see if each char matches
                        for (j = root.s.IndexOf(s[0]); j < root.s.Length && strIndex < s.Length; j++)
                        {
                            // COntinue building buffer if it matches substring
                            if (root.s[j] == s[strIndex])
                            {
                                buffer += s[strIndex];
                                strIndex++;
                            }
                            // Reset Buffer and index traacker if it no longer matches substring
                            else
                            {
                                found = false;
                                buffer = "";
                                strIndex = 0;
                                i = -1;
                            }

                        }

                        s = s.Substring(strIndex);
                        strIndex = 0;

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
            if (root.s.Length > 0)
            {
                c = root.s[i];
            }

            //Case 2: Two childrenm Traverse left or right and update index if going right
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
                // and recursively call its children
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

        // Goes to every node and concatenates its string
        // Internal nodes will concatenate the empty string (defaukt value)
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
            // Print out contents of nonempty roots, internal nodes only print out the cumulative length
            // as the their default is the empty string
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
        if (q.length != 0)
        {
            newRoot.left = p;
            newRoot.right = q;
            newRoot.length = p.length + q.length;
            newRoot.s = "";
        }
        else newRoot = p;


        return newRoot;
    }






    //Split the rope with root p at index i and return the root of the right subtree

    private Node Split(Node p, int i)
    {
        Node q = new Node();
        q.length = 0;
        if (p != null)
        {
            Split(p, i);
        }
        return q;
        void Split(Node p, int i)
        {
            if (p != null)
            {
                // Case 1: Internal Node
                if (p.left != null && p.right != null)
                {
                    // Case where index is on the left side of the tree
                    if (i < p.left.length)
                    {
                        // Add right node to split tree and set right side of root to empty
                        q = Concatenate(p.right, q);
                        p.right = null;

                        // Re-adjust New tree if necessary
                        if (q.left != null &&q.right != null) {
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
                        }

                        //Adjust root on way back up

                        Split(p.left, i);

                    }

                    //Traverse down right side and reduce index when going down
                    else
                    {
                        Split(p.right, i - p.left.length);
                    }

                    //Adjust root on way back up
                    // Compressing Nodes with only 1 child and updating lengths
                    AdjustRoot(p);

                }
                //Case 2: Leaf Node
                else
                {
                    // Create a new node
                    // Split string at leaf node and place right most part in the new node
                    // Concatenate new node to split tree
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




        void AdjustRoot(Node p)
        {

            // Case 1: Empty left and Non-Empty right
            if (p.right == null && p.left != null)
            {

                p.length = p.left.length;
                p.s = p.left.s;
                p.right = p.left.right;
                p.left = p.left.left;


            }
            // Case 2: Empty right and Non-Empty Left
            else if (p.left == null && p.right != null)
            {

                p.length = p.right.length;
                p.s = p.right.s;
                p.left = p.right.left;
                p.right = p.right.right;

            }
            // Case 3: Both Non Empty children
            else if (p.left != null && p.right != null)
            {
                p.length = p.left.length + p.right.length;


            }
            //Case 4: Both empty children
            //        Do nothing
        }



    }




    // Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al.
    private Node Rebalance()
    {
        Node p = root;
        Node[] fibInterval = new Node[50];
        Rebalance(p);

        //After adding all the leaf nodes, go through the array jo
        p = null;
        for (int i = 0; i < fibInterval.Length; i++)
        {
            if (fibInterval[i] != null)
            {
                if (p == null)
                    p = fibInterval[i];
                else
                    p = Concatenate(fibInterval[i], p);
            }
        }
        Compress(p);
        return p;

        //Perform a left to right traversal of the tree 
        // So that
        void Rebalance(Node p)
        {
            if (p != null)
            {
                //Case 1: Leaf Node, Add it to Fibonacci Interval Array
                if (p.s.Length > 0)
                {
                    FibInsert(p.s.Length, p);
                }
                // Case 2: Internal Node
                else
                {

                    Rebalance(p.left);

                    Rebalance(p.right);

                }
            }
        }



        Node FibInsert(int l, Node p)
        {
            int fib0 = 1, fib1 = 2, fib = 0, i = 0;
            //Goes through array concateneting and existing trees along the way
            while (p.length > fib)
            {
                fib = fib0 + fib1; // Fn = Fn-1 + Fn-2
                fib1 = fib0;        // Fn-2 = Fn-1
                fib0 = fib;        // Fn-1 = Fn
                i++;
                // Concatenate if tree is not empty
                if (fibInterval[i] != null)
                {
                    // Concatenate any existing nodes up to the fibonacci index
                    p = Concatenate(fibInterval[i], p);
                    fibInterval[i] = null;
                }

            }
            //Update spot with tree
            fibInterval[i] = p;
            return (p);
        }

        //Supporting method to go through the rebalanced tree and compress if possible
        void Compress(Node p)
        {
            if (p != null)
            {
                //Recursively call Compress
                Compress(p.left);
                Compress(p.right);

                // Compress if needed if both children are nonempty
                if (p.left != null && p.right != null)
                {
                    //If child string can be added to be as smaller as the maximum length, compress
                    // Note that it asks for combining total lengths if less than 5 but I believe it would be better to compress
                    // as much as possible up to the max string length
                    // TO change condition to 5 swap MAX_SUBSTRING_LENGTH to 5
                    if (p.left.s.Length + p.right.s.Length <= MAX_SUBSTRING_LENGTH && p.left.s.Length > 0 && p.right.s.Length > 0)
                    {
                        p.s = p.left.s + p.right.s;
                        p.left = null;
                        p.right = null;
                    }
                }
            }
        }








    }





}


//References:
//Hans-J. Boehm, Russ Atkinson, and Michael Plass, Ropes: an Alternative to Strings, Software - Practice and Experience, Volume 25(12), December 1995, pp 1315-1330
