using System.Collections.Generic;

using QuinnScript.Objects.Blocks;
using QuinnScript.Objects.Lines;

namespace QuinnScript.Objects;

class Executable
{
    public string Name { get; set; }
    public LineBase[] Lines { get; set; }
    public Dictionary<string, int> LabelIndex { get; set; }
    public Dictionary<int, BlockBase> BlockIndex { get; set; }

    public Executable(string name, LineBase[] lines)
    {
        this.Name = name;
        this.Lines = lines;
        this.LabelIndex = new Dictionary<string, int>();
        this.BlockIndex = new Dictionary<int, BlockBase>();
    }

    public Executable(string name, LineBase[] lines, Dictionary<string, int> labels, Dictionary<int, BlockBase> blocks)
    {
        this.Name = name;
        this.Lines = lines;
        this.LabelIndex = labels;
        this.BlockIndex = blocks;
    }
}
