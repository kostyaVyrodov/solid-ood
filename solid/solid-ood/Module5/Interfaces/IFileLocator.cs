﻿using System.IO;

namespace SolidOod.Module5.Interfaces
{
    public interface IFileLocator
    {
        FileInfo GetFileInfo(int id);
    }
}
