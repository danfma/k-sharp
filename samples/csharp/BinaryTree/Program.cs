using static System.Console;

var tree = new BinaryTree();
WriteLine(tree.IsEmpty); // True
tree.Add(5);
WriteLine(tree.IsEmpty); // False
tree.Add(3);
tree.Add(7);
tree.Add(1);

public class BinaryTree
{
    private TreeNode? _root;

    public bool IsEmpty => _root is null;
    
    public void Add(int value)
    {
        if (_root is null)
        {
            _root = new TreeNode(value);
        }
        else
        {
            _root.Add(value);
        }
    }
    
    public bool Contains(int value)
    {
        return _root?.Contains(value) ?? false;
    }
    
    public IEnumerable<int> EnumerateInOrder()
    {
        return _root?.EnumerateInOrder() ?? [];
    }

    public class TreeNode(int value)
    {
        public int Value { get; } = value;
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }

        public TreeNode Add(int value)
        {
            if (value < Value)
            {
                return Left is null
                    ? Left = new TreeNode(value)
                    : Left.Add(value);
            }

            if (value > Value)
            {
                return Right is null
                    ? Right = new TreeNode(value)
                    : Right.Add(value);
            }

            return this;
        }

        public bool Contains(int value)
        {
            return value == Value || (Left?.Contains(value) ?? Right?.Contains(value) ?? false);
        }

        public IEnumerable<int> EnumerateInOrder()
        {
            foreach (var value in Left?.EnumerateInOrder() ?? [])
            {
                yield return value;
            }
            
            yield return Value;
            
            foreach (var value in Right?.EnumerateInOrder() ?? [])
            {
                yield return value;
            }
        }
    }
}
