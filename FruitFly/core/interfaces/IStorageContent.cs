
// Pavel Prchal, 2020

using System;
using System.Linq;
using System.Globalization;
using fruitfly.core;

namespace fruitfly.core
{
    public interface IStorageContent
    {
        string[] BuildFolderStack();
    }
}