

public class COIS3020Assignment2
{
    public static void Main(string[] args)
    {

    }
}

public class Rope
{
    protected class Node
    {
        string s { get; set; }
        Node left { get; set; }
        Node right { get; set; }

        
    }

    //Create a balanced rope from a given string S
    public Rope (string s)
    {
        Node n = new Node ();
        n.s = "";
    }


    
}
