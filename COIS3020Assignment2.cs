

using System.Data.SqlTypes;

public class COIS3020Assignment2
{
    public static void Main(string[] args)
    {

    }
}

public class Rope
{
    private class Node
    {
        public string s { get; set; }
        public Node left { get; set; }
        public Node right { get; set; }


    }

    //Create a balanced rope from a given string S
    public Rope(string s)
    {
        Node root = Build(s, 0, s.Length - 1);

    }

    // Insert string S at index i
    public void Insert(string s, int i)
    {

    }

    //Delete the substring S[i, j]
    public void Delete(int i, int j)
    {

    }

    //Return the substring S[i, j]
    public string Substring(int i, int j)
    {
        return null;
    }


    // Return the index of the first occurrence of S; -1 otherwise
    public int Find(string s)
    {
        return -1;
    }

    // Return the character at index i
    public char CharAt(int i)
    {
        return '\0';
    }

    // Return the index of the first occurrence of character c
    public int IndexOf(char c)
    {
        return -1;
    }

    // Reverse the string represented by the current rope
    public void Reverse()
    {

    }

    //Return length of the string
    public int Length()
    {
        return 0;
    }

    //Return the string represented by the current rope
    string ToString()
    {
        return null;
    }

    // Print the augmented binary tree of the current rope
    //Time Complexity: O(n)
    void PrintRope()
    {

    }

    //SUPPORTING METHODS

    //Recursively build a balanced rope for S[i, j] and return its root
    private Node Build(string s, int i, int j)
    {
        return null;
    }

    //Return the root of the rope constructed by concatenating two ropes with roots p and q
    private Node Concatenate(Node p, Node q)
    {
        return null;
    }


    //Split the rope with root p at index i and return the root of the right subtree
    private Node Split(Node p, int i)
    {
        return null;
    }

    // Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al.
    private Node Rebalance()
    {
        return null;
    }
}


//References:
//Hans-J. Boehm, Russ Atkinson, and Michael Plass, Ropes: an Alternative to Strings, Software - Practice and Experience, Volume 25(12), December 1995, pp 1315-1330
