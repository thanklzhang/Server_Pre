using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserDataMgr<T> : DBEntityMgr<T> where T : DBEntity, new()
{

}

