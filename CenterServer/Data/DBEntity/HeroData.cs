using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HeroData : DBEntity
{
    [PrimaryKey]
    public int id;

    public int userId;

    public int configId;

    public int level;

    public void AddLevel(int value)
    {
        this.level += value;
    }
}

